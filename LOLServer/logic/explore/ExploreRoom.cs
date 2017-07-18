using GameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using GameProtocol.dto;
using SpaceNetServer.tool;
using NetFrame;
using SpaceNetServer.dao.model;
using GameProtocol.dto.explore;

namespace SpaceNetServer.logic.explore
{
    /// <summary>
    /// 战斗匹配房间模型
    /// </summary>
    public class ExploreRoom:AbsMulitHandler, HandlerInterface
    {
        public int id;//房间唯一ID
        public int area;
        public int playerMax = 10;//每支队伍需要匹配到的人数
        public ConcurrentDictionary<int, PlayerModel> playerDic = new ConcurrentDictionary<int,PlayerModel>();
        int frameIndex=0;
        public List<byte[]> allInputList = new List<byte[]>();
        public List<byte[]> inputList = new List<byte[]>();

        public void MessageReceive(NetFrame.UserToken token, NetFrame.auto.SocketModel message)
        {
            switch (message.command)
            {
                case ExploreProtocol.INPUT_CREQ:
                    Input( message.GetMessage<byte[]>());
                    break;
            }
        }
        public void Execute()
        {
            if (inputList.Count == 0)
                return;
            brocast(ExploreProtocol.INPUT_BRO,new InputDTO() {
                frameIndex = this.frameIndex,
                inputs = this.inputList
            });
            inputList.Clear();
        }
        public void Creat()
        {
            frameIndex = 0;
            ScheduleUtil.Instance.AddExecute(Execute);
        }
        public void Clear()
        {
            frameIndex = 0;
            ScheduleUtil.Instance.RemoveExecute(Execute);
            allInputList.Clear();
            inputList.Clear();
        }
        private void Input(byte[] b)
        {
            allInputList.Add(b);
            inputList.Add(b);
        }
        public void PlayerEnter(UserToken token)
        {
            Console.WriteLine("有玩家加入");
            base.enter(token);
            int userId = getUserId(token);

            ExploreRoomModel room = new ExploreRoomModel();
            room.playerArray = new PlayerModel[playerDic.Count];
            int i = 0;
            Console.WriteLine("房间内玩家数量为"+playerDic.Count);
            foreach (PlayerModel m in playerDic.Values)
            {
                room.playerArray[i] = m;
                i++;
            }
            write(token, ExploreProtocol.INFORM_SREQ, room);
            PlayerModel model = new PlayerModel();
            model.userID = userId;
            playerDic.TryAdd(userId, model);
            brocast(ExploreProtocol.PLAYER_ENTER_BRO, model, token);
        }
        public void PlayerLeave(UserToken token)
        {
            base.leave(token);
            brocast(ExploreProtocol.PLAYER_LEAVE_BRO,getUserId(token));
        }
      
        public void ClientClose(UserToken token, string error)
        {

        }
        public override byte GetType()
        {
            return Protocol.TYPE_EXPLORE;
        }
    }
}
