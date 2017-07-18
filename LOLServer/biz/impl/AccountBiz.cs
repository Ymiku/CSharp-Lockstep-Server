using SpaceNetServer.cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNetServer.biz.impl
{
   public class AccountBiz:IAccountBiz
    {
       IAccountCache accountCache = CacheFactory.accountCache;

        public int create(NetFrame.UserToken token, string account, string password)
        {
            if (accountCache.hasAccount(account)) return 1;
            accountCache.add(account, password);
            return 0;
        }

        public int login(NetFrame.UserToken token, string account, string password)
        {
            //账号密码为空 输入不合法
            if (account == null || password == null) return -4;
            //判断账号是否存在  不存在则无法登陆
            if (!accountCache.hasAccount(account)) return -1;
            //判断此账号当前是否在线
            if (accountCache.isOnline(account)) return -2;
            //判断账号密码是否匹配
            if (!accountCache.match(account, password)) return -3;
            //验证都通过 说明可以登录  调用上线并返回成功
            accountCache.online(token, account);
            return 0;
        }

        public void close(NetFrame.UserToken token)
        {
            accountCache.offline(token);
        }

        public int get(NetFrame.UserToken token)
        {
            return accountCache.getId(token);
        }
    }
}
