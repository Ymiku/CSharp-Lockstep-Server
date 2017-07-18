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

namespace SpaceNetServer.logic.match
{
    /// <summary>
    /// 战斗匹配逻辑处理类
    /// </summary>
   public class MatchHandler:AbsOnceHandler, HandlerInterface
    {
       /// <summary>
       /// 多线程处理类中  防止数据竞争导致脏数据  使用线程安全字典
       /// 玩家所在匹配房间映射
       /// </summary>
       ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();
        /// <summary>
        /// 副本id与房间列表映射
        /// </summary>
        ConcurrentDictionary<int, List<int>> instanceMap = new ConcurrentDictionary<int, List<int>>();
        /// <summary>
        /// 房间id与模型映射
        /// </summary>
        ConcurrentDictionary<int, MatchRoom> roomMap = new ConcurrentDictionary<int, MatchRoom>();
       /// <summary>
       /// 回收利用过的房间对象再次利用，减少gc性能开销
       /// </summary>
       ConcurrentStack<MatchRoom> cache = new ConcurrentStack<MatchRoom>();
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
            switch (message.command) { 
                case MatchProtocol.ENTER_CREQ:
                    enter(token);
                    break;
                case MatchProtocol.LEAVE_CREQ:
                    leave(token);
                    break;
            }
        }

        private void leave(UserToken token)
        {
            //取出用户唯一ID
            int userId = getUserId(token);
            Console.WriteLine("用户取消匹配" + userId);
            //判断用户是否有房间映射关系
            if (!userRoom.ContainsKey(userId)) {
                return;
            }
            //获取用户所在房间ID
            int roomId = userRoom[userId];
            //判断是否拥有此房间
            if (roomMap.ContainsKey(roomId)) {
                MatchRoom room = roomMap[roomId];
                //根据用户所在的队伍 进行移除
                if (room.team.Contains(userId)) room.team.Remove(userId);
                //移除用户与房间之间的映射关系
                userRoom.TryRemove(userId, out roomId);
                //如果当前用户为此房间最后一人 则移除房间 并丢进缓存队列
                if (room.team.Count == 0) {
                    roomMap.TryRemove(roomId, out room);
                    cache.Push(room);
                }
            }
        }

        private void enter(UserToken token) {
            token.instanceID = 0;
            int userId=getUserId(token);
            Console.WriteLine("用户开始匹配" + userId);
            //判断玩家当前是否正在匹配队列中 
            if (!userRoom.ContainsKey(userId)) {
                MatchRoom room = null;
                bool isenter = false;
                //当前是否有等待中的房间
                if (roomMap.Count > 0)
                {
                    //遍历当前所有等待中的房间
                    foreach (MatchRoom item in roomMap.Values)
                    {
                        //如果没满员
                        if (item.teamMax > item.team.Count) {
                            room = item;
                            room.team.Add(userId);
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
                            cache.TryPop(out room);
                            room.team.Add(userId);
                            roomMap.TryAdd(room.id, room);
                            userRoom.TryAdd(userId, room.id);
                        }
                        else
                        {
                            room = new MatchRoom();
                            room.id = index.GetAndAdd();
                            room.team.Add(userId);
                            roomMap.TryAdd(room.id, room);
                            userRoom.TryAdd(userId, room.id);
                        }
                    }

                }
                else { 
                    //没有等待中的房间
                    //直接从缓存列表中找出可用房间 或者创建新房间
                    if (cache.Count > 0)
                    {
                        cache.TryPop(out room);
                        room.team.Add(userId);
                        roomMap.TryAdd(room.id, room);
                        userRoom.TryAdd(userId, room.id);
                    }
                    else
                    {
                        room = new MatchRoom();
                        room.id = index.GetAndAdd();
                        room.team.Add(userId);
                        roomMap.TryAdd(room.id, room);
                        userRoom.TryAdd(userId, room.id);
                    }
                
                }
                //不管什么方式进入房间 ，判断房间是否满员，满了就开始选人并将当前房间丢进缓存队列
                if (room.team.Count == room.teamMax) { 
                    // 这里通知选人模块 开始选人了
                    EventUtil.createSelect(room.team);
                    writeToUsers(room.team.ToArray(), GetType(), 0, MatchProtocol.ENTER_SELECT_BRO, null);
                    //移除玩家与房间映射
                    foreach (int item in room.team)
                    {
                        int i;
                        userRoom.TryRemove(item, out i);
                    }
                    //重置房间数据 供下次使用
                    room.team.Clear();
                    //将房间从等待房间表中移除
                    roomMap.TryRemove(room.id, out room);
                    //将房间丢进缓存表 供下次使用
                    cache.Push(room);
                }

            }
        }
  
        public override byte GetType()
        {
            return Protocol.TYPE_MATCH;
        }
    }
}
