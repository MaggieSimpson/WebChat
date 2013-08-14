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
    public class UserController : BaseControllerTemp
    {
        public UserController(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        [HttpGet]
        [ActionName("online")]
        public IEnumerable<User> Get()
        {
            return unitOfWork.Users.All();
        }


        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage Register(User user)
        {
            var responseMsg = this.PerformOperation(() =>
            {
                unitOfWork.Users.Add(user);
                //var sessionKey = UsersRepository.LoginUser();
                return user;
            });
            return responseMsg;
        }

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage Login(User user)
        {
            var responseMsg = this.PerformOperation(() =>
            {
                string username = string.Empty;
                //var sessionKey = UsersRepository.LoginUser(user.Username, user.Password);
                return user;
            });
            return responseMsg;
        }

        //[HttpGet]
        //[ActionName("logout")]
        //public HttpResponseMessage Logout(string sessionKey)
        //{
        //    var responseMsg = this.PerformOperation(() =>
        //    {
        //        UsersRepository.LogoutUser(sessionKey);
        //    });
        //    return responseMsg;
        //}

        //[HttpGet]
        //[ActionName("online")]
        //public HttpResponseMessage GetOnlineUsers(string sessionKey)
        //{
        //    var responseMsg = this.PerformOperation(() =>
        //    {
        //        UsersRepository.GetOnlineUsers(sessionKey);
        //    });
        //    return responseMsg;
        //}
    }
}