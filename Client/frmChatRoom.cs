using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Server;
using System.IO;

namespace Client
{
    public partial class frmChatRoom : Form
    {
        private Client client;
        public frmChatRoom()
        {
            InitializeComponent();
            this.Initialize();
        }

        private void Initialize()
        {
            ConnectServer();
            this.LogonClient();
        }

        private void LogonClient()
        {
            string clientName = string.Format("{0}@{0}", Environment.UserName, Environment.MachineName);
            this.client = Client.Initialize(clientName, (ChatRoom)Activator.GetObject(typeof(ChatImp), "tcp://localhost:5777/MyChat"));
            this.client.SetUpEvents(new LoggedEventHandler(this.Logged), new UnloggedEventHandler(this.Unlogged), new ReceivedEventHandler(this.Received));
            this.client.Login();
        }

        private static void ConnectServer()
        {
            var clientProvider = new BinaryClientFormatterSinkProvider();
            var serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            var props = new Dictionary<string, object>();
            props["port"] = 0;
            props["name"] = Guid.NewGuid().ToString();
            props["typeFilterLevel"] = TypeFilterLevel.Full;
            var tcpChannel = new TcpChannel(props, clientProvider, serverProvider);
            ChannelServices.RegisterChannel(tcpChannel, false);
        }

        public void Received(string fromUser, string toUser, string message)
        {
            if (this.client.Name == toUser)
                this.lbxMessages.Items.Add(fromUser + " said : " + message);
        }
        public void Logged(string user)
        {
            if (this.client.Name != user)
                this.lbxMessages.Items.Add(user + " has login...");
        }
        public void Unlogged(string user)
        {
            if (this.client.Name != user)
                this.lbxMessages.Items.Add(user + " has logout.");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.client.SendToAll(txtMessage.Text);
            this.lbxMessages.Items.Add("You said : " + txtMessage.Text);
        }

        public class Client
        {
            private Client(string name, ChatRoom chat)
            {
                this.Name = name;
                this.chat = chat;
            }

            public static Client Initialize(string name, ChatRoom chat)
            {
                return new Client(name, chat);
            }
            private ChatRoom chat;

            public string Name
            { get; private set; }

            public void Login()
            {
                this.chat.Login(this.Name);
            }

            public void SetUpEvents(LoggedEventHandler logged, UnloggedEventHandler unlogged, ReceivedEventHandler received)
            {
                if (!(this.chat is ChatImp))
                    throw new InvalidCastException("Chat must be derived of ChatImp");
                var chatImp = this.chat as ChatImp;
                chatImp.OnLogged += logged;
                chatImp.OnUnlogged += unlogged;
                chatImp.OnReceived += received;
            }

            public void SendToAll(string message)
            {
                this.chat.SendToAll(this.Name, message);
            }
        }
    }
}
