using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNetServer.logic
{
   public interface HandlerInterface
    {
         void ClientClose(UserToken token, string error);

      //   void ClientConnect(UserToken token);

         void MessageReceive(UserToken token, SocketModel message);
    }
}
