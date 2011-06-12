using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Runtime.Serialization.Formatters;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            StartChat();
        }

        private static void StartChat()
        {
            Console.WriteLine("Starting Chat 1.0...");
            
            var serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

            var props = new Dictionary<string, object>();
            props["port"] = 5777;
            props["typeFilterLevel"] = TypeFilterLevel.Full;

            var tcpChannel = new TcpChannel(props, null, serverProvider);

            ChannelServices.RegisterChannel(tcpChannel, false);

            var commonInterface = typeof(ChatImp);
            RemotingConfiguration.RegisterWellKnownServiceType(commonInterface, "MyChat", WellKnownObjectMode.Singleton);
            Console.WriteLine("Service started!");
            Console.WriteLine("Press any key to stop...");
            Console.ReadLine();
        }
    }
}
