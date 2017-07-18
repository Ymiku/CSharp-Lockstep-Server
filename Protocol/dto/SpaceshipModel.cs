using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public class SpaceshipModel
    {
        public int id;
        public int level;
        public int exp;
        public int[] modInfo;

    }
}
