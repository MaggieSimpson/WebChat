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
    public class MessageController : BaseControllerTemp
    {
        public MessageController(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        [HttpGet]
        public IEnumerable<MessageBase> Get(string sessionKey)
        {
            if (sessionKey == null)
            {
                throw new ArgumentException("Session key must not be null!");
            }

            return unitOfWork.Messages.All().Where(x => x.Sender.Sessionkey == sessionKey && x.State);
        }

        [HttpGet]
        [ActionName("byUsername")]
        public HttpResponseMessage ByUsername(string sessionKey, string username)
        {
            if (sessionKey == null)
            {
                throw new ArgumentException("Session key must not be null!");
            }

            var responseMsg = this.PerformOperation(() =>
            {
                var messages = unitOfWork.Messages.All().Where(
                    x => x.Reciever.Username == username && x.Sender.Sessionkey == sessionKey).ToList();

                foreach (var message in messages)
                {
                    message.State = true;
                    unitOfWork.Messages.Update(message.MessageId, message);
                }

                return messages;
            });

            return responseMsg;
        }
    }
}
