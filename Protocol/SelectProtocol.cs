using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
   public class SelectProtocol
    {
       public const int ENTER_CREQ = 0;
       public const int ENTER_SRES = 1;
       public const int ENTER_EXBRO = 2;
       public const int SELECT_CREQ = 3;
       public const int SELECT_SRES = 4;
       public const int SELECT_BRO = 5;
       public const int TALK_CREQ = 6;
       public const int TALK_BRO = 7;
       public const int READY_CREQ = 8;
       public const int READY_BRO = 9;
       public const int DESTORY_BRO = 10;
       public const int FIGHT_BRO = 11;
    }
}
