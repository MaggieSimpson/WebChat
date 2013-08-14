using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebChat.Service.Controllers
{
    public class UserController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        [HttpPost]
        public HttpResponseMessage Register(UserRegister userRegister)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public HttpResponseMessage Login(UserLogin userLogin)
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