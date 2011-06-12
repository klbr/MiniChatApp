using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{


    public abstract class ChatRoom : MarshalByRefObject
    {
        public abstract void Login(string user);
        public abstract void Send(string fromUser, string toUser, string message);
        public abstract void SendToAll(string fromUser, string message);
        public abstract void Logout(string user);
    }
}
