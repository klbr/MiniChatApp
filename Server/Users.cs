using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class User
    {
        public string Login
        {
            get;
            private set;
        }
        public string Auth
        {
            get;
            private set;
        }
        private Queue<string> messages;
        
        public User(string login, string auth)
        {
            this.Auth = auth;
            this.Login = login;
            this.messages = new Queue<string>();
        }

        public void AddMessage(string message)
        {
            lock (this)
            {
                this.messages.Enqueue(message);
            }
        }

        public string[] GetMessages()
        {
            lock (this)
            {
                var listMessages = new List<string>();
                while (this.messages.Count > 0)
                    listMessages.Add(this.messages.Dequeue());
                return listMessages.ToArray();
            }
        }
    }
}
