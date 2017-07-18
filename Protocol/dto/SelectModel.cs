using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public class SelectModel
    {
        public int userId;//用户ID
        public string name;//用户昵称
        public int hero;//所选英雄
        public bool enter;//是否进入
        public bool ready;//是否已准备
    }
}
