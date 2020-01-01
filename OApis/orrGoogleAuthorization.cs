using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;
using OApis;
using System.Net.Http;
using Google.Apis.Json;

namespace GApis.OAuth
{

    public static class OAuth2Services
    {

        public enum Auth2Services : int
        {
            Google = 0,
            FaceBook = 1,
            Twitter = 2
        }

        public static Auth2Services Auth2Service = Auth2Services.Google;

        public static string GetInstalledAppRedirectUri()
        {
            switch (Auth2Service)
            {
                case Auth2Services.Google:
                    return GoogleAuthConsts.InstalledAppRedirectUri;
                case Auth2Services.FaceBook:
                    return "https://accounts.google.com/o/oauth2/approval";
                default:
                    return GoogleAuthConsts.InstalledAppRedirectUri;
            }
        }

        public static string GetOidcAuthorizationUrl()
        {
            switch (Auth2Service)
            {
                case Auth2Services.Google:
                    return GoogleAuthConsts.OidcAuthorizationUrl;
                case Auth2Services.FaceBook:
                    return "https://graph.facebook.com/oauth/authorize";        //"https://www.facebook.com/dialog/oauth";
                default:
                    return GoogleAuthConsts.OidcAuthorizationUrl;
            }
        }

        public static string GetTokenUrl()
        {
            switch (Auth2Service)
            {
                case Auth2Services.Google:
                    return GoogleAuthConsts.TokenUrl;
                case Auth2Services.FaceBook:
                    return "https://graph.facebook.com/oauth/access_token";  //"https://graph.facebook.com/v2.10/oauth/access_token";
                default:
                    return GoogleAuthConsts.TokenUrl;
            }
        }

        public static string GetscopeSeparator()
        {
            switch (Auth2Service)
            {
                case Auth2Services.Google:
                    return " ";
                case Auth2Services.FaceBook:
                    return ",";
                default:
                    return " ";
            }
        }
    }

    public class EmbeddedBrowserCodeReceiver : ICodeReceiver
    {

        public string RedirectUri
        {
            get { return OAuth2Services.GetInstalledAppRedirectUri(); }
        }

        public  Task<AuthorizationCodeResponseUrl> ReceiveCodeAsync(
          AuthorizationCodeRequestUrl url,
          CancellationToken taskCancellationToken)
        {
            return null;
        }

        Task<AuthorizationCodeResponseUrl> ICodeReceiver.ReceiveCodeAsync(AuthorizationCodeRequestUrl url, CancellationToken taskCancellationToken)
        {
            var tcs = new TaskCompletionSource<AuthorizationCodeResponseUrl>();

            try
            {
                var webAuthDialog = new AuthorizationDialog(url.Build());
                webAuthDialog.ShowDialog();
                if (webAuthDialog.ResponseUrl == null)
                {
                    tcs.SetCanceled();
                }
                else
                {
                    tcs.SetResult(webAuthDialog.ResponseUrl);
                }
                return tcs.Task;
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
                return tcs.Task;
            }
        }
    }


    public class WebAuthorizationBroker_Ex : GoogleWebAuthorizationBroker
    {

        public new static async Task<UserCredential> AuthorizeAsync(
            ClientSecrets clientSecrets,
            IEnumerable<string> scopes,
            string user,
            CancellationToken taskCancellationToken,
            IDataStore dataStore = null, ICodeReceiver codeReceiver = null)
        {
            var initializer = new AuthorizationCodeFlow_Ex.Initializer
            {
                ClientSecrets = clientSecrets,
                AuthorizationServerUrl = OAuth2Services.GetOidcAuthorizationUrl(),
                TokenServerUrl = OAuth2Services.GetTokenUrl(),
                
            };

            return await AuthorizeAsync(initializer, scopes, user,
                taskCancellationToken, dataStore, codeReceiver).ConfigureAwait(false);
        }

        private static async Task<UserCredential> AuthorizeAsyncCore(
            AuthorizationCodeFlow_Ex.Initializer initializer,
            IEnumerable<string> scopes,
            string user,
            CancellationToken taskCancellationToken,
            IDataStore dataStore)
        {

            initializer.Scopes = scopes;
            initializer.DataStore = dataStore ?? new FileDataStore(Folder);
            var flow = new AuthorizationCodeFlow_Ex(initializer);
            return await new AuthorizationCodeInstalledApp(flow,
                new LocalServerCodeReceiver())
                .AuthorizeAsync(user, taskCancellationToken).ConfigureAwait(false);
        }

        public static async Task ReauthorizeAsync(
            UserCredential userCredential,
            CancellationToken taskCancellationToken,
            ICodeReceiver codeReceiver = null)
        {
            codeReceiver = codeReceiver ?? new LocalServerCodeReceiver();
            // Create an authorization code installed app instance and authorize the user.
            UserCredential newUserCredential = await new AuthorizationCodeInstalledApp(userCredential.Flow,
            codeReceiver).AuthorizeAsync
            (userCredential.UserId, taskCancellationToken).ConfigureAwait(false);
            userCredential.Token = newUserCredential.Token;
        }
    }

    /// <summary>
    /// override RedirectUri
    /// </summary>
    public class AuthorizationCodeFlow_Ex : GoogleAuthorizationCodeFlow
    {
        public string RedirectUri;

        public AuthorizationCodeFlow_Ex(Initializer initializer)
        : base(initializer) { }

        public override AuthorizationCodeRequestUrl CreateAuthorizationCodeRequest(string redirectUri)
        {
            //return base.CreateAuthorizationCodeRequest(GoogleWebAuthorizationBroker_Ex.RedirectUri);
            return new AuthorizationCodeRequestUrl(new Uri(AuthorizationServerUrl))
            {
                ClientId = ClientSecrets.ClientId,
                Scope = string.Join(OAuth2Services.GetscopeSeparator(), Scopes),
                RedirectUri = redirectUri
            };
        }
    }

}

