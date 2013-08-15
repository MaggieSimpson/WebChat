using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Service.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public bool MessagesState { get; set; }
    }
}