using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.constans
{
    /// <summary>
    /// 英雄属性配置表
    /// </summary>
   public class HeroData
    {

       public static readonly Dictionary<int, HeroDataModel> heroMap = new Dictionary<int, HeroDataModel>();
       /// <summary>
       /// 静态构造 初次访问的时候自动调用
       /// </summary>
       static HeroData() {
           create(1, "阿狸", 100, 20, 500, 300, 5, 2, 30, 10, 1, 0.5f, 200,200, 1, 2, 3, 4);
           create(2, "阿木木", 100, 20, 500, 300, 5, 2, 30, 10, 1, 0.5f, 200, 200, 1, 2, 3, 4);
           create(3, "埃希", 100, 20, 500, 300, 5, 2, 30, 10, 1, 0.5f, 200, 200, 6, 2, 3, 4);
           create(4, "盲僧", 100, 20, 500, 300, 5, 2, 30, 10, 1, 0.5f, 200, 200, 3, 2, 3, 4);
       }
       /// <summary>
       /// 创建模型并添加进字典
       /// </summary>
       /// <param name="code"></param>
       /// <param name="name"></param>
       /// <param name="atkBase"></param>
       /// <param name="defBase"></param>
       /// <param name="hpBase"></param>
       /// <param name="mpBase"></param>
       /// <param name="atkArr"></param>
       /// <param name="defArr"></param>
       /// <param name="hpArr"></param>
       /// <param name="mpArr"></param>
       /// <param name="speed"></param>
       /// <param name="aSpeed"></param>
       /// <param name="range"></param>
       /// <param name="eyeRange"></param>
       /// <param name="skills"></param>
       private static void create(int code,
           string name,
           int atkBase,
          int  defBase,
           int hpBase,
          int  mpBase,
          int  atkArr,
          int  defArr,
          int  hpArr,
           int mpArr,
           float speed,
           float aSpeed,
           float range,
           float eyeRange,
           params int[] skills) {
               HeroDataModel model = new HeroDataModel();
               model.code = code;
               model.name = name;
               model.atkBase = atkBase;
               model.defBase = defBase;
               model.hpBase = hpBase;
               model.mpBase = mpBase;
               model.atkArr = atkArr;
               model.defArr = defArr;
               model.hpArr = hpArr;
               model.mpArr = mpArr;
               model.speed = speed;
               model.aSpeed = aSpeed;
               model.range = range;
               model.eyeRange = eyeRange;
               model.skills = skills;
               heroMap.Add(code, model);
       }
    }

       public partial class HeroDataModel
       {
           public int code;//策划定义的唯一编号
           public string name;//英雄名称
           public int atkBase;//初始(基础)攻击力
           public int defBase;//初始防御
           public int hpBase;//初始血量
           public int mpBase;//初始蓝
           public int atkArr;//攻击成长、
           public int defArr;//防御成长
           public int hpArr;//血量成长
           public int mpArr;//蓝成长
           public float speed;//移动速度
           public float aSpeed;//攻击速度
           public float range;//攻击距离
           public float eyeRange;//视野范围
           public int[] skills;//拥有技能
       }
    
}
