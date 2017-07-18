using GameProtocol;
using SpaceNetServer.dao.model;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using SpaceNetServer.tool;
using GameProtocol.dto.explore;

namespace SpaceNetServer.logic.explore
{
    /// <summary>
    /// 战斗匹配逻辑处理类
    /// </summary>
    public class ExploreHandler : AbsOnceHandler, HandlerInterface
    {
        /// <summary>
        /// 多线程处理类中  防止数据竞争导致脏数据  使用线程安全字典
        /// 玩家所在匹配房间映射
        /// </summary>
        ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();
        /// <summary>
        /// 副本id与房间列表映射
        /// </summary>
        object mapLocker = new object();
        Dictionary<int, List<int>> instanceMap = new Dictionary<int, List<int>>();
        /// <summary>
        /// 房间id与模型映射
        /// </summary>
        ConcurrentDictionary<int,ExploreRoom> roomMap = new ConcurrentDictionary<int, ExploreRoom>();
        /// <summary>
        /// 回收利用过的房间对象再次利用，减少gc性能开销
        /// </summary>
        ConcurrentStack<ExploreRoom> cache = new ConcurrentStack<ExploreRoom>();
        /// <summary>
        /// 房间ID自增器
        /// </summary>
        ConcurrentInteger index = new ConcurrentInteger();

        public void ClientClose(NetFrame.UserToken token, string error)
        {
            leave(token);
        }

        public void MessageReceive(NetFrame.UserToken token, NetFrame.auto.SocketModel message)
        {
            switch (message.command)
            {
                case ExploreProtocol.ENTER_CREQ:
                    enter(token, message.area);
                    break;
                case ExploreProtocol.INFORM_CREQ:
                    inform(token,message.area);
                    break;
                case ExploreProtocol.LEAVE_CREQ:
                    leave(token);
                    break;
                default:
                    roomMap[userRoom[getUserId(token)]].MessageReceive(token, message);
                    break;
            }
        }
        private void leave(UserToken token)
        {
            //取出用户唯一ID
            int userId = getUserId(token);
            Console.WriteLine("角色离开匹配   ID:" + userId);
            //判断用户是否有房间映射关系
            if (!userRoom.ContainsKey(userId))
            {
                return;
            }
            //获取用户所在房间ID
            int roomId = userRoom[userId];
            //判断是否拥有此房间
            if (roomMap.ContainsKey(roomId))
            {
                ExploreRoom room = roomMap[roomId];
                //根据用户所在的队伍 进行移除
                PlayerModel m;
                if (room.playerDic.ContainsKey(userId)) room.playerDic.TryRemove(userId,out m);
                //移除用户与房间之间的映射关系
                userRoom.TryRemove(userId, out roomId);
                //如果当前用户为此房间最后一人 则移除房间 并丢进缓存队列
                lock (mapLocker)
                {
                    room.PlayerLeave(token);
                    if (room.playerDic.Count == 0)
                    {
                        List<int> roomList;
                        instanceMap.TryGetValue(room.area, out roomList);
                        roomList.Remove(roomId);
                        if (roomList.Count == 0)
                            instanceMap.Remove(roomId);
                        roomMap.TryRemove(roomId, out room);
                        room.Clear();
                        cache.Push(room);
                    }
                }
            }
        }
        private void enter(UserToken token,int area)
        {
            write(token, ExploreProtocol.ENTER_SRES, 0);
        }
        private void inform(UserToken token,int area)
        {
            Console.WriteLine("角色开始匹配");
            token.instanceID = area;
            int userId = getUserId(token);
            //判断玩家当前是否正在匹配队列中 
            if (!userRoom.ContainsKey(userId))
            {
                Console.WriteLine("用户没在某个房间内，非断线重连");
                ExploreRoom room = null;
                bool isenter = false;
                //当前是否有等待中的房间
                lock (mapLocker)
                {
                    List<int> instanceList;
                    if (!instanceMap.TryGetValue(area, out instanceList)) {
                        instanceList = new List<int>();
                        instanceMap.Add(area,instanceList);
                    }
                    if (instanceList.Count > 0)
                    {
                        //遍历当前所有等待中的房间
                        foreach (int i in instanceList)
                        {
                            //如果没满员
                            if (roomMap[i].playerMax > roomMap[i].playerDic.Count)
                            {
                                Console.WriteLine("找到一个未满的房间");
                                room = roomMap[i];
                                //如果队伍1没有满员 则进入队伍1 否则进入队伍2
                                room.PlayerEnter(token);
                                //添加玩家与房间的映射关系
                                isenter = true;
                                userRoom.TryAdd(userId, room.id);
                                break;
                            }
                        }
                        if (!isenter)
                        {
                            //走到这里 说明等待中的房间全部满员了
                            if (cache.Count > 0)
                            {
                                Console.WriteLine("满员 缓存");
                                cache.TryPop(out room);
                                room.Creat();
                                room.PlayerEnter(token);
                                roomMap.TryAdd(room.id, room);
                                userRoom.TryAdd(userId, room.id);
                                instanceMap.TryGetValue(area,out instanceList);
                                instanceList.Add(room.id);
                            }
                            else
                            {
                                Console.WriteLine("满员 新建");
                                room = new ExploreRoom();
                                room.Creat();
                                room.id = index.GetAndAdd();
                                room.PlayerEnter(token);
                                roomMap.TryAdd(room.id, room);
                                userRoom.TryAdd(userId, room.id);
                                instanceMap.TryGetValue(area,out instanceList);
                                instanceList.Add(room.id);
                            }
                        }

                    }
                    else
                    {
                        //没有等待中的房间
                        //直接从缓存列表中找出可用房间 或者创建新房间
                        if (cache.Count > 0)
                        {
                            Console.WriteLine("get room from cache");
                            cache.TryPop(out room);
                            room.area = area;
                            room.Creat();
                            room.PlayerEnter(token);
                            roomMap.TryAdd(room.id, room);
                            userRoom.TryAdd(userId, room.id);
                            instanceMap.TryGetValue(area,out instanceList);
                            instanceList.Add(room.id);
                        }
                        else
                        {
                            Console.WriteLine("创建新房间");
                            room = new ExploreRoom();
                            room.area = area;
                            room.id = index.GetAndAdd();
                            room.Creat();
                            room.PlayerEnter(token);
                            roomMap.TryAdd(room.id, room);
                            userRoom.TryAdd(userId, room.id);
                            instanceMap.TryGetValue(area, out instanceList);
                            instanceList.Add(room.id);
                        }
                    }
                }
            }
        }
        public override byte GetType()
        {
            return Protocol.TYPE_EXPLORE;
        }
    }
}
