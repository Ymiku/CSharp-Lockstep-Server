using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNetServer.tool
{
   public class TimeTaskModel
    {
       //任务逻辑
       private TimeEvent execut;
       //任务执行的时间
       public long time;
       public int id;//任务ID

       public TimeTaskModel(int id, TimeEvent execut, long time) {
           this.id = id;
           this.execut = execut;
           this.time = time;
       }
       public void run() {
           execut();
       }
    }
}
