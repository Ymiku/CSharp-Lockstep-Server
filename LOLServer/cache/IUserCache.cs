using SpaceNetServer.dao.model;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNetServer.cache
{
   public interface IUserCache
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool create(UserToken token,string name,int accountId);
        //是否拥有角色
        bool has(UserToken token);
        /// <summary>
        /// 判断对应帐号ID是否拥有角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool hasByAccountId(int id);
        //根据连接获取用户信息
        USER get(UserToken token);
        //根据用户ID获取用户信息
        USER get(int id);
        //用户登录
        USER online(NetFrame.UserToken token, int id);
        //用户下线
         void offline(NetFrame.UserToken token);
        //通过ID获取连接
        UserToken getToken(int id);
        /// <summary>
        /// 通过帐号ID获取角色
        /// </summary>
        /// <param name="accId"></param>
        /// <returns></returns>
        USER getByAccountId(int accId);
        /// <summary>
        /// 角色是否已经在线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool isOnline(int id);
    }
}
