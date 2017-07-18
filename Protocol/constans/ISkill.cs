using GameProtocol.dto.fight;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.constans
{
   public interface ISkill
    {
       void damage(int level, ref AbsFightModel atk, ref AbsFightModel target, ref List<int[]> damages);
    }
}
