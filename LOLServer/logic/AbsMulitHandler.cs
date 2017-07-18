using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNetServer.logic
{
   public class AbsMulitHandler:AbsOnceHandler
    {
       public List<UserToken> list = new List<UserToken>();
        private object _locker = new object();
       /// <summary>
       /// 用户进入当前子模块
       /// </summary>
       /// <param name="token"></param>
       /// <returns></returns>
       public bool enter(UserToken token) {
            lock (_locker)
            {
                if (list.Contains(token))
                {
                    return false;
                }
                list.Add(token);
                return true;
            }
       }
       /// <summary>
       /// 用户是否在此子模块
       /// </summary>
       /// <param name="token"></param>
       /// <returns></returns>
       public bool isEntered(UserToken token) {
           return list.Contains(token);
       }
       /// <summary>
       /// 用户离开当前子模块
       /// </summary>
       /// <param name="token"></param>
       /// <returns></returns>
       public bool leave(UserToken token) {
            lock (_locker)
            {
                if (list.Contains(token))
                {
                    list.Remove(token);
                    return true;
                }
                return false;
            }
       }
       #region 消息群发API

       public void brocast(int command, object message,UserToken exToken=null) {
           brocast(GetArea(), command, message, exToken);
       }
       public void brocast(int area, int command, object message, UserToken exToken = null)
       {
           brocast(GetType(), area, command, message, exToken);
       }
       public void brocast(byte type, int area, int command, object message, UserToken exToken = null)
       {
           byte[] value = MessageEncoding.encode(CreateSocketModel(type, area, command, message));
           value = LengthEncoding.encode(value);
            lock (_locker)
            {
                foreach (UserToken item in list)
                {
                    if (item != exToken)
                    {
                        byte[] bs = new byte[value.Length];
                        Array.Copy(value, 0, bs, 0, value.Length);
                        item.write(bs);
                    }
                }
            }
       }
       #endregion
    }
}
