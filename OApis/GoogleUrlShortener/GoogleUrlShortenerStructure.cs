using Google.Apis.Auth.OAuth2;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using Google.Apis.Urlshortener.v1.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GApis.OApis.GoogleUrlShortener
{
    public class GoogleUrlShortenerStructure
    {

        public class UrlGetOptionalParms
        {
            /// Additional information to return.
            public UrlResource.GetRequest.ProjectionEnum Projection { get; set; }
        }

        public class UrlListOptionalParms
        {
            /// Additional information to return.
            public UrlResource.ListRequest.ProjectionEnum Projection { get; set; }
            /// Token for requesting successive pages of results.
            public string StartToken { get; set; }
        }

        /// <summary>
        /// Authenticate to Google Using Oauth2
        /// <para>Documentation: https://developers.google.com/accounts/docs/OAuth2 </para>
        /// <para>Credentials are stored in System.Environment.SpecialFolder.Personal</para>
        /// </summary>
        /// <param name="clientId">From Google Developer console https://console.developers.google.com</param>
        /// <param name="clientSecret">From Google Developer console https://console.developers.google.com</param>
        /// <param name="userName">Identifying string for the user who is being authentcated.</param>
        /// <param name="_ApplicationName">DriveService ApplicationName(+UserAgent)</param>
        /// <returns>SheetsService used to make requests against the Sheets API</returns>
        public static UrlshortenerService AuthenticateOauth(string clientId, string clientSecret, string userName, string _ApplicationName = "GoogleUrlShortener Oauth2")
        {
            try
            {
                if (string.IsNullOrEmpty(clientId))
                    throw new ArgumentNullException("clientId");
                if (string.IsNullOrEmpty(clientSecret))
                    throw new ArgumentNullException("clientSecret");
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");

                string[] scopes = new string[] { UrlshortenerService.Scope.Urlshortener };

                var credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = System.IO.Path.Combine(credPath, ".credentials/apiName");

                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                                             , scopes
                                                                                             , userName
                                                                                             , CancellationToken.None
                                                                                             , new FileDataStore(credPath, true)).Result;
                return new UrlshortenerService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account GoogleUrlShortener failed" + ex.Message);
                throw new Exception("CreateServiceFailed", ex);
            }
        }

        /// <summary>
        /// UserCredential
        /// <para>return DriveService</para>
        /// </summary>
        /// <param name="_credential"></param>
        /// <param name="_ApplicationName"></param>
        /// <returns></returns>
        public static UrlshortenerService AuthenticateOauth(UserCredential _credential, string _ApplicationName = "GoogleUrlShortener Oauth2")
        {
            try
            {
                return new UrlshortenerService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = _credential,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account GoogleUrlShortener failed" + ex.Message);
                throw new Exception("CreateServiceFailed", ex);
            }
        }

        /// <summary>
        /// ApiKey
        /// <para>return DriveService</para>
        /// </summary>
        /// <param name="_credential"></param>
        /// <param name="_ApplicationName"></param>
        /// <returns></returns>
        public static UrlshortenerService AuthenticateOauth(string _ApiKey, string _ApplicationName = "GoogleUrlShortener Oauth2")
        {
            try
            {
                return new UrlshortenerService(new BaseClientService.Initializer()
                {
                    ApiKey = _ApiKey,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account GoogleUrlShortener failed" + ex.Message);
                throw new Exception("CreateServiceFailed", ex);
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/url-shortener/v1/url/get
        /// <para>Expands a short URL or gets creation time and analytics.</para>
        /// </summary>
        /// <param name="service">Authenticated urlshortener service.</param>  
        /// <param name="shortUrl">The short URL, including the protocol.</param>
        /// <param name="optional">Optional paramaters.</param>
        /// <returns>UrlResponse</returns>
        public static Url Get(UrlshortenerService service, string shortUrl, UrlGetOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (shortUrl == null)
                    throw new ArgumentNullException(shortUrl);

                // Building the initial request.
                var request = service.Url.Get(shortUrl);

                // Applying optional parameters to the request.                
                request = (UrlResource.GetRequest)GoogleUrlShortenerHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Url.Get failed.", ex);
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/url-shortener/v1/url/insert
        /// <para>Creates a new short URL.</para>
        /// </summary>
        /// <param name="service">Authenticated urlshortener service.</param>  
        /// <param name="body">A valid urlshortener v1 body.</param>
        /// <returns>UrlResponse</returns>
        public static Url Insert(UrlshortenerService service, Url body , string apiKey = "")
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");

                // Building the initial request.
                var request = service.Url.Insert(body);

                if (apiKey != "")
                    request.Key = apiKey;

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {

                if (((dynamic)ex).Error.Code == 400)
                {
                    Console.WriteLine(((dynamic)ex).Error.Message);
                    body.Id = ((dynamic)ex).Error.Message;
                    return body;
                }
                else
                {
                    throw new Exception("Request Url.Insert failed.", ex);
                }
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/url-shortener/v1/url/list
        /// <para>Retrieves a list of URLs shortened by a user.</para>
        /// </summary>
        /// <param name="service">Authenticated urlshortener service.</param>  
        /// <param name="optional">Optional paramaters.</param>
        /// <returns>UrlHistoryResponse</returns>
        public static UrlHistory List(UrlshortenerService service, UrlListOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");

                // Building the initial request.
                var request = service.Url.List();

                // Applying optional parameters to the request.                
                request = (UrlResource.ListRequest)GoogleUrlShortenerHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Url.List failed.", ex);
            }
        }
    }

    public class GoogleUrlShortenerExtend
    {

    }

    public class GoogleUrlShortenerHelpers
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
                if (piShared != null)
                {
                    try
                    {
                        piShared.SetValue(request, property.GetValue(optional, null), null);
                    }
                    catch (ArgumentException ae)
                    {
                        Console.WriteLine(ae.Message);

                    }
                    catch (System.Reflection.TargetException te)
                    {
                        Console.WriteLine(te.Message);

                    }
                    catch (System.Reflection.TargetInvocationException tie)
                    {
                        Console.WriteLine(tie.Message);

                    }
                    catch (System.Reflection.TargetParameterCountException tpce)
                    {
                        Console.WriteLine(tpce.Message);
                    }
                    catch (MethodAccessException mae)
                    {
                        Console.WriteLine(mae.Message);
                    }
                    catch
                    {
                        Console.WriteLine("property");
                    }

                }
            }

            return request;
        }
    }
}


