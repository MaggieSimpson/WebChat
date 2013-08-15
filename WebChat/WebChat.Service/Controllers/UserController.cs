using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebChat.Models;
using WebChat.Service.Models;

namespace WebChat.Service.Controllers
{
    public class UserController : BaseController
    {
        private const string SessionKeyChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int SessionKeyLen = 50;

        public UserController(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        [HttpGet]
        [ActionName("all")]
        public IEnumerable<User> GetAll()
        {
            return unitOfWork.Users.All();
        }

        [HttpGet]
        [ActionName("online")]
        public IEnumerable<User> GetOnline()
        {
            return unitOfWork.Users.All().Where(x => x.Sessionkey != null);
        }

        [HttpPost]
        [ActionName("register")]
        public User Register(User user)
        {
            if (!ModelState.IsValid)
            {
                throw new ArgumentException("Invalid credentials");
            }

            unitOfWork.Users.Add(user);
            return user;
        }

        [HttpPost]
        [ActionName("login")]
        public User Login(User user)
        {
            var dbUser = unitOfWork.Users.All().FirstOrDefault(
                x => x.Username == user.Username && x.Password == user.Password);

            if (dbUser == null)
            {
                throw new ArgumentException("Invalid credentials");
            }

            dbUser.Sessionkey = GenerateSessionKey(user.UserId);
            unitOfWork.Users.Update(dbUser.UserId, dbUser);

            return dbUser;
        }

        [HttpPost]
        [ActionName("logout")]
        public HttpResponseMessage Logout(User user)
        {
            var dbUser = unitOfWork.Users.All().FirstOrDefault(x => x.Sessionkey == user.Sessionkey);

            if (dbUser == null)
            {
                throw new ArgumentException("User is not logged in");
            }

            dbUser.Sessionkey = null;
            unitOfWork.Users.Update(dbUser.UserId, dbUser);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private static string GenerateSessionKey(int userId)
        {
            var keyChars = new StringBuilder();

            keyChars.Append(userId.ToString());

            while (keyChars.Length < SessionKeyLen)
            {
                int randomCharNum;
                lock (random)
                {
                    randomCharNum = random.Next(SessionKeyChars.Length);
                }
                var randomKeyChar = SessionKeyChars[randomCharNum];
                keyChars.Append(randomKeyChar);
            }

            var sessionKey = keyChars.ToString();
            return sessionKey;
        }
    }
}