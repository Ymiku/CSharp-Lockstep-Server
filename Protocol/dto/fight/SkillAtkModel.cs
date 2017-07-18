using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.fight
{
    [Serializable]
   public class SkillAtkModel
    {
       public int userId;
       public int type;//0表示目标攻击 1表示指定点
       public int skill;
       public float[] position;
       public int target;
    }
}
