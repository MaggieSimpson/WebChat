using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebChat.Data;
using WebChat.Data.Migrations;
using WebChat.Models;

namespace WebChat.ConsoleClient
{
    public class Program
    {
        public static void Main()
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<WebChatContext>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<WebChatContext, Configuration>());

            using (var db = new WebChatContext())
            {
                db.Database.Initialize(force: true);

                db.Users.Add(new User() { Username = "pesho", Password = "123" });
                db.Messages.Add(new TextMessage() { Content = "Hello, World!" });

                db.SaveChanges();
            }
        }
    }
}
