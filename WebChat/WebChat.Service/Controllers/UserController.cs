using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebChat.Data;
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
        public IEnumerable<UserModel> GetOnlineUsers(string sessionKey)
        {
            HashSet<UserModel> onlineUsers = new HashSet<UserModel>();
            var users = unitOfWork.Users.All().Where(x => x.Sessionkey != null && x.Sessionkey != sessionKey).ToList();

            var messages = unitOfWork.Messages.All().GroupBy(x => x.Reciever.Username).ToDictionary(x => x.Key, x => x.ToList());
            var dbUser = unitOfWork.Users.All().First(x => x.Sessionkey == sessionKey);

            foreach (var user in users)
            {
                bool hasUnreadMessages = messages[dbUser.Username].Any(x => x.State == false && x.Sender.Username == user.Username);

                UserModel userModel = new UserModel()
                {
                    MessagesState = hasUnreadMessages,
                    ProfilePicture = user.ProfilePicture,
                    Username = user.Username
                };

                onlineUsers.Add(userModel);
            }

            return onlineUsers;
        }

        [HttpGet]
        [ActionName("profilePicture")]
        public string GetProfilePictureUrl(string sessionKey)
        {
            return unitOfWork.Users.All().First(x => x.Sessionkey != null && x.Sessionkey == sessionKey).ProfilePicture;
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
        [ActionName("uploadImage")]
        public HttpResponseMessage UploadImage(string sessionkey)
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                var files = new List<string>();

                var dbUser = unitOfWork.Users.All().First(x => x.Sessionkey == sessionkey);

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var fileName = DateTime.Now.Ticks + dbUser.Username + postedFile.FileName;
                    var path = HttpContext.Current.Server.MapPath("~/App_Data/") + fileName;
                    postedFile.SaveAs(path);

                    var url = DropBoxUploader.UploadProfilePicToDropBox(path, fileName);
                    File.Delete(path);

                    dbUser.ProfilePicture = url;
                    unitOfWork.Users.Update(dbUser.UserId, dbUser);

                    files.Add(fileName);

                    break;
                }

                result = Request.CreateResponse(HttpStatusCode.Created, files);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
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