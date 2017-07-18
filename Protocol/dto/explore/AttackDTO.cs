using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.explore
{
    [Serializable]
    public class AttackDTO
    {
        public int userId;//攻击者ID
        public int skillID;//技能id 0为普攻
    }
}
