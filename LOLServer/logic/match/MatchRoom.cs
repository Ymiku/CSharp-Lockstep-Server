using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNetServer.logic.match
{
    /// <summary>
    /// 战斗匹配房间模型
    /// </summary>
   public class MatchRoom
    {
       public int id;//房间唯一ID
       public int teamMax = 3;//每支队伍需要匹配到的人数
       public List<int> team = new List<int>();//队伍一 人员ID
    }
}
