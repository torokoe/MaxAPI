using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using static Google.Apis.PeopleService.v1.PeopleResource;
using System;
using System.IO;
using System.Threading;

namespace GApis.GooglePeople
{
    public class GooglePeopleStructure
    {

        #region Parameters

        static ConnectionsListOptionalParms d_clops = new ConnectionsListOptionalParms()
        {
            PersonFields = "addresses,ageRanges,biographies,birthdays,braggingRights,coverPhotos,emailAddresses,events,genders,imClients,interests,locales,memberships,metadata,names,nicknames,occupations,organizations,phoneNumbers,photos,relations,relationshipInterests,relationshipStatuses,residences,skills,taglines,urls"
        };

        static GetPersonOptionalParms d_gpops = new GetPersonOptionalParms()
        {
            PersonFields = "addresses,ageRanges,biographies,birthdays,braggingRights,coverPhotos,emailAddresses,events,genders,imClients,interests,locales,memberships,metadata,names,nicknames,occupations,organizations,phoneNumbers,photos,relations,relationshipInterests,relationshipStatuses,residences,skills,taglines,urls"
        };

        /// <summary>
        /// Documentation: https://www.googleapis.com/discovery/v1/apis/people/v1/rest
        /// </summary>
        public class ConnectionsListOptionalParms
        {
            /// <summary>
            /// The token of the page to be returned.
            /// </summary>
            public string PageToken { get; set; }

            /// <summary>
            /// The number of connections to include in the response. Valid values are between 1 and 500, inclusive. Defaults to 100.
            /// </summary>
            public int PageSize { get; set; }

            /// <summary>
            /// The order in which the connections should be sorted. Defaults to `LAST_MODIFIED_ASCENDING`.
            /// <para>Google.Apis.People.v1.PeopleResource.ConnectionsResource.ListRequest.SortOrderEnum</para>
            /// </summary>
            public string SortOrder { get; set; }

            /// <summary>
            ///  A sync token, returned by a previous call to `people.connections.list`. Only resources changed since the sync token was created are returned.
            /// </summary>
            public string SyncToken { get; set; }

            /// <summary>
            /// [DEPRECATED] A mask to restrict results to a subset of person fields.
            /// </summary>
            public object requestMask { get; set; }

            /// <summary>
            /// [DEPRECATED] (Please use personFields instead)
            /// <para>Describes which person fields to return in the response.</para>
            /// </summary>
            public object RequestMaskIncludeField { get; set; }

            /// <summary>
            /// [Required] A field mask to restrict which fields on the person are returned.
            /// </summary>
            public object PersonFields { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class GetPersonOptionalParms
        {
            /// <summary>
            /// [DEPRECATED] A mask to restrict results to a subset of person fields.
            /// </summary>
            public object requestMask { get; set; }

            /// <summary>
            /// [DEPRECATED] (Please use personFields instead)
            /// <para>Describes which person fields to return in the response.</para>
            /// </summary>
            public object RequestMaskIncludeField { get; set; }

            /// <summary>
            /// [Required] A field mask to restrict which fields on the person are returned.
            ///<para>addresses,ageRanges,biographies,birthdays,braggingRights,coverPhotos,emailAddresses,events,genders</para>
            ///<para>imClients,interests,locales,memberships,metadata,names,nicknames,occupations,organizations,phoneNumbers</para>
            ///<para>photos,relations,relationshipInterests,relationshipStatuses,residences,skills,taglines,urls</para>
            /// </summary>
            public string PersonFields { get; set; }
        }

        #endregion

        /// <summary>
        /// AuthenticateOauth
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="userName"></param>
        /// <param name="_ApplicationName"></param>
        /// <returns></returns>
        public static PeopleServiceService AuthenticateOauth(string clientId, string clientSecret, string userName, string _ApplicationName = "People Oauth2")
        {
            try
            {

                if (string.IsNullOrEmpty(clientId))
                    throw new ArgumentNullException("clientId");
                if (string.IsNullOrEmpty(clientSecret))
                    throw new ArgumentNullException("clientSecret");
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");

                // These are the scopes of permissions you need. It is best to request only what you need and not all of them                
                string[] scopes = new string[] { PeopleServiceService.Scope.UserPhonenumbersRead,	//View your phone numbers
                                                 PeopleServiceService.Scope.UserAddressesRead,   	//View your street addresses
                                                 PeopleServiceService.Scope.UserBirthdayRead,    	//View your complete date of birth
                                                 PeopleServiceService.Scope.ContactsReadonly,     	//View your contacts
                                                 PeopleServiceService.Scope.UserEmailsRead,      	//View your email addresses
                                                 PeopleServiceService.Scope.UserinfoProfile,      	//View your basic profile info
                                                 PeopleServiceService.Scope.UserinfoEmail,        	//View your email address
                                                 PeopleServiceService.Scope.PlusLogin,            	//Know your basic profile info and list of people in your circles.
                                                 PeopleServiceService.Scope.Contacts};             //Manage your contacts

                var credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = System.IO.Path.Combine(credPath, ".credentials/apiName");

                // Requesting Authentication or loading previously stored authentication for userName
                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                                             , scopes
                                                                                             , userName
                                                                                             , CancellationToken.None
                                                                                             , new FileDataStore(credPath, true)).Result;
                // Returning the SheetsService
                return new PeopleServiceService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account PeopleService failed" + ex.Message);
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
        public static PeopleServiceService AuthenticateOauth(string clientSecretJson, string userName, string _ApplicationName = "People Oauth2")
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");
                if (string.IsNullOrEmpty(clientSecretJson))
                    throw new ArgumentNullException("clientSecretJson");
                if (!System.IO.File.Exists(clientSecretJson))
                    throw new Exception("clientSecretJson file does not exist.");

                // These are the scopes of permissions you need. It is best to request only what you need and not all of them
                string[] scopes = new string[] { PeopleServiceService.Scope.UserPhonenumbersRead,	//View your phone numbers
                                                 PeopleServiceService.Scope.UserAddressesRead,   	//View your street addresses
                                                 PeopleServiceService.Scope.UserBirthdayRead,    	//View your complete date of birth
                                                 PeopleServiceService.Scope.ContactsReadonly,     	//View your contacts
                                                 PeopleServiceService.Scope.UserEmailsRead,      	//View your email addresses
                                                 PeopleServiceService.Scope.UserinfoProfile,      	//View your basic profile info
                                                 PeopleServiceService.Scope.UserinfoEmail,        	//View your email address
                                                 PeopleServiceService.Scope.PlusLogin,            	//Know your basic profile info and list of people in your circles.
                                                 PeopleServiceService.Scope.Contacts};            	//Manage your contacts
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
                return new PeopleServiceService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account PeopleService failed" + ex.Message);
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
        public static PeopleServiceService AuthenticateOauth(UserCredential _credential, string _ApplicationName = "People Oauth2")
        {
            try
            {
                return new PeopleServiceService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = _credential,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account PeopleService failed" + ex.Message);
                throw new Exception("CreateServiceFailed", ex);
            }
        }

        /// <summary>
        /// ApiKey
        /// <para>return PeopleService</para>
        /// </summary>
        /// <param name="_credential"></param>
        /// <param name="_ApplicationName"></param>
        /// <returns></returns>
        public static PeopleServiceService AuthenticateOauth(string _ApiKey, string _ApplicationName = "People Oauth2")
        {
            try
            {
                return new PeopleServiceService(new BaseClientService.Initializer()
                {
                    ApiKey = _ApiKey,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account PeopleService failed" + ex.Message);
                throw new Exception("CreateServiceFailed", ex);
            }
        }

        /// <summary>
        /// Provides a list of the authenticated user's contacts merged with any linked profiles. 
        /// Documentation: https://developers.google.com/people/v1/reference/connections/list
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated people service.</param>  
        /// <param name="resourceName">The resource name to return connections for.
        /// <para>To get information about the authenticated user, specify people/me.</para>
        /// <para>To get information about a google account, specify people/account_id.</para>
        /// <para>To get information about a contact, specify the resource name that identifies the contact as returned by- </para>
        /// <para>People.connections.list</para></param>
        /// <param name="optional">Optional paramaters.</param>        /// <returns>ListConnectionsResponseResponse</returns>
        public static ListConnectionsResponse List(PeopleServiceService service, string resourceName = "people/me" , ConnectionsListOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (resourceName == null)
                    throw new ArgumentNullException(resourceName);
                if (optional == null)
                    optional = d_clops;
                // Building the initial request.
                var request = service.People.Connections.List(resourceName);

                // Applying optional parameters to the request.                
                request = (ConnectionsResource.ListRequest)GooglePeopleHelpers.ApplyOptionalParms(request, optional);
                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Connections.List failed.", ex);
            }
        }

        /// <summary>
        /// Get profile
        /// </summary>
        /// <param name="service"></param>
        /// <param name="resourceName"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        public static Person GetPerson(PeopleServiceService service, string resourceName = "people/me", GetPersonOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (resourceName == null)
                    throw new ArgumentNullException(resourceName);
                if (optional == null)
                    optional = d_gpops;
                // Building the initial request.
                var request = service.People.Get(resourceName);

                // Applying optional parameters to the request.                
                request = (GetRequest)GooglePeopleHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception Ex)
            {
                throw new Exception("Request Files.List failed.", Ex);
            }
        }
    }

    /// <summary>
    /// GooglePeopleExtend
    /// </summary>
    public class GooglePeopleExtend
    {

    }

    public class GooglePeopleHelpers
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
                    if(piShared!=null)
                        piShared.SetValue(request, property.GetValue(optional, null), null);
            }

            return request;
        }
    }
}
