using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.explore
{
    [Serializable]
    public class MoveDTO
    {
        public int userId;
        public float x;
        public float y;
        public float z;
        public float rotateX;
        public float rotateY;
        public float rotateZ;
    }
}
