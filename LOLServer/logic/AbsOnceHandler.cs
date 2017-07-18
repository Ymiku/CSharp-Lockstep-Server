using SpaceNetServer.biz;
using SpaceNetServer.dao.model;
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNetServer.logic
{
   public class AbsOnceHandler
    {
      public IUserBiz userBiz = BizFactory.userBiz;

       private byte type;
       private int area;

       public void SetArea(int area) {
           this.area = area;
       }

       public virtual int GetArea() {
           return area;
       }

       public void SetType(byte type)
       {
           this.type = type;
       }

       public new virtual byte GetType()
       {
           return type;
       }

       /// <summary>
       /// 通过连接对象获取用户
       /// </summary>
       /// <param name="token"></param>
       /// <returns></returns>
       public USER getUser(UserToken token)
       {
           return userBiz.get(token);
       }

       /// <summary>
       /// 通过ID获取用户
       /// </summary>
       /// <param name="token"></param>
       /// <returns></returns>
       public USER getUser(int id)
       {
           return userBiz.get(id);
       }

       /// <summary>
       /// 通过连接对象 获取用户ID
       /// </summary>
       /// <param name="token"></param>
       /// <returns></returns>
       public int getUserId(UserToken token){
           USER user = getUser(token);
           if(user==null)return -1;
           return user.id;
       }
       /// <summary>
       /// 通过用户ID获取连接
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public UserToken getToken(int id) {
           return userBiz.getToken(id);
       }


       #region 通过连接对象发送
       public void write(UserToken token,int command) {
           write(token, command, null);
       }
       public void write(UserToken token, int command,object message)
       {
           write(token,GetArea(), command, message);
       }
       public void write(UserToken token,int area, int command, object message)
       {
           write(token,GetType(), GetArea(), command, message);
       }
       public void write(UserToken token,byte type, int area, int command, object message)
       {
           byte[] value = MessageEncoding.encode(CreateSocketModel(type,area,command,message));
           value = LengthEncoding.encode(value);
           token.write(value);
       }
       #endregion

       #region 通过ID发送
       public void write(int id, int command)
       {
           write(id, command, null);
       }
       public void write(int id, int command, object message)
       {
           write(id, GetArea(), command, message);
       }
       public void write(int id, int area, int command, object message)
       {
           write(id, GetType(), area, command, message);
       }
       public void write(int id, byte type, int area, int command, object message)
       {
           UserToken token= getToken(id);
           if(token==null)return;
           write(token, type, area, command, message);
       }

       public void writeToUsers(int[] users, byte type, int area, int command, object message) {
           byte[] value = MessageEncoding.encode(CreateSocketModel(type, area, command, message));
           value = LengthEncoding.encode(value);
           foreach (int item in users)
           {
               UserToken token = userBiz.getToken(item);
               if (token == null) continue;
                   byte[] bs = new byte[value.Length];
                   Array.Copy(value, 0, bs, 0, value.Length);
                   token.write(bs);
               
           }
       }


       #endregion





       public SocketModel CreateSocketModel(byte type, int area, int command, object message)
       {
           return new SocketModel(type, area, command, message);
       }
    }
}
