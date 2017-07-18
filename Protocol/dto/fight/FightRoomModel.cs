using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.fight
{
    [Serializable]
    public class FightRoomModel
    {
        public AbsFightModel[] teamOne;
        public AbsFightModel[] teamTwo;
    }
}
