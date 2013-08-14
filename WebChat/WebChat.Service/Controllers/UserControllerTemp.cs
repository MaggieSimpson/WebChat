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
    public class UserControllerTemp : BaseControllerTemp
    {
        public UserControllerTemp(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IEnumerable<User> Get()
        {
            return unitOfWork.Users.All();
        }

        [HttpPost]
        public HttpResponseMessage Register(User user)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public HttpResponseMessage Login(User user)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public HttpResponseMessage Logout(string sessionKey)
        {
            throw new NotImplementedException();
        }
    }
}