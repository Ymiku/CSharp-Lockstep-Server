using SpaceNetServer.cache;
using SpaceNetServer.dao.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNetServer.biz.impl
{
    /// <summary>
    /// 用户事物处理
    /// </summary>
   public class UserBiz:IUserBiz
    {
       IAccountBiz accBiz = BizFactory.accountBiz;
       IUserCache userCache = CacheFactory.userCache;
        public bool Create(NetFrame.UserToken token, string name)
        {
            //帐号是否登陆 获取帐号ID
            int accountId= accBiz.get(token);
            if (accountId == -1) return false;
            //判断当前帐号是否已经拥有角色
            if (userCache.hasByAccountId(accountId)) return false;
            userCache.create(token, name,accountId);
            return true;
        }

        public dao.model.USER getByAccount(NetFrame.UserToken token)
        {
            //帐号是否登陆 获取帐号ID
            int accountId = accBiz.get(token);
            if (accountId == -1) return null;
            return userCache.getByAccountId(accountId);
        }

        public dao.model.USER get(int id)
        {
            return userCache.get(id);
        }

        public USER online(NetFrame.UserToken token)
        { int accountId= accBiz.get(token);
            if (accountId == -1) return null;
            USER user = userCache.getByAccountId(accountId);
            if (userCache.isOnline(user.id)) return null;
            userCache.online(token, user.id);
            return user;
        }

        public void offline(NetFrame.UserToken token)
        {
            userCache.offline(token);
        }

        public NetFrame.UserToken getToken(int id)
        {
          return  userCache.getToken(id);
        }


        public USER get(NetFrame.UserToken token)
        {
            return userCache.get(token);
        }
    }
}
