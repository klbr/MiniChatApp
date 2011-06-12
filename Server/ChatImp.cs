using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class ChatImp : ChatRoom, ChatEvents
    {
        private List<User> users;

        public ChatImp()
        {
            this.users = new List<User>();
        }

        private bool IsExist(string user)
        {
            return this.users.Any(u => u.Login == user);
        }

        public override void Login(string user)
        {
            if (this.IsExist(user))
                throw new Exception("Usuario já cadastrado.");

            this.users.Add(new User(user, Guid.NewGuid().ToString()));
            this.OnLogged(user);
            Console.WriteLine("User Added: " + user + ".");
        }

        public override void Send(string fromUser, string toUser, string message)
        {
            var user = SenderValidate(fromUser, toUser);
            user.AddMessage(fromUser + " : " + message);
            this.OnReceived(fromUser, toUser, message);
            Console.WriteLine("User: " + fromUser + " send to: " + toUser + ".");
        }

        private User SenderValidate(string fromUser, string toUser)
        {
            if (fromUser == toUser)
                throw new Exception("Não é possível enviar mensagem para você mesmo!");
            var user = this.users.Where(u => u.Login == toUser).SingleOrDefault();
            if (user == null)
                throw new Exception("Usuario não encontrado.");
            return user;
        }

        public override void SendToAll(string fromUser, string message)
        {
            foreach(var user in this.users.Where(u => u.Login != fromUser).ToArray())
            {
                this.Send(fromUser, user.Login, message);
            }
        }

        public override void Logout(string login)
        {
            var user = this.users.Where(u => u.Login == login).SingleOrDefault();
            if (user == null)
                throw new Exception("Usuario não encontrado.");
            
            this.users.Remove(user);
            this.OnUnlogged(user.Login);
        }

        public event LoggedEventHandler OnLogged;

        public event UnloggedEventHandler OnUnlogged;

        public event ReceivedEventHandler OnReceived;
    }
}
