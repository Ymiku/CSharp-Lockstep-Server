using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public class SolarDTO
    {
        public int seed;
        public List<DefencerModel> deftencerList = new List<DefencerModel>();
    }
}
