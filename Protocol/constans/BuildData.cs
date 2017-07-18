using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.constans
{
    /// <summary>
    /// 建筑属性配置表
    /// </summary>
    public class BuildData
    {
        public static readonly Dictionary<int, BuildDataModel> buildMap = new Dictionary<int, BuildDataModel>();
        /// <summary>
        /// 静态构造 初次访问的时候自动调用
        /// </summary>
        static BuildData()
        {
            // create(1, "主基地", 5000, 0, 50, false, true, false, 0);
            create(1, "高级箭塔", 3000, 200, 50, false, true, true, 30);
            create(2, "中级箭塔", 2000, 150, 30, true, true, false, 0);
            create(3, "初级箭塔", 1000, 100, 20, true, true, false, 0);
        }

        static void create(int id, string name, int hp, int atk, int def, bool initiative, bool infrared, bool reborn, int rebornTime)
        {
            BuildDataModel model = new BuildDataModel(id, name, hp, atk, def, initiative, infrared, reborn, rebornTime);
            buildMap.Add(id, model);
        }
    }
    public partial class BuildDataModel
    {
        public int code;//箭塔编码
        public int hp;//箭塔血量
        public int atk;//箭塔攻击
        public int def;//箭塔防御
        public bool initiative;//是否攻击型建筑
        public bool infrared;//不可否认，这货字面意思是红外线，但是红外代表夜视，所以这里咱用来表示是否反隐吧
        public string name;//还是给个名字 用来区分下吧
        public bool reborn;//是否复活
        public int rebornTime;//复活时间，单位秒

        public BuildDataModel() { }
        public BuildDataModel(int code, string name, int hp, int atk, int def, bool initiative, bool infrared, bool reborn, int rebornTime)
        {
            this.code = code;
            this.hp = hp;
            this.atk = atk;
            this.def = def;
            this.initiative = initiative;
            this.infrared = infrared;
            this.name = name;
            this.reborn = reborn;
            this.rebornTime = rebornTime;
        }

    }
}
