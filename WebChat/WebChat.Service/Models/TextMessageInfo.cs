using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Service.Models
{
    public class TextMessageInfo
    {
        public string SessionKey { get; set; }
        public string Reciever { get; set; }
        public string Content { get; set; }
    }
}