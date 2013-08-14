using System;
using WebChat.Data;
using WebChat.Data.Repositories;
using WebChat.Models;

namespace WebChat.Service.Models
{
    public class UnitOfWork : IDisposable
    {
        public IRepository<User> Users { get; private set; }

        public IRepository<MessageBase> Messages { get; private set; }

        public UnitOfWork()
        {
            {
                var db = new WebChatContext();
                this.Users = new DatabaseRepository<User>(db);
            }

            {
                var db = new WebChatContext();
                this.Messages = new DatabaseRepository<MessageBase>(db);
            }
        }

        public void Dispose()
        {
            this.Users.Dispose();
            this.Messages.Dispose();
        }
    }
}
