using Spring.IO;
using Spring.Social.Dropbox.Api;
using Spring.Social.Dropbox.Connect;
using Spring.Social.OAuth1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Data
{
    public class DropBoxUploader
    {
        // Register your own Dropbox app at https://www.dropbox.com/developers/apps
        // with "Full Dropbox" access level and set your app keys and app secret below
        private const string DropboxAppKey = "1m5nd8kxn72k5w7";
        private const string DropboxAppSecret = "2x8swxs1rnbpbp4";

        public static string UploadProfilePicToDropBox(string filePath, string fileName)
        {
            DropboxServiceProvider dropboxServiceProvider =
                new DropboxServiceProvider(DropboxAppKey, DropboxAppSecret, AccessLevel.AppFolder);

            // Authenticate the application (if not authenticated) and load the OAuth token
            //if (!File.Exists(OAuthTokenFileName))
            //{
            //    AuthorizeAppOAuth(dropboxServiceProvider);
            //}
            OAuthToken oauthAccessToken = LoadOAuthToken();

            // Login in Dropbox
            IDropbox dropbox = dropboxServiceProvider.GetApi(oauthAccessToken.Value, oauthAccessToken.Secret);

            //// Display user name (from his profile)
            //DropboxProfile profile = dropbox.GetUserProfileAsync().Result;
            //Console.WriteLine("Hi " + profile.DisplayName + "!");

            // Create new folder
            string newFolderName =  DateTime.Now.Ticks.ToString();
            Entry createFolderEntry = dropbox.CreateFolderAsync(newFolderName).Result;
            //Console.WriteLine("Created folder: {0}", createFolderEntry.Path);

            // Upload a file
            Entry uploadFileEntry = dropbox.UploadFileAsync(
                new FileResource(filePath),
                "/" + newFolderName + fileName).Result;
            //Console.WriteLine("Uploaded a file: {0}", uploadFileEntry.Path);

            // Share a file
            DropboxLink sharedUrl = dropbox.GetMediaLinkAsync(uploadFileEntry.Path).Result;
            //Process.Start(sharedUrl.Url);
            return sharedUrl.Url.ToString();
        }

        private static OAuthToken LoadOAuthToken()
        {
            using (var db = new WebChatContext())
            {
                var data = db.DropboxAuthentication.First();

                OAuthToken oauthAccessToken = new OAuthToken(data.Token, data.Secret);
                return oauthAccessToken;
            }
        }

        //private static void AuthorizeAppOAuth(DropboxServiceProvider dropboxServiceProvider)
        //{
        //    // Authorization without callback url
        //    //Console.Write("Getting request token...");
        //    OAuthToken oauthToken = dropboxServiceProvider.OAuthOperations.FetchRequestTokenAsync(null, null).Result;
        //    //Console.WriteLine("Done.");

        //    OAuth1Parameters parameters = new OAuth1Parameters();
        //    string authenticateUrl = dropboxServiceProvider.OAuthOperations.BuildAuthorizeUrl(
        //        oauthToken.Value, parameters);
        //    //Console.WriteLine("Redirect the user for authorization to {0}", authenticateUrl);
        //    Process.Start(authenticateUrl);
        //    //Console.Write("Press [Enter] when authorization attempt has succeeded.");
        //    //Console.ReadLine();

        //    //Console.Write("Getting access token...");
        //    AuthorizedRequestToken requestToken = new AuthorizedRequestToken(oauthToken, null);
        //    OAuthToken oauthAccessToken =
        //        dropboxServiceProvider.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;
        //    //Console.WriteLine("Done.");

        //    string[] oauthData = new string[] { oauthAccessToken.Value, oauthAccessToken.Secret };
        //    File.WriteAllLines(OAuthTokenFileName, oauthData);
        //}
    }
}