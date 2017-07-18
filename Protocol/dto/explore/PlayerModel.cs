using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.explore
{
    [Serializable]
    public class PlayerModel:AbsActorModel
    {
        public int userID;
        public int characterID;
        public int characterMaxHP;
        public int characterMaxMP;
        public int vehicleID;
        public int vehicleMaxHP;
        public int vehicleMaxMP;
    }
}
