using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public IEnumerable<User> GetOnlineUsers(string sessionKey)
        {
            return unitOfWork.Users.All().Where(x => x.Sessionkey != null && x.Sessionkey != sessionKey);
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
        public async Task<HttpResponseMessage> UploadImage(string sessionkey)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                var dbUser = unitOfWork.Users.All().FirstOrDefault(x => x.Sessionkey == sessionkey);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                    string fileName = file.LocalFileName;
                    var url = DropBoxUploader.UploadProfilePicToDropBox(fileName, file.Headers.ContentDisposition.FileName);
                    dbUser.ProfilePicture = url;
                    unitOfWork.Users.Update(dbUser.UserId, dbUser);
                    break;
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
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