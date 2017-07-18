using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.fight
{
    [Serializable]
   public class FightSkill
    {
       public int code;//策划编码
       public int level;//等级
       public int nextLevel;//学习需要角色等级
       public int time;//冷却时间--ms
       public string name;//技能名称
       public float range;//释放距离
       public string info;//技能描述
       public SkillTarget target;//技能伤害目标类型
       public SkillType type;//技能释放类型

       public FightSkill() { }
       public FightSkill(
         int code,
         int level,
         int nextLevel,
         int time,
         string name,
         float range,
         string info,
         SkillTarget  target,
         SkillType  type
           ) {
               this.code = code;
               this.level = level;
               this.nextLevel = nextLevel;
               this.time = time;
               this.name = name;
               this.range = range;
               this.info = info;
               this.target = target;
               this.type = type;
       }
    }


    /// <summary>
    /// 能够造成效果的单位类型
    /// </summary>
   public enum SkillTarget { 
       SELF,//自身释放
       F_H,//友方英雄
       F_N_B,//友方非建筑单位
       F_ALL,//友方全体
       E_H,//敌方英雄
       E_N_B,//敌方非建筑
       E_S_N,//敌方和中立单位
       N_F_ALL//非友方单位
   }

    /// <summary>
    /// 技能释放方式
    /// </summary>
   public enum SkillType { 
        SELF,//以自身为中心进行释放
       TARGET,//以目标为中心 进行释放
       POSITION,//以鼠标点击位置为目标 释放技能
       PASSIVE//被动技能
   }
    /// <summary>
    /// 战斗模型类型
    /// </summary>
   public enum ModelType { 
        BUILD, //建筑
       HUMAN//生物
   }
}
