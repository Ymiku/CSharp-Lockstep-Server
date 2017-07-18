using GameProtocol;
using GameProtocol.dto;
using SpaceNetServer.biz;
using SpaceNetServer.dao.model;
using SpaceNetServer.tool;
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNetServer.logic.user
{
    public class UserHandler : AbsOnceHandler, HandlerInterface
    {

        public void ClientClose(NetFrame.UserToken token, string error)
        {
            userBiz.offline(token);
        }

        public void ClientConnect(NetFrame.UserToken token)
        {
            
        }

        public void MessageReceive(NetFrame.UserToken token, NetFrame.auto.SocketModel message)
        {
            switch(message.command){
                case UserProtocol.CREATE_CREQ:
                    create(token, message.GetMessage<string>());
                    break;
                case UserProtocol.INFO_CREQ:
                    info(token);
                    break;
                case UserProtocol.ONLINE_CREQ:
                    online(token);
                    break;
            }
        }

        private void create(UserToken token, string message) {


            ExecutorPool.Instance.execute(
                    delegate()
                    {
                        write(token,UserProtocol.CREATE_SRES,
                        userBiz.Create(token, message));
                    }
                    );
	
        }

        private void info(UserToken token)
        {


            ExecutorPool.Instance.execute(
                    delegate()
                    {
                        write(token, UserProtocol.INFO_SRES,
                        convert(userBiz.getByAccount(token)));
                    }
                    );
	
        }
        private void online(UserToken token)
        {


            ExecutorPool.Instance.execute(
                    delegate()
                    {
                        write(token, UserProtocol.ONLINE_SRES,
                        convert(userBiz.online(token)));
                    }
                    );
	
        }


        private UserDTO convert(USER user)
        {
            if (user==null) return null;
            return new UserDTO(user.name, user.id, user.level, user.win, user.lose, user.ran,user.heroList.ToArray());
        }

        public override byte GetType() {
            return Protocol.TYPE_USER;
        }
    }
}
