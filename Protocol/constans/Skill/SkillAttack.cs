using GameProtocol.dto.fight;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.constans.Skill
{
   public class SkillAttack:ISkill
    {
        public void damage(int level,ref AbsFightModel atk,ref AbsFightModel target,ref List<int[]> damages)
        {
            int value = atk.atk - target.def;
            value = value > 0 ? value : 1;
            target.hp = target.hp - value <= 0 ? 0 : target.hp - value;
            damages.Add(new int[]{target.id,value,target.hp==0?0:1});
        }
    }
}
