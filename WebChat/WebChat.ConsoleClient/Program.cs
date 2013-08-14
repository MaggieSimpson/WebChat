using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebChat.Data;
using WebChat.Models;

namespace WebChat.ConsoleClient
{
    public class Program
    {
        public static void Main()
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<WebChatContext>());

            using (var db = new WebChatContext())
            {
                db.Database.Initialize(true);

                //db.Users.Add(new User { Username = "Gosho", Password = "123456" });

                db.Messages.Add(new TextMessage { Content = "hello", Date = DateTime.Now });

                db.SaveChanges();
            }
        }
    }
}
