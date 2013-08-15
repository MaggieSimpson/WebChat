using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

            var messages = unitOfWork.Messages.All().Where(
                x => x.Reciever.Username == username && x.Sender.Sessionkey == sessionKey).ToList();

            foreach (var message in messages)
            {
                message.State = true;
                unitOfWork.Messages.Update(message.MessageId, message);
            }

            return messages;
        }
    }
}
