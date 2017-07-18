using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame.auto
{
   public class MessageEncoding
    {
       /// <summary>
       /// 消息体序列化
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public static byte[] encode(object value) {
           SocketModel model = value as SocketModel;
           ByteArray ba = new ByteArray();
           ba.write(model.type);
           ba.write(model.area);
           ba.write(model.command);
           //判断消息体是否为空  不为空则序列化后写入
           if (model.message != null)
           {
               ba.write(SerializeUtil.encode(model.message));
           }
           byte[] result = ba.getBuff();
           ba.Close();
           return result;
       }
       /// <summary>
       /// 消息体反序列化
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public static object decode(byte[] value)
       {
           ByteArray ba = new ByteArray(value);
           SocketModel model = new SocketModel();
           byte type;
           int area;
           int command;
           //从数据中读取 三层协议  读取数据顺序必须和写入顺序保持一致
           ba.read(out type);
           ba.read(out area);
           ba.read(out command);
           model.type = type;
           model.area = area;
           model.command = command;
           //判断读取完协议后 是否还有数据需要读取 是则说明有消息体 进行消息体读取
           if (ba.Readnable) {
               byte[] message;
               //将剩余数据全部读取出来
               ba.read(out message, ba.Length - ba.Position);
               //反序列化剩余数据为消息体
               model.message = SerializeUtil.decode(message);
           }
           ba.Close();
           return model;
       }
    }
}
