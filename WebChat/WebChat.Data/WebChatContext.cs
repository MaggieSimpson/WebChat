using System;
using System.Data.Entity;
using System.Linq;
using WebChat.Models;

namespace WebChat.Data
{
    public class WebChatContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<MessageBase> Messages { get; set; }
    }
}
