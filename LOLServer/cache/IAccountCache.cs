﻿using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNetServer.cache
{
   public  interface IAccountCache
    {
       /// <summary>
       /// 账号是否存在
       /// </summary>
       /// <param name="account"></param>
       /// <returns></returns>
       bool hasAccount(string account);
       /// <summary>
       /// 账号密码是否匹配
       /// </summary>
       /// <param name="account"></param>
       /// <param name="password"></param>
       /// <returns></returns>
       bool match(string account,string password);
       /// <summary>
       /// 账号是否在线
       /// </summary>
       /// <param name="account"></param>
       /// <returns></returns>
       bool isOnline(string account);

       /// <summary>
       /// 当前连接对象所对应的ID
       /// </summary>
       /// <param name="token"></param>
       /// <returns></returns>
       int getId(UserToken token);
       /// <summary>
       /// 账号上线
       /// </summary>
       /// <param name="token"></param>
       /// <param name="account"></param>
       void online(UserToken token, string account);
       /// <summary>
       /// 用户下线
       /// </summary>
       /// <param name="token"></param>
       void offline(UserToken token);
       /// <summary>
       /// 添加账号
       /// </summary>
       /// <param name="account"></param>
       /// <param name="password"></param>
       void add(string account, string password);
    }
}
