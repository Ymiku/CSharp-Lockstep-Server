using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    /// <summary>
    /// 登录协议
    /// </summary>
   public class LoginProtocol
    {
       public const int LOGIN_CREQ = 0;//客户端申请登录
       public const int LOGIN_SRES = 1;//服务器反馈给客户端 登录结果

       public const int REG_CREQ = 2;//客户端申请注册
       public const int REG_SRES = 3;//服务器反馈给客户端 注册结果
    }
}
