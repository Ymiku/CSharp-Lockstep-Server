using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
   public class SelectRoomDTO
    {
        public SelectModel[] teamOne;
        public SelectModel[] teamTwo;

        public int GetTeam(int uid) {
            foreach (SelectModel item in teamOne)
            {
                if (item.userId == uid) return 1;
            }
            foreach (SelectModel item in teamTwo)
            {
                if (item.userId == uid) return 2;
            }
            return -1;
        }
    }
}
