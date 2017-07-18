using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.fight
{
    [Serializable]
   public class MoveDTO
    {
        public int userId;
        public float x;
        public float y;
        public float z;
    }
}
