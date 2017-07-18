using SpaceNetServer.tool;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNetServer.logic.select
{
    public class SelectHandler : AbsOnceHandler, HandlerInterface
    {

        /// <summary>
        /// 多线程处理类中  防止数据竞争导致脏数据  使用线程安全字典
        /// 玩家所在匹配房间映射
        /// </summary>
        ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();
        /// <summary>
        /// 房间id与模型映射
        /// </summary>
        ConcurrentDictionary<int, SelectRoom> roomMap = new ConcurrentDictionary<int, SelectRoom>();
        /// <summary>
        /// 回收利用过的房间对象再次利用，减少gc性能开销
        /// </summary>
        ConcurrentStack<SelectRoom> cache = new ConcurrentStack<SelectRoom>();
        /// <summary>
        /// 房间ID自增器
        /// </summary>
        ConcurrentInteger index = new ConcurrentInteger();

        public SelectHandler() {
            EventUtil.createSelect = Create;
            EventUtil.destorySelect = destory;
        }

        public void Create(List<int> team)
        {
            SelectRoom room;
            if (!cache.TryPop(out room)) {
                room = new SelectRoom();
                //添加唯一ID
                room.SetArea(index.GetAndAdd());
            }
            //房间数据初始化
            room.Init(team);
            //绑定映射关系
            foreach (int item in team)
            {
                userRoom.TryAdd(item, room.GetArea());
            }
            
            roomMap.TryAdd(room.GetArea(), room);
        }

        public void destory(int roomId) {
            SelectRoom room;
            if (roomMap.TryRemove(roomId, out room)) {
               //移除角色和房间之间的绑定关系
                int temp=0;
                foreach (int item in room.team.Keys)
	            {
                    userRoom.TryRemove(item, out temp);
	            }
             
                room.list.Clear();
                room.readList.Clear();
                room.team.Clear();
            //将房间丢进缓存队列 供下次选择使用
                cache.Push(room);
            }
        }

        public void ClientClose(NetFrame.UserToken token, string error)
        {
            int userId= getUserId(token);
            //判断当前玩家是否有房间
            if (userRoom.ContainsKey(userId)) {
                int roomId;
                //移除并获取玩家所在房间
                userRoom.TryRemove(userId, out roomId);
                if (roomMap.ContainsKey(roomId)) {
                    //通知
                    roomMap[roomId].ClientClose(token, error);
                }
            }
        }

        public void MessageReceive(NetFrame.UserToken token, NetFrame.auto.SocketModel message)
        {          
            int userId = getUserId(token);
            if (userRoom.ContainsKey(userId)) {
                int roomId = userRoom[userId];
                if (roomMap.ContainsKey(roomId)) {
                    roomMap[roomId].MessageReceive(token, message);
                }
            }
        }
    }
}
