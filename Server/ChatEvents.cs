using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public delegate void LoggedEventHandler(string user);
    public delegate void UnloggedEventHandler(string user);
    public delegate void ReceivedEventHandler(string fromuser, string toUser, string message);
    public interface ChatEvents
    {
        event LoggedEventHandler OnLogged;
        event UnloggedEventHandler OnUnlogged;
        event ReceivedEventHandler OnReceived;
    }
}
