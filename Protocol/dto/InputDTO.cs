using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public class InputDTO
    {
        public int frameIndex;
        public List<byte[]> inputs;
    }
}
