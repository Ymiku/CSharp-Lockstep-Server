using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.fight
{
  [Serializable]
   public class AbsFightModel
    {
       public int id;//战斗区域中 唯一识别码
       public ModelType type;//标识当前生命体是属于什么类别
       public int code;//模型唯一识别码 但是战斗中会有多个相同兵种出现 所以这里只用于标识形象急获取对应的数据
       public int hp;//当前血量
       public int maxHp;//最大血量
       public int atk;//攻击
       public int def;//防御
       public string name;//名称
       public float speed;//移动速度
       public float aSpeed;//攻击速度
       public float aRange;//攻击范围
       public float eyeRange;//视野范围
       public int team;//单位所在的队伍
    }
}
