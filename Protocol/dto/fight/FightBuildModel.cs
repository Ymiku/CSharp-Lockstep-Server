using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.fight
{
    [Serializable]
   public class FightBuildModel:AbsFightModel
    {
       public bool born;//是否重生
       public int bornTime;//重生时间
       public bool initiative;//是否攻击
       public bool infrared;//红外线 （是否反隐） 
       public FightBuildModel() { }
       public FightBuildModel(int id, int code, int hp, int hpMax, int atk, int def, bool reborn, int rebornTime, bool initiative, bool infrared, string name)
       {
           this.id = id;
           this.code = code;
           this.hp = hp; this.maxHp = hpMax;
           this.atk = atk;
           this.def = def;
           this.born = reborn;
           this.bornTime = rebornTime;
           this.initiative = initiative;
           this.infrared = infrared;
           this.name = name;
       }
    }
}
