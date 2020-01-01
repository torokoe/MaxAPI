using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.AnalyticsReporting.v4;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Gmail.v1.Data;
using static Google.Apis.Gmail.v1.UsersResource;
using static GApis.OApis.GoogleMail.GoogleMailStructure;

namespace GApis.OApis.GoogleMail
{
    /// <summary>
    /// C# Document Version 0.1
    /// </summary>
    public class GoogleMailStructure
    {
        #region Parameters

        public class ThreadsGetOptionalParms
        {
            /// <summary>
            ///  The format to return the messages in.
            /// </summary>
            public ThreadsResource.GetRequest.FormatEnum Format { get; set; } = ThreadsResource.GetRequest.FormatEnum.Full;

            /// <summary>
            /// When given and format is METADATA, only include headers specified.
            /// </summary>
            public string MetadataHeaders { get; set; }
        }

        public class ThreadsListOptionalParms
        {
            /// <summary>
            /// Include threads from SPAM and TRASH in the results.
            /// </summary>
            public bool IncludeSpamTrash { get; set; }
            /// <summary>
            /// Only return threads with labels that match all of the specified label IDs.
            /// </summary>
            public string LabelIds { get; set; }
            /// <summary>
            ///  Maximum number of threads to return.
            /// </summary>
            public int MaxResults { get; set; }
            /// <summary>
            /// Page token to retrieve a specific page of results in the list.
            /// </summary>
            public string PageToken { get; set; }
            /// <summary>
            /// [Required] Only return threads matching the specified query. Supports the same query format as the Gmail search box. For example, "from:someuser@example.com rfc822msgid: is:unread". Parameter cannot be used when accessing the api using the gmail.metadata scope.
            /// </summary>
            public string Q { get; set; }

        }

        public class HistoryListOptionalParms
        {
            /// <summary>
            /// Only return messages with a label matching the ID.
            /// </summary>
            public string LabelId { get; set; }
            /// <summary>
            /// The maximum number of history records to return.
            /// </summary>
            public int MaxResults { get; set; }
            /// <summary>
            /// Page token to retrieve a specific page of results in the list.
            /// </summary>
            public string PageToken { get; set; }
            /// <summary>
            /// [Required] Returns history records after the specified startHistoryId. The supplied startHistoryId should be obtained from the historyId of a message, thread, or previous list response. History IDs increase chronologically but are not contiguous with random gaps in between valid IDs. Supplying an invalid or out of date startHistoryId typically returns an HTTP 404 error code. A historyId is typically valid for at least a week, but in some rare circumstances may be valid for only a few hours. If you receive an HTTP 404 error response, your application should perform a full sync. If you receive no nextPageToken in the response, there are no updates to retrieve and you can store the returned historyId for a future request.
            /// </summary>
            public string StartHistoryId { get; set; }

        }

        public class DraftsListOptionalParms
        {
            /// <summary>
            /// Include drafts from SPAM and TRASH in the results.
            /// </summary>
            public bool IncludeSpamTrash { get; set; }
            /// <summary>
            ///  Maximum number of drafts to return.
            /// </summary>
            public int MaxResults { get; set; }
            /// <summary>
            /// Page token to retrieve a specific page of results in the list.
            /// </summary>
            public string PageToken { get; set; }
            /// <summary>
            /// [Required] Only return draft messages matching the specified query. Supports the same query format as the Gmail search box. For example, "from:someuser@example.com rfc822msgid: is:unread".
            /// </summary>
            public string Q { get; set; }

        }

        public class DraftsGetOptionalParms
        {
            /// <summary>
            ///  The format to return the draft in.
            /// </summary>
            public DraftsResource.GetRequest.FormatEnum Format { get; set; } = DraftsResource.GetRequest.FormatEnum.Full;
        }

        public class MessagesInsertOptionalParms
        {
            /// <summary>
            /// Mark the email as permanently deleted (not TRASH) and only visible in Google Vault to a Vault administrator. Only used for G Suite accounts. (Default: false)
            /// </summary>
            public bool deleted { get; set; } = false;

            /// <summary>
            /// Source for Gmail's internal date of the message. 
            /// <para>Acceptable values are:</para>
            /// <para>"dateHeader": The internal message time is based on the Date header in the email, when valid.</para>
            /// <para>"receivedTime": The internal message date is set to the time the message is received by Gmail. (default)</para>
            /// </summary>
            public MessagesResource.InsertRequest.InternalDateSourceEnum internalDateSource { get; set; } = MessagesResource.InsertRequest.InternalDateSourceEnum.ReceivedTime;

        }

        public class MessagesGetOptionalParms
        {
            /// <summary>
            /// The format to return the message in.
            /// <para>Acceptable values are:</para>
            /// "full": Returns the full email message data with body content parsed in the payload field; the raw field is not used. (default)
            /// "metadata": Returns only email message ID, labels, and email headers.
            /// "minimal": Returns only email message ID and labels; does not return the email headers, body, or payload.
            /// "raw": Returns the full email message data with body content in the raw field as a base64url encoded string; the payload field is not used.
            /// </summary>
            public MessagesResource.GetRequest.FormatEnum Format { get; set; } = MessagesResource.GetRequest.FormatEnum.Full;

            /// <summary>
            /// When given and format is METADATA, only include headers specified.
            /// </summary>
       ///     public string metadataHeaders { get; set; } = null;
        }

        public class MessagesListOptionalParms
        {
            /// <summary>
            /// Include messages from SPAM and TRASH in the results. (Default: false)
            /// </summary>
            public bool includeSpamTrash { get; set; } = false;

            /// <summary>
            /// Only return messages with labels that match all of the specified label IDs.
            /// </summary>
            public string labelIds { get; set; }

            /// <summary>
            /// Maximum number of messages to return.
            /// </summary>
            public uint maxResults { get; set; }

            /// <summary>
            /// Page token to retrieve a specific page of results in the list.
            /// </summary>
            public string pageToken { get; set; }

            /// <summary>
            /// Only return messages matching the specified query. 
            /// <para>Supports the same query format as the Gmail search box.</para>
            /// <para>For example, "from:someuser@example.com rfc822msgid: is:unread". Parameter cannot be used when accessing the api using the gmail.metadata scope.</para>
            /// </summary>
            public string Q { get; set; }
        }

        public class MessagesImportOptionalParms
        {
            /// <summary>
            /// Mark the email as permanently deleted (not TRASH) and only visible in Google Vault to a Vault administrator. Only used for G Suite accounts. (Default: false)
            /// </summary>
            public bool deleted { get; set; } = false;

            /// <summary>
            /// Source for Gmail's internal date of the message. 
            /// <para>Acceptable values are:</para>
            /// <para>"dateHeader": The internal message time is based on the Date header in the email, when valid. (default)</para>
            /// <para>"receivedTime": The internal message date is set to the time the message is received by Gmail.</para>
            /// </summary>
            public MessagesResource.ImportRequest.InternalDateSourceEnum internalDateSource { get; set; } = MessagesResource.ImportRequest.InternalDateSourceEnum.DateHeader;

            /// <summary>
            /// Ignore the Gmail spam classifier decision and never mark this email as SPAM in the mailbox. (Default: false)
            /// </summary>
            public bool neverMarkSpam { get; set; } = false;

            /// <summary>
            /// Process calendar invites in the email and add any extracted meetings to the Google Calendar for this user. (Default: false)
            /// </summary>
            public bool processForCalendar { get; set; } = false;
        }

        #endregion

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
        public static GmailService AuthenticateOauth(string clientId, string clientSecret, string userName, string _ApplicationName = "GmailService Oauth2")
        {
            try
            {
                if (string.IsNullOrEmpty(clientId))
                    throw new ArgumentNullException("clientId");
                if (string.IsNullOrEmpty(clientSecret))
                    throw new ArgumentNullException("clientSecret");
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");

                string[] scopes = new string[] { GmailService.Scope.GmailReadonly };

                var credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = System.IO.Path.Combine(credPath, ".credentials/apiName");

                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                                             , scopes
                                                                                             , userName
                                                                                             , CancellationToken.None
                                                                                             , new FileDataStore(credPath, true)).Result;
                return new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account GmailService failed" + ex.Message);
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
        public static GmailService AuthenticateOauth(UserCredential _credential, string _ApplicationName = "GmailService Oauth2")
        {
            try
            {
                return new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = _credential,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account GmailService failed" + ex.Message);
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
        public static GmailService AuthenticateOauth(string _ApiKey, string _ApplicationName = "GmailService Oauth2")
        {
            try
            {
                return new GmailService(new BaseClientService.Initializer()
                {
                    ApiKey = _ApiKey,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account GmailService failed" + ex.Message);
                throw new Exception("CreateServiceFailed", ex);
            }
        }
    }

    /// <summary>
    /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/threads
    /// <para>A collection of messages representing a conversation.</para>
    /// </summary>
    public class GoogleMailThreadsStructure
    {
        #region Threads

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/threads/delete
        /// <para>Immediately and permanently deletes the specified thread. This operation cannot be undone. Prefer threads.trash instead.</para> 
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">ID of the Thread to delete.</param>
        public static string Delete(GmailService service, string userId, string id)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Threads.Delete(userId, id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Threads.Delete failed.", ex);
            }
        }


        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/threads/get
        /// <para>Gets the specified thread.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the thread to retrieve.</param>
        /// <param name="optional">Optional paramaters.</param>        /// <returns>ThreadResponse</returns>
        public static Google.Apis.Gmail.v1.Data.Thread Get(GmailService service, string userId, string id, ThreadsGetOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Building the initial request.
                var request = service.Users.Threads.Get(userId, id);

                // Applying optional parameters to the request.                
                request = (ThreadsResource.GetRequest)GoogleMailHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Threads.Get failed.", ex);
            }
        }


        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/threads/list
        /// <para>Lists the threads in the user's mailbox.</para> 
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="optional">Optional paramaters.</param>        /// <returns>ListThreadsResponseResponse</returns>
        public static ListThreadsResponse List(GmailService service, string userId, ThreadsListOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Building the initial request.
                var request = service.Users.Threads.List(userId);

                // Applying optional parameters to the request.                
                request = (ThreadsResource.ListRequest)GoogleMailHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Threads.List failed.", ex);
            }
        }


        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/threads/modify
        /// <para>Modifies the labels applied to the thread. This applies to all messages in the thread.</para> 
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the thread to modify.</param>
        /// <param name="body">A valid gmail v1 body.</param>
        /// <returns>ThreadResponse</returns>
        public static Google.Apis.Gmail.v1.Data.Thread Modify(GmailService service, string userId, string id, ModifyThreadRequest body)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Threads.Modify(body, userId, id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Threads.Modify failed.", ex);
            }
        }


        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/threads/trash
        /// <para>Moves the specified thread to the trash.</para> 
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the thread to Trash.</param>
        /// <returns>ThreadResponse</returns>
        public static Google.Apis.Gmail.v1.Data.Thread Trash(GmailService service, string userId, string id)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Threads.Trash(userId, id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Threads.Trash failed.", ex);
            }
        }


        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/threads/untrash
        /// <para>Removes the specified thread from the trash.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the thread to remove from Trash.</param>
        /// <returns>ThreadResponse</returns>
        public static Google.Apis.Gmail.v1.Data.Thread Untrash(GmailService service, string userId, string id)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Threads.Untrash(userId, id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Threads.Untrash failed.", ex);
            }
        }

        #endregion
    }

    /// <summary>
    /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/labels
    /// <para>Labels are used to categorize messages and threads within the user's mailbox.</para>
    /// </summary>
    public class GoogleMailLabelsStructure
    {
        #region Labels

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/labels/create
        /// <para>Creates a new label.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="body">A valid gmail v1 body.</param>
        /// <returns>LabelResponse</returns>
        public static Label Create(GmailService service, string userId, Label body)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Make the request.
                return service.Users.Labels.Create(body, userId).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Labels.Create failed.", ex);
            }
        }


        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/labels/delete
        /// <para>Immediately and permanently deletes the specified label and removes it from any messages and threads that it is applied to.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the label to delete.</param>
        public static string Delete(GmailService service, string userId, string id)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Labels.Delete(userId, id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Labels.Delete failed.", ex);
            }
        }


        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/labels/get
        /// <para>Gets the specified label.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the label to retrieve.</param>
        /// <returns>LabelResponse</returns>
        public static Label Get(GmailService service, string userId, string id)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Labels.Get(userId, id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Labels.Get failed.", ex);
            }
        }


        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/labels/list
        /// <para>Lists all labels in the user's mailbox.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <returns>ListLabelsResponseResponse</returns>
        public static ListLabelsResponse List(GmailService service, string userId)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Make the request.
                return service.Users.Labels.List(userId).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Labels.List failed.", ex);
            }
        }


        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/labels/patch
        /// <para>Updates the specified label. This method supports patch semantics.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the label to update.</param>
        /// <param name="body">A valid gmail v1 body.</param>
        /// <returns>LabelResponse</returns>
        public static Label Patch(GmailService service, string userId, string id, Label body)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Labels.Patch(body, userId, id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Labels.Patch failed.", ex);
            }
        }


        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/labels/update
        /// <para>Updates the specified label.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the label to update.</param>
        /// <param name="body">A valid gmail v1 body.</param>
        /// <returns>LabelResponse</returns>
        public static Label Update(GmailService service, string userId, string id, Label body)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Labels.Update(body, userId, id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Labels.Update failed.", ex);
            }
        }

        #endregion
    }

    /// <summary>
    /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/history
    /// <para>There is no persistent data associated with this resource.</para>
    /// </summary>
    public class GoogleMailHistoryStructure
    {
        #region History

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/Users/history/list
        /// <para>Lists the history of all changes to the given mailbox. History results are returned in chronological order (increasing historyId).</para> 
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="optional">Optional paramaters.</param>        /// <returns>ListHistoryResponseResponse</returns>
        public static ListHistoryResponse List(GmailService service, string userId, HistoryListOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Building the initial request.
                var request = service.Users.History.List(userId);

                // Applying optional parameters to the request.                
                request = (HistoryResource.ListRequest)GoogleMailHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request History.List failed.", ex);
            }
        }

        #endregion
    }

    /// <summary>
    /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings
    /// <para>There is no persistent data associated with this resource.</para>
    /// </summary>
    public class GoogleMailSettingStructure
    {

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/filters
        /// <para>Resource definition for Gmail filters. Filters apply to specific messages instead of an entire email thread.</para>
        /// </summary>
        public class GoogleMailSettingFilters
        {
            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/filters/create
            /// <para>Creates a filter.</para>
            /// </summary>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <param name="body">A valid gmail v1 body.</param>
            /// <returns>FilterResponse</returns>
            public static Filter Create(GmailService service, string userId, Filter body)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (body == null)
                        throw new ArgumentNullException("body");
                    if (userId == null)
                        throw new ArgumentNullException(userId);

                    // Make the request.
                    return service.Users.Settings.Filters.Create(body, userId).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request Filters.Create failed.", ex);
                }
            }


            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/filters/delete
            /// <para>Deletes a filter.</para>
            /// </summary>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <param name="id">The ID of the filter to be deleted.</param>
            public static string Delete(GmailService service, string userId, string id)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (userId == null)
                        throw new ArgumentNullException(userId);
                    if (id == null)
                        throw new ArgumentNullException(id);

                    // Make the request.
                    return service.Users.Settings.Filters.Delete(userId, id).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request Filters.Delete failed.", ex);
                }
            }


            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/filters/get
            /// <para>Gets a filter.</para>
            /// </summary>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <param name="id">The ID of the filter to be fetched.</param>
            /// <returns>FilterResponse</returns>
            public static Filter Get(GmailService service, string userId, string id)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (userId == null)
                        throw new ArgumentNullException(userId);
                    if (id == null)
                        throw new ArgumentNullException(id);

                    // Make the request.
                    return service.Users.Settings.Filters.Get(userId, id).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request Filters.Get failed.", ex);
                }
            }

            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/filters/list
            /// <para>Lists the message filters of a Gmail user.</para> 
            /// </summary>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <returns>ListFiltersResponseResponse</returns>
            public static ListFiltersResponse List(GmailService service, string userId)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (userId == null)
                        throw new ArgumentNullException(userId);

                    // Make the request.
                    return service.Users.Settings.Filters.List(userId).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request Filters.List failed.", ex);
                }
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/sendAs
        /// <para>Settings associated with a send-as alias, which can be either the primary login address associated with the account or a custom "from" address.</para>
        /// <para>Send-as aliases correspond to the "Send Mail As" feature in the web interface.</para>
        /// </summary>
        public class GoogleMailSettingSendAs
        {

            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/sendAs/create
            /// <para>Creates a custom "from" send-as alias. If an SMTP MSA is specified, Gmail will attempt to connect to the SMTP service to validate the configuration before creating the alias.</para>
            /// <para>If ownership verification is required for the alias, a message will be sent to the email address and the resource's verification status will be set to pending;</para>
            /// <para>otherwise, the resource will be created with verification status set to accepted. If a signature is provided, Gmail will sanitize the HTML before saving it with the alias.</para>
            /// </summary>
            /// <remarks>This method is only available to service account clients that have been delegated domain-wide authority.</remarks>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <param name="body">A valid gmail v1 body.</param>
            public static SendAs Create(GmailService service, string userId, SendAs body)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (body == null)
                        throw new ArgumentNullException("body");
                    if (userId == null)
                        throw new ArgumentNullException(userId);

                    // Make the request.
                    return service.Users.Settings.SendAs.Create(body, userId).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request SendAs.Create failed.", ex);
                }
            }


            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/sendAs/delete
            /// <para>Deletes the specified send-as alias. Revokes any verification that may have been required for using it.</para>
            /// </summary>
            /// <remarks>This method is only available to service account clients that have been delegated domain-wide authority.</remarks>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <param name="sendAsEmail">The send-as alias to be deleted.</param>
            public static string Delete(GmailService service, string userId, string sendAsEmail)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (userId == null)
                        throw new ArgumentNullException(userId);
                    if (sendAsEmail == null)
                        throw new ArgumentNullException(sendAsEmail);

                    // Make the request.
                    return service.Users.Settings.SendAs.Delete(userId, sendAsEmail).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request SendAs.Delete failed.", ex);
                }
            }


            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/sendAs/get
            /// <para>Gets the specified send-as alias. Fails with an HTTP 404 error if the specified address is not a member of the collection.</para>
            /// </summary>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <param name="sendAsEmail">The send-as alias to be retrieved.</param>
            public static SendAs Get(GmailService service, string userId, string sendAsEmail)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (userId == null)
                        throw new ArgumentNullException(userId);
                    if (sendAsEmail == null)
                        throw new ArgumentNullException(sendAsEmail);

                    // Make the request.
                    return service.Users.Settings.SendAs.Get(userId, sendAsEmail).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request SendAs.Get failed.", ex);
                }
            }


            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/sendAs/list
            /// <para>Lists the send-as aliases for the specified account. The result includes the primary send-as address associated with the account as well as any custom "from" aliases.</para>
            /// </summary>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            public static ListSendAsResponse List(GmailService service, string userId)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (userId == null)
                        throw new ArgumentNullException(userId);

                    // Make the request.
                    return service.Users.Settings.SendAs.List(userId).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request SendAs.List failed.", ex);
                }
            }


            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/sendAs/patch
            /// <para>Updates a send-as alias. If a signature is provided, Gmail will sanitize the HTML before saving it with the alias.</para>
            /// <para>Addresses other than the primary address for the account can only be updated by service account clients that have been delegated domain-wide authority. This method supports patch semantics.</para>
            /// </summary>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <param name="sendAsEmail">The send-as alias to be updated.</param>
            /// <param name="body">A valid gmail v1 body.</param>
            /// <returns>SendAsResponse</returns>
            public static SendAs Patch(GmailService service, string userId, string sendAsEmail, SendAs body)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (body == null)
                        throw new ArgumentNullException("body");
                    if (userId == null)
                        throw new ArgumentNullException(userId);
                    if (sendAsEmail == null)
                        throw new ArgumentNullException(sendAsEmail);

                    // Make the request.
                    return service.Users.Settings.SendAs.Patch(body, userId, sendAsEmail).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request SendAs.Patch failed.", ex);
                }
            }


            /// <summary> 
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/sendAs/update
            /// <para>Updates a send-as alias. If a signature is provided, Gmail will sanitize the HTML before saving it with the alias.</para>
            /// <para>Addresses other than the primary address for the account can only be updated by service account clients that have been delegated domain-wide authority.</para>
            /// </summary>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <param name="sendAsEmail">The send-as alias to be updated.</param>
            /// <param name="body">A valid gmail v1 body.</param>
            public static SendAs Update(GmailService service, string userId, string sendAsEmail, SendAs body)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (body == null)
                        throw new ArgumentNullException("body");
                    if (userId == null)
                        throw new ArgumentNullException(userId);
                    if (sendAsEmail == null)
                        throw new ArgumentNullException(sendAsEmail);

                    // Make the request.
                    return service.Users.Settings.SendAs.Update(body, userId, sendAsEmail).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request SendAs.Update failed.", ex);
                }
            }


            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/sendAs/verify
            /// <para>Sends a verification email to the specified send-as alias address. The verification status must be pending.</para>
            /// </summary>
            /// <remarks>This method is only available to service account clients that have been delegated domain-wide authority.</remarks>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <param name="sendAsEmail">The send-as alias to be verified.</param>
            public static string Verify(GmailService service, string userId, string sendAsEmail)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (userId == null)
                        throw new ArgumentNullException(userId);
                    if (sendAsEmail == null)
                        throw new ArgumentNullException(sendAsEmail);

                    // Make the request.
                    return service.Users.Settings.SendAs.Verify(userId, sendAsEmail).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request SendAs.Verify failed.", ex);
                }
            }

            /// <summary>
            /// An S/MIME email config.
            /// </summary>
            public class GoogleMailSettingSendAsSmimeInfo
            {
                /// <summary>
                /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/sendAs/smimeInfo/delete
                /// <para>Deletes the specified S/MIME config for the specified send-as alias.</para>
                /// </summary>
                /// <param name="service">Authenticated gmail service.</param>
                /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
                /// <param name="sendAsEmail">The send-as alias to be retrieved.</param>
                /// <param name="id">The immutable ID for the SmimeInfo.</param>
                /// <returns></returns>
                public static string Delete(GmailService service, string userId, string sendAsEmail, string id)
                {
                    try
                    {
                        // Initial validation.
                        if (service == null)
                            throw new ArgumentNullException("service");
                        if (userId == null)
                            throw new ArgumentNullException(userId);
                        if (sendAsEmail == null)
                            throw new ArgumentNullException(sendAsEmail);
                        if (id == null)
                            throw new ArgumentNullException(id);

                        // Make the request.
                        return service.Users.Settings.SendAs.SmimeInfo.Delete(userId, sendAsEmail, id).Execute();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Request SendAs.SmimeInfo.Delete failed.", ex);
                    }
                }

                /// <summary>
                /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/sendAs/smimeInfo/get
                /// <para>Gets the specified S/MIME config for the specified send-as alias.</para>
                /// </summary>
                /// <param name="service">Authenticated gmail service.</param>  
                /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
                /// <param name="sendAsEmail">The send-as alias to be retrieved.</param>
                /// <param name="id">The immutable ID for the SmimeInfo.</param>
                public static SmimeInfo Get(GmailService service, string userId, string sendAsEmail, string id)
                {
                    try
                    {
                        // Initial validation.
                        if (service == null)
                            throw new ArgumentNullException("service");
                        if (userId == null)
                            throw new ArgumentNullException(userId);
                        if (sendAsEmail == null)
                            throw new ArgumentNullException(sendAsEmail);
                        if (id == null)
                            throw new ArgumentNullException(id);

                        // Make the request.
                        return service.Users.Settings.SendAs.SmimeInfo.Get(userId, sendAsEmail, id).Execute();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Request SendAs.SmimeInfo.Get failed.", ex);
                    }
                }

                /// <summary>
                /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/sendAs/smimeInfo/insert
                /// <para>Insert (upload) the given S/MIME config for the specified send-as alias. Note that pkcs12 format is required for the key.</para>
                /// </summary>
                /// <param name="service">Authenticated gmail service.</param>
                /// <param name="body"></param>
                /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
                /// <param name="sendAsEmail">The email address that appears in the "From:" header for mail sent using this alias.</param>
                public static SmimeInfo Insert(GmailService service, SmimeInfo body, string userId, string sendAsEmail)
                {
                    try
                    {
                        // Initial validation.
                        if (service == null)
                            throw new ArgumentNullException("service");
                        if (body == null)
                            throw new ArgumentNullException("body");
                        if (userId == null)
                            throw new ArgumentNullException(userId);
                        if (sendAsEmail == null)
                            throw new ArgumentNullException(sendAsEmail);

                        // Make the request.
                        return service.Users.Settings.SendAs.SmimeInfo.Insert(body, userId, sendAsEmail).Execute();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Request SendAs.SmimeInfo.Insert failed.", ex);
                    }
                }

                /// <summary>
                /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/sendAs/list
                /// <para>Lists S/MIME configs for the specified send-as alias.</para>
                /// </summary>
                /// <param name="service">Authenticated gmail service.</param>  
                /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
                /// <param name="sendAsEmail">The email address that appears in the "From:" header for mail sent using this alias.</param>
                public static ListSmimeInfoResponse List(GmailService service, string userId, string sendAsEmail)
                {
                    try
                    {
                        // Initial validation.
                        if (service == null)
                            throw new ArgumentNullException("service");
                        if (userId == null)
                            throw new ArgumentNullException(userId);
                        if (sendAsEmail == null)
                            throw new ArgumentNullException(sendAsEmail);

                        // Make the request.
                        return service.Users.Settings.SendAs.SmimeInfo.List(userId, sendAsEmail).Execute();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Request SendAs.SmimeInfo.List failed.", ex);
                    }
                }

                /// <summary>
                /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/sendAs/smimeInfo/setDefault
                /// <para>Sets the default S/MIME config for the specified send-as alias.</para>
                /// </summary>
                /// <param name="service">Authenticated gmail service.</param>  
                /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
                /// <param name="sendAsEmail">The send-as alias to be retrieved.</param>
                /// <param name="id">The immutable ID for the SmimeInfo.</param>
                public static string SetDefault(GmailService service, string userId, string sendAsEmail, string id)
                {
                    try
                    {
                        // Initial validation.
                        if (service == null)
                            throw new ArgumentNullException("service");
                        if (userId == null)
                            throw new ArgumentNullException(userId);
                        if (sendAsEmail == null)
                            throw new ArgumentNullException(sendAsEmail);
                        if (id == null)
                            throw new ArgumentNullException(id);

                        // Make the request.
                        return service.Users.Settings.SendAs.SmimeInfo.SetDefault(userId, sendAsEmail, id).Execute();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Request SendAs.SmimeInfo.SetDefault failed.", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/forwardingAddresses
        /// <para>Settings for a forwarding address.</para>
        /// </summary>
        public class GoogleMailSettingForwardingAddresses
        {
            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/forwardingAddresses/create
            /// <para>Creates a forwarding address. If ownership verification is required, a message will be sent to the recipient and the resource's verification status will be set to pending;</para>
            /// <para>otherwise, the resource will be created with verification status set to accepted.</para>
            /// </summary>
            /// <remarks>This method is only available to service account clients that have been delegated domain-wide authority.</remarks>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <param name="body">A valid gmail v1 body.</param>
            /// <returns>ForwardingAddressResponse</returns>
            public static ForwardingAddress Create(GmailService service, string userId, ForwardingAddress body)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (body == null)
                        throw new ArgumentNullException("body");
                    if (userId == null)
                        throw new ArgumentNullException(userId);

                    // Make the request.
                    return service.Users.Settings.ForwardingAddresses.Create(body, userId).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request ForwardingAddresses.Create failed.", ex);
                }
            }


            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/forwardingAddresses/delete
            /// <para>Deletes the specified forwarding address and revokes any verification that may have been required.</para>
            /// </summary>
            /// <remarks>This method is only available to service account clients that have been delegated domain-wide authority.</remarks>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <param name="forwardingEmail">The forwarding address to be deleted.</param>
            public static string Delete(GmailService service, string userId, string forwardingEmail)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (userId == null)
                        throw new ArgumentNullException(userId);
                    if (forwardingEmail == null)
                        throw new ArgumentNullException(forwardingEmail);

                    // Make the request.
                    return service.Users.Settings.ForwardingAddresses.Delete(userId, forwardingEmail).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request ForwardingAddresses.Delete failed.", ex);
                }
            }


            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/forwardingAddresses/get
            /// <para>Gets the specified forwarding address.</para>
            /// </summary>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <param name="forwardingEmail">The forwarding address to be retrieved.</param>
            /// <returns>ForwardingAddressResponse</returns>
            public static ForwardingAddress Get(GmailService service, string userId, string forwardingEmail)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (userId == null)
                        throw new ArgumentNullException(userId);
                    if (forwardingEmail == null)
                        throw new ArgumentNullException(forwardingEmail);

                    // Make the request.
                    return service.Users.Settings.ForwardingAddresses.Get(userId, forwardingEmail).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request ForwardingAddresses.Get failed.", ex);
                }
            }


            /// <summary>
            /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/settings/forwardingAddresses/list
            /// <para>Lists the forwarding addresses for the specified account.</para>
            /// </summary>
            /// <param name="service">Authenticated gmail service.</param>  
            /// <param name="userId">User's email address. The special value "me" can be used to indicate the authenticated user.</param>
            /// <returns>ListForwardingAddressesResponseResponse</returns>
            public static ListForwardingAddressesResponse List(GmailService service, string userId)
            {
                try
                {
                    // Initial validation.
                    if (service == null)
                        throw new ArgumentNullException("service");
                    if (userId == null)
                        throw new ArgumentNullException(userId);

                    // Make the request.
                    return service.Users.Settings.ForwardingAddresses.List(userId).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Request ForwardingAddresses.List failed.", ex);
                }
            }
        }

    }

    /// <summary>
    /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages
    /// <para>An email message.</para>
    /// </summary>
    public class GoogleMailMessagesStructure
    {

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/attachments
        /// <para>The body of a single MIME message part.</para>
        /// </summary>
        public class GoogleMailMessagesAttachments
            {
                /// <summary>
                /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/attachments/get
                /// <para>Gets the specified message attachment.</para>
                /// </summary>
                /// <param name="service">Authenticated gmail service.</param>  
                /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
                /// <param name="messageId">The ID of the message containing the attachment.</param>
                /// <param name="id">The ID of the attachment.</param>
                /// <returns>MessagePartBodyResponse</returns>
                public static MessagePartBody Get(GmailService service, string userId, string messageId, string id)
                {
                    try
                    {
                        // Initial validation.
                        if (service == null)
                            throw new ArgumentNullException("service");
                        if (userId == null)
                            throw new ArgumentNullException(userId);
                        if (messageId == null)
                            throw new ArgumentNullException(messageId);
                        if (id == null)
                            throw new ArgumentNullException(id);

                        // Make the request.
                        return service.Users.Messages.Attachments.Get(userId, messageId, id).Execute();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Request Attachments.Get failed.", ex);
                    }
                }
            }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/insert
        /// <para>Directly inserts a message into only this user's mailbox similar to IMAP APPEND, bypassing most scanning and classification. Does not send a message.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="body">The type of upload request to the /upload URI. Acceptable values are:
        /// <para>media - Simple upload. Upload the media only, without any metadata.</para>
        /// <para>multipart - Multipart upload. Upload both the media and its metadata, in a single request.</para>
        /// <para>resumable - Resumable upload. Upload the file in a resumable fashion, using a series of at least two requests where the first request includes the metadata.</para></param>
        /// <param name="optional">Optional paramaters.</param>
        public static Message Insert(GmailService service, string userId, Message body, MessagesInsertOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Building the initial request.
                var request = service.Users.Messages.Insert(body, userId);

                // Applying optional parameters to the request.                
                request = (MessagesResource.InsertRequest)GoogleMailHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Messages.Insert failed.", ex);
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/delete
        /// <para>Immediately and permanently deletes the specified message. This operation cannot be undone. Prefer messages.trash instead.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the message to delete.</param>
        public static string Delete(GmailService service, string userId, string id)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Messages.Delete(userId, id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Messages.Delete failed.", ex);
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/get
        /// <para>Gets the specified message.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the message to retrieve.</param>
        /// <param name="optional">Optional paramaters.</param>
        public static Message Get(GmailService service, string userId, string id, MessagesGetOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Building the initial request.
                var request = service.Users.Messages.Get(userId, id);

                // Applying optional parameters to the request.                
                request = (MessagesResource.GetRequest)GoogleMailHelpers.ApplyOptionalParms(request, optional);
                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Messages.Get failed.", ex);
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/list
        /// <para>Lists the messages in the user's mailbox.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="optional">Optional paramaters.</param>
        public static ListMessagesResponse List(GmailService service, string userId, MessagesListOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Building the initial request.
                var request = service.Users.Messages.List(userId);

                // Applying optional parameters to the request.                
                request = (MessagesResource.ListRequest)GoogleMailHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.(100records)
                //return request.Execute();

                //PageStreamer(all records)
                var pageStreamer = new Google.Apis.Requests.PageStreamer<Google.Apis.Gmail.v1.Data.Message, MessagesResource.ListRequest, Google.Apis.Gmail.v1.Data.ListMessagesResponse, string>(
                                                   (req, token) => request.PageToken = token,
                                                   response => response.NextPageToken,
                                                   response => response.Messages);

                var allMessages = new Google.Apis.Gmail.v1.Data.ListMessagesResponse();
                allMessages.Messages = new List<Google.Apis.Gmail.v1.Data.Message>();

                foreach (var result in pageStreamer.Fetch(request))
                {
                    allMessages.Messages.Add(result);
                }

                return allMessages;
            }
            catch (Exception ex)
            {
                throw new Exception("Request Messages.List failed.", ex);
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/send
        /// <para>Sends the specified message to the recipients in the To, Cc, and Bcc headers.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="body">The type of upload request to the /upload URI. Acceptable values are:
        /// <para>media - Simple upload. Upload the media only, without any metadata.</para>
        /// <para>multipart - Multipart upload. Upload both the media and its metadata, in a single request.</para>
        /// <para>resumable - Resumable upload. Upload the file in a resumable fashion, using a series of at least two requests where the first request includes the metadata.</para></param>
        public static Message Send(GmailService service, string userId, Message body)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Make the request.
                return service.Users.Messages.Send(body, userId).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Messages.Send failed.", ex);
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/modify
        /// <para>Modifies the labels on the specified message.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="body">addLabelIds[] : A list of IDs of labels to add to this message.
        /// <para>A list IDs of labels to remove from this message.	</para></param>
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the message to modify.</param>
        /// <returns>DraftResponse</returns>
        public static Message Modify(GmailService service,ModifyMessageRequest body, string userId, string id)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Messages.Modify(body, userId, id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Messages.Modify failed.", ex);
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/trash
        /// <para>Moves the specified message to the trash.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the message to Trash.</param>
        public static Message Trash(GmailService service, string userId, string id)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Messages.Trash(userId, userId).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Messages.Trash failed.", ex);
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/untrash
        /// <para>Removes the specified message from the trash.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the message to Trash.</param>
        public static Message UnTrash(GmailService service, string userId, string id)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Messages.Untrash(userId, userId).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Messages.Untrash failed.", ex);
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/import
        /// <para>Imports a message into only this user's mailbox, with standard email delivery scanning and classification similar to receiving via SMTP. Does not send a message.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="body">The type of upload request to the /upload URI. Acceptable values are:
        /// <para>media - Simple upload. Upload the media only, without any metadata.</para>
        /// <para>multipart - Multipart upload. Upload both the media and its metadata, in a single request.</para>
        /// <para>resumable - Resumable upload. Upload the file in a resumable fashion, using a series of at least two requests where the first request includes the metadata.</para></param>
        /// <param name="optional">Optional paramaters.</param>
        public static Message Import(GmailService service, string userId, Message body, MessagesListOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Building the initial request.
                var request = service.Users.Messages.Import(body,userId);

                // Applying optional parameters to the request.                
                request = (MessagesResource.ImportRequest)GoogleMailHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Messages.Import failed.", ex);
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/batchDelete
        /// <para>Deletes many messages by message ID. Provides no guarantees that messages were not already deleted or even existed at all.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="body"><para>ids[] : The IDs of the messages to delete.</para></param>
        public static string BatchDelete(GmailService service, string userId, BatchDeleteMessagesRequest body)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Make the request.
                return service.Users.Messages.BatchDelete(body, userId).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Messages.BatchDelete failed.", ex);
            }
        }

        /// <summary>
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/messages/batchModify
        /// <para>Modifies the labels on the specified messages.</para>
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="body"><para>ids[] : The IDs of the messages to delete.</para>
        /// <para>addLabelIds[]	: A list of label IDs to add to messages.</para>
        /// <para>removeLabelIds[] : A list of label IDs to remove from messages.</para></param>
        public static string BatchModify(GmailService service, string userId, BatchModifyMessagesRequest body)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Make the request.
                return service.Users.Messages.BatchModify(body, userId).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Messages.BatchModify failed.", ex);
            }
        }

    }

    /// <summary>
    /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/drafts
    /// <para>A draft email in the user's mailbox.</para>
    /// </summary>
    public class GoogleMailDraftsStructure
    {
        /// <summary>
        /// Creates a new draft with the DRAFT label. 
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/drafts/create
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="body">A valid gmail v1 body.</param>
        /// <returns>DraftResponse</returns>
        public static Draft Create(GmailService service, string userId, Draft body)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Make the request.
                return service.Users.Drafts.Create(body, userId).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Drafts.Create failed.", ex);
            }
        }


        /// <summary>
        /// Immediately and permanently deletes the specified draft. Does not simply trash it. 
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/drafts/delete
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the draft to delete.</param>
        public static string Delete(GmailService service, string userId, string id)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Drafts.Delete(userId, id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Drafts.Delete failed.", ex);
            }
        }


        /// <summary>
        /// Gets the specified draft. 
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/drafts/get
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the draft to retrieve.</param>
        /// <param name="optional">Optional paramaters.</param>        /// <returns>DraftResponse</returns>
        public static Draft Get(GmailService service, string userId, string id, DraftsGetOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Building the initial request.
                var request = service.Users.Drafts.Get(userId, id);

                // Applying optional parameters to the request.                
                request = (DraftsResource.GetRequest)GoogleMailHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Drafts.Get failed.", ex);
            }
        }


        /// <summary>
        /// Lists the drafts in the user's mailbox. 
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/drafts/list
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="optional">Optional paramaters.</param>        /// <returns>ListDraftsResponseResponse</returns>
        public static ListDraftsResponse List(GmailService service, string userId, DraftsListOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Building the initial request.
                var request = service.Users.Drafts.List(userId);

                // Applying optional parameters to the request.                
                request = (DraftsResource.ListRequest)GoogleMailHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Drafts.List failed.", ex);
            }
        }


        /// <summary>
        /// Sends the specified, existing draft to the recipients in the To, Cc, and Bcc headers. 
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/drafts/send
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="body">A valid gmail v1 body.</param>
        /// <returns>MessageResponse</returns>
        public static Message Send(GmailService service, string userId, Draft body)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);

                // Make the request.
                return service.Users.Drafts.Send(body, userId).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Drafts.Send failed.", ex);
            }
        }


        /// <summary>
        /// Replaces a draft's content. 
        /// Documentation: https://developers.google.com/gmail/api/v1/reference/users/drafts/update
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated gmail service.</param>  
        /// <param name="userId">The user's email address. The special value me can be used to indicate the authenticated user.</param>
        /// <param name="id">The ID of the draft to update.</param>
        /// <param name="body">A valid gmail v1 body.</param>
        /// <returns>DraftResponse</returns>
        public static Draft Update(GmailService service, string userId, string id, Draft body)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");
                if (userId == null)
                    throw new ArgumentNullException(userId);
                if (id == null)
                    throw new ArgumentNullException(id);

                // Make the request.
                return service.Users.Drafts.Update(body, userId, id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Drafts.Update failed.", ex);
            }
        }

    }

    public class GoogleMailExtend
    {
        /// <summary>
        /// Authenticating to Google using a Service account
        /// Documentation: https://developers.google.com/accounts/docs/OAuth2#serviceaccount
        /// </summary>
        /// <param name="serviceAccountEmail">From Google Developer console https://console.developers.google.com</param>
        /// <param name="serviceAccountCredentialFilePath">Location of the .p12 or Json Service account key file downloaded from Google Developer console https://console.developers.google.com</param>
        /// <returns>AnalyticsService used to make requests against the Analytics API</returns>
        public static GmailService AuthenticateServiceAccount(string serviceAccountEmail, string serviceAccountCredentialFilePath, string _ApplicationName = "GmailService Oauth2")
        {
            try
            {
                if (string.IsNullOrEmpty(serviceAccountCredentialFilePath))
                    throw new Exception("Path to the service account credentials file is required.");
                if (!File.Exists(serviceAccountCredentialFilePath))
                    throw new Exception("The service account credentials file does not exist at: " + serviceAccountCredentialFilePath);
                if (string.IsNullOrEmpty(serviceAccountEmail))
                    throw new Exception("ServiceAccountEmail is required.");

                // These are the scopes of permissions you need. It is best to request only what you need and not all of them
                string[] scopes = new string[] { AnalyticsReportingService.Scope.Analytics };             // View your Google Analytics data

                // For Json file
                if (Path.GetExtension(serviceAccountCredentialFilePath).ToLower() == ".json")
                {
                    GoogleCredential credential;
                    using (var stream = new FileStream(serviceAccountCredentialFilePath, FileMode.Open, FileAccess.Read))
                    {
                        credential = GoogleCredential.FromStream(stream)
                             .CreateScoped(scopes);
                    }

                    // Create the  Analytics service.
                    return new GmailService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = _ApplicationName,
                    });
                }
                else if (Path.GetExtension(serviceAccountCredentialFilePath).ToLower() == ".p12")
                {   // If its a P12 file

                    var certificate = new X509Certificate2(serviceAccountCredentialFilePath, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
                    var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        Scopes = scopes
                    }.FromCertificate(certificate));

                    // Create the  Gmail service.
                    return new GmailService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = _ApplicationName,
                    });
                }
                else
                {
                    throw new Exception("Unsupported Service accounts credentials.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Create service account GmailService failed" + ex.Message);
                throw new Exception("CreateServiceFailed", ex);
            }
        }

        /// <summary>
        /// List all Messages of the user's mailbox matching the query.
        /// </summary>
        /// <param name="service">Gmail API service instance.</param>
        /// <param name="userId">User's email address. The special value "me"
        /// can be used to indicate the authenticated user.</param>
        /// <param name="query">String used to filter Messages returned.</param>
        public static List<Message> ListMessages(GmailService service, String userId, String query)
        {
            List<Message> result = new List<Message>();
            UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List(userId);
            request.Q = query;

            do
            {
                try
                {
                    ListMessagesResponse response = request.Execute();
                    result.AddRange(response.Messages);
                    request.PageToken = response.NextPageToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
            } while (!String.IsNullOrEmpty(request.PageToken));

            return result;
        }

    }

    public class GoogleMailHelpers
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
                if (piShared != null) {
                    try
                    {
                        piShared.SetValue(request, property.GetValue(optional, null), null);
                    } catch (ArgumentException ae)
                    {
                        Console.WriteLine(ae.Message);

                    } catch (System.Reflection.TargetException te)
                    {
                        Console.WriteLine(te.Message);

                    } catch (System.Reflection.TargetInvocationException tie)
                    {
                        Console.WriteLine(tie.Message);

                    } catch (System.Reflection.TargetParameterCountException tpce)
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
