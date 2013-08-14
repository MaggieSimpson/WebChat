using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using WebChat.Data;
using WebChat.Models;
using WebChat.Service.Models;

namespace WebChat.Service.Controllers
{
    public class MessageController : BaseController
    {
        public MessageController(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        [HttpGet]
        public IEnumerable<MessageBase> Get(string sessionKey)
        {
            if (string.IsNullOrEmpty(sessionKey))
            {
                throw new ArgumentNullException("sessionKey");
            }

            return unitOfWork.Messages.All().Where(x => x.Sender.Sessionkey == sessionKey && x.State);
        }

        [HttpGet]
        [ActionName("byUsername")]
        public IEnumerable<MessageBase> ByUsername(string sessionKey, string username)
        {
            if (string.IsNullOrEmpty(sessionKey))
            {
                throw new ArgumentNullException("sessionKey");
            }

            var dbUser = unitOfWork.Users.All().FirstOrDefault(x => x.Sessionkey == sessionKey);

            var messages = unitOfWork.Messages.All().ToList().Where(
                x => (x.Reciever.Username == username && x.Sender.Sessionkey == sessionKey) ||
                    (x.Reciever.Username == dbUser.Username && x.Sender.Username == username)).ToList();

            foreach (var message in messages)
            {
                if (message.Reciever.Username == dbUser.Username)
                {
                    message.State = true;
                    unitOfWork.Messages.Update(message.MessageId, message);
                }
            }

            return messages;
        }

        [HttpPost]
        [ActionName("send")]
        public TextMessage Send([FromBody]TextMessageInfo info)
        {
            if (!ModelState.IsValid)
            {
                throw new ArgumentException("Invalid credentials");
            }

            var sender = unitOfWork.Users.All().FirstOrDefault(x => x.Sessionkey == info.SessionKey);

            var reciever = unitOfWork.Users.All().FirstOrDefault(x => x.Username == info.Reciever);

            TextMessage message = new TextMessage()
            {
                Content = info.Content,
                Date = DateTime.Now,
                Reciever = reciever,
                Sender = sender,
                State = false
            };

            unitOfWork.Messages.Add(message);

            string channel = "stamat-channel";

            PubnubContext.Publish(channel, JsonConvert.SerializeObject(message));

            return message;
        }
    }
}
