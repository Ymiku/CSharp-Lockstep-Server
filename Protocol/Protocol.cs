using System;
using System.Collections.Generic;
using System.Text;


namespace GameProtocol
{
   public class Protocol
    {
       public const byte TYPE_LOGIN = 0;//登录模块
       public const byte TYPE_USER = 1;//用户模块
       public const byte TYPE_MATCH = 2;//战斗匹配模块
       public const byte TYPE_SELECT = 3;//战斗匹配模块
       public const byte TYPE_FIGHT = 4;//战斗模块
       public const byte TYPE_EXPLORE = 5;//探索模块
    }
}
