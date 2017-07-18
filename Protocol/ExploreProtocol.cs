using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    public class ExploreProtocol
    {
        public const int ENTER_CREQ = 0;//申请进入探索
        public const int ENTER_SRES = 1;//返回申请结果

        public const int INFORM_CREQ = 2;//加载完毕，申请队伍信息
        public const int INFORM_SREQ = 3;//返回申请结果

        public const int LEAVE_CREQ = 4;//申请离开探索
        public const int LEAVE_SRES = 5;//返回离开结果

        public const int PLAYER_ENTER_BRO = 6;//玩家进入探索
        public const int PLAYER_LEAVE_BRO = 7;//玩家离开

        public const int INPUT_CREQ = 8;
        public const int INPUT_BRO = 9;   
    }
}
