using GameProtocol.dto;
using SpaceNetServer.tool;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNetServer.logic.fight
{
   public class FightHandler :AbsMulitHandler, HandlerInterface
    {
        /// <summary>
        /// 多线程处理类中  防止数据竞争导致脏数据  使用线程安全字典
        /// 玩家所在匹配房间映射
        /// </summary>
        ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();
        /// <summary>
        /// 房间id与模型映射
        /// </summary>
        ConcurrentDictionary<int, FightRoom> roomMap = new ConcurrentDictionary<int, FightRoom>();
        /// <summary>
        /// 回收利用过的房间对象再次利用，减少gc性能开销
        /// </summary>
        ConcurrentStack<FightRoom> cache = new ConcurrentStack<FightRoom>();
        /// <summary>
        /// 房间ID自增器
        /// </summary>
        ConcurrentInteger index = new ConcurrentInteger();

        public FightHandler() {
            EventUtil.createFight = create;
            EventUtil.destoryFight = destory;
        }
        /// <summary>
        /// 战斗结束 房间移除
        /// </summary>
        /// <param name="roomId"></param>
        public void destory(int roomId)
        {
            FightRoom room;
            if (roomMap.TryRemove(roomId, out room))
            {
                //移除角色和房间之间的绑定关系
                int temp = 0;
                  foreach (int item in room.team.Keys)
                   {
                       userRoom.TryRemove(item, out temp);
                   }
                //将房间丢进缓存队列 供下次选择使用
                cache.Push(room);
            }
        }
        /// <summary>
        /// 创建战场
        /// </summary>
        /// <param name="team"></param>
        /// <param name="teamTwo"></param>
        public void create(SelectModel[] team) {
            FightRoom room;
            if (!cache.TryPop(out room))
            {
                room = new FightRoom();
                //添加唯一ID
                room.SetArea(index.GetAndAdd());
            }
            //房间数据初始化
            room.init(team);
            //绑定映射关系
            foreach (SelectModel item in team)
            {
                userRoom.TryAdd(item.userId, room.GetArea());
            }

            
            roomMap.TryAdd(room.GetArea(), room);
        }

        public void ClientClose(NetFrame.UserToken token, string error)
        {
            ///判断玩家是否在某场战斗中
            if (userRoom.ContainsKey(getUserId(token)))
            {
                roomMap[userRoom[getUserId(token)]].ClientClose(token, error);
            }
        }

        public void MessageReceive(NetFrame.UserToken token, NetFrame.auto.SocketModel message)
        {
            
            roomMap[userRoom[getUserId(token)]].MessageReceive(token, message);
        }
    }
}
