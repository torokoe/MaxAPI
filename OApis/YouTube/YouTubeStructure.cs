using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GApis.OApis.YouTube
{
    public class YouTubeStructure
    {
        /// <summary>
        /// AuthenticateOauth
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="userName"></param>
        /// <param name="_ApplicationName"></param>
        /// <returns></returns>
        public static YouTubeService AuthenticateOauth(string clientId, string clientSecret, string userName, string _ApplicationName = "YouTubeService Oauth2")
        {
            try
            {

                if (string.IsNullOrEmpty(clientId))
                    throw new ArgumentNullException("clientId");
                if (string.IsNullOrEmpty(clientSecret))
                    throw new ArgumentNullException("clientSecret");
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");

                string[] scopes = new string[] { };

                var credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = System.IO.Path.Combine(credPath, ".credentials/apiName");

                // Requesting Authentication or loading previously stored authentication for userName
                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                                             , scopes
                                                                                             , userName
                                                                                             , CancellationToken.None
                                                                                             , new FileDataStore(credPath, true)).Result;
                // Returning the SheetsService
                return new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account YouTubeService failed" + ex.Message);
                throw new Exception("CreateServiceFailed", ex);
            }
        }

        /// <summary>
        /// This method requests Authentcation from a user using Oauth2.  
        /// Credentials are stored in System.Environment.SpecialFolder.Personal
        /// Documentation: https://developers.google.com/accounts/docs/OAuth2
        /// </summary>
        /// <param name="clientSecretJson">Path to the client secret json file from Google Developers console.</param>
        /// <param name="userName">Identifying string for the user who is being authentcated.</param>
        /// <returns>DriveService used to make requests against the Drive API</returns>
        public static YouTubeService AuthenticateOauth(string clientSecretJson, string userName, string _ApplicationName = "YouTubeService Oauth2")
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");
                if (string.IsNullOrEmpty(clientSecretJson))
                    throw new ArgumentNullException("clientSecretJson");
                if (!System.IO.File.Exists(clientSecretJson))
                    throw new Exception("clientSecretJson file does not exist.");

                string[] scopes = new string[] { };
                UserCredential credential;
                using (var stream = new FileStream(clientSecretJson, FileMode.Open, FileAccess.Read))
                {
                    string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    credPath = Path.Combine(credPath, ".credentials/", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

                    // Requesting Authentication or loading previously stored authentication for userName
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                                                                             scopes,
                                                                             userName,
                                                                             CancellationToken.None,
                                                                             new FileDataStore(credPath, true)).Result;
                }

                // Create Drive API service.
                return new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account YouTubeService failed" + ex.Message);
                throw new Exception("CreateServiceFailed", ex);
            }
        }

        /// <summary>
        /// UserCredential
        /// <para>return PeopleService</para>
        /// </summary>
        /// <param name="_credential"></param>
        /// <param name="_ApplicationName"></param>
        /// <returns></returns>
        public static YouTubeService AuthenticateOauth(UserCredential _credential, string _ApplicationName = "YouTubeService Oauth2")
        {
            try
            {
                return new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = _credential,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account YouTubeService failed" + ex.Message);
                throw new Exception("CreateServiceFailed", ex);
            }
        }

    }

    public class YouTubeExtend
    {

    }

    public class YouTubeHelpers
    {
        /// <summary>
        /// Using reflection to apply optional parameters to the request.  
        /// 
        /// If the optonal parameters are null then we will just return the request as is.
        /// </summary>
        /// <param name="request">The request. </param>
        /// <param name="optional">The optional parameters. </param>
        /// <returns></returns>
        public static object ApplyOptionalParms(object request, object optional)
        {
            if (optional == null)
                return request;

            System.Reflection.PropertyInfo[] optionalProperties = (optional.GetType()).GetProperties();

            foreach (System.Reflection.PropertyInfo property in optionalProperties)
            {
                // Copy value from optional parms to the request.  They should have the same names and datatypes.
                System.Reflection.PropertyInfo piShared = (request.GetType()).GetProperty(property.Name);
                if (property.GetValue(optional, null) != null) // TODO Test that we do not add values for items that are null
                    if (piShared != null)
                        piShared.SetValue(request, property.GetValue(optional, null), null);
            }

            return request;
        }
    }
}
