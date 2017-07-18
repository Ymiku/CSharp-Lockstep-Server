using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.fight
{
    [Serializable]
   public class DamageDTO
    {
        public int userId;
        public int skill;
        public int[][] target;
    }
}
