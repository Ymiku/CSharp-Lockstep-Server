using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.fight
{
    [Serializable]
    public class FightPlayerModel:AbsFightModel
    {
        public int level;//等级
        public int exp;//经验
        public int free;//剩余潜能点
        public int money;//玩家经济
        public int[] equs;//玩家装备
        public FightSkill[] skills;//玩家拥有技能
        public int mp;//当前能量
        public int maxMp;//最大能量

        public int SkillLevel(int code) {
            foreach (FightSkill item in skills)
            {
                if (item.code == code) {
                    return item.level;
                }
            }
            return -1;
        }
    }
}
