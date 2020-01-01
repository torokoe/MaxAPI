using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GApis.GoogleDrive
{

    public class GoogleDriveStructure
    {
        #region Parameters

        /// Lists or searches files. 
        /// <summary>Documentation: https://developers.google.com/drive/v3/reference/files/list
        /// </summary>
        public class FilesListOptionalParms
        {
            /// <summary>
            /// The source of files to list. Deprecated: use 'corpora' instead. 
            /// <para>Acceptable values are:</para>
            /// <para>"domain": Files shared to the user's domain.</para>
            /// <para>"user": Files owned by or shared to the user.</para>
            /// </summary>
            public string Corpus { get; set; }

            /// <summary>
            /// A comma-separated list of sort keys.
            /// <para>Valid keys are 'createdTime', 'folder', 'modifiedByMeTime', 'modifiedTime', 'name', 'quotaBytesUsed', 'recency', 'sharedWithMeTime', 'starred', and 'viewedByMeTime'.</para>
            /// <para>Each key sorts ascending by default, but may be reversed with the 'desc' modifier.</para>
            /// <para>Example usage: ?orderBy=folder,modifiedTime desc,name.</para>
            /// <para>Please note that there is a current limitation for users with approximately one million files in which the requested sort order is ignored.</para>
            /// </summary>
            public string OrderBy { get; set; }

            /// <summary>
            /// The maximum number of files to return per page.
            /// </summary>
            public int PageSize { get; set; }

            /// <summary>
            /// The token for continuing a previous list request on the next page. This should be set to the value of 'nextPageToken' from the previous response.
            /// </summary>
            public string PageToken { get; set; }

            /// <summary>
            /// [Required] A query for filtering the file results. See the "Search for Files" guide for supported syntax.
            /// </summary>
            public string Q { get; set; }

            /// <summary>
            /// A comma-separated list of spaces to query within the corpus. Supported values are 'drive', 'appDataFolder' and 'photos'.
            /// </summary>
            public string Spaces { get; set; }

            /// <summary>
            /// Selector specifying a subset of fields to include in the response.
            /// </summary>
            public string fields { get; set; }

            /// <summary>
            /// Alternative to userIp.
            /// </summary>
            public string quotaUser { get; set; }

            /// <summary>
            /// IP address of the end user for whom the API call is being made.
            /// </summary>
            public string userIp { get; set; }

            /// <summary>
            /// Whether Team Drive items should be included in results. (Default: false)
            /// </summary>
            public bool includeTeamDriveItems { get; set; }

            /// <summary>
            /// Whether the requesting application supports Team Drives. (Default: false)
            /// </summary>
            public bool supportsTeamDrives { get; set; }

            /// <summary>
            /// ID of Team Drive to search.
            /// </summary>
            public string teamDriveId { get; set; }
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
        public static DriveService AuthenticateOauth(string clientId, string clientSecret, string userName, string _ApplicationName = "Drive Oauth2")
        {
            try
            {
                if (string.IsNullOrEmpty(clientId))
                    throw new ArgumentNullException("clientId");
                if (string.IsNullOrEmpty(clientSecret))
                    throw new ArgumentNullException("clientSecret");
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");

                string[] scopes = new string[] { DriveService.Scope.DriveReadonly };

                var credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = System.IO.Path.Combine(credPath, ".credentials/apiName");

                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                                             , scopes
                                                                                             , userName
                                                                                             , CancellationToken.None
                                                                                             , new FileDataStore(credPath, true)).Result;
                return new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account DriveService failed" + ex.Message);
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
        public static DriveService AuthenticateOauth(UserCredential _credential, string _ApplicationName = "Drive Oauth2")
        {
            try
            {
                return new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = _credential,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account DriveService failed" + ex.Message);
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
        public static DriveService AuthenticateOauth(string _ApiKey, string _ApplicationName = "Drive Oauth2")
        {
            try
            {
                return new DriveService(new BaseClientService.Initializer()
                {
                    ApiKey = _ApiKey,
                    ApplicationName = _ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account DriveService failed" + ex.Message);
                throw new Exception("CreateServiceFailed", ex);
            }
        }

        /// <summary>
        /// Lists or searches files.
        /// Documentation: https://developers.google.com/drive/v3/reference/files/list
        /// Generation Note: This does not always build correctly.  Google needs to standardize things I need to figure out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Drive service. </param>
        /// <param name="optional">The optional parameters. </param>
        /// <returns>FileListResponse</returns>
        public static Google.Apis.Drive.v3.Data.FileList ListAll(DriveService service, FilesListOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");

                // Building the initial request.
                var request = service.Files.List();

                // Applying optional parameters to the request.
                request = (FilesResource.ListRequest)GoogleDriveHelpers.ApplyOptionalParms(request, optional);

                var pageStreamer = new Google.Apis.Requests.PageStreamer<Google.Apis.Drive.v3.Data.File, FilesResource.ListRequest, Google.Apis.Drive.v3.Data.FileList, string>(
                                                   (req, token) => request.PageToken = token,
                                                   response => response.NextPageToken,
                                                   response => response.Files);

                var allFiles = new Google.Apis.Drive.v3.Data.FileList();
                allFiles.Files = new List<Google.Apis.Drive.v3.Data.File>();

                foreach (var result in pageStreamer.Fetch(request))
                {
                    allFiles.Files.Add(result);
                }

                return allFiles;
            }
            catch (Exception Ex)
            {
                throw new Exception("Request Files.List failed.", Ex);
            }
        }
    }

    public class GoogleDriveExtend
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="list"></param>
        /// <param name="indent"></param>
        /// <param name="_lst"></param>
        /// <returns></returns>
        public static List<string> FileList_PrettyPrint(DriveService service, Google.Apis.Drive.v3.Data.FileList list, string indent, ref List<string> _lst)
        {
            foreach (var item in list.Files.OrderBy(a => a.Name))
            {
                _lst.Add(string.Format("{0}|-{1}", indent, item.Name));

                if (item.MimeType == "application/vnd.google-apps.folder")
                {
                    var ChildrenFiles = GoogleDriveStructure.ListAll(service, new GoogleDriveStructure.FilesListOptionalParms { Q = string.Format("('{0}' in parents)", item.Id), PageSize = 1000 });
                    FileList_PrettyPrint(service, ChildrenFiles, indent + "  ", ref _lst);
                }
            }
            return _lst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="file"></param>
        /// <param name="saveTo"></param>
        public static void DownloadFile(Google.Apis.Drive.v3.DriveService service, Google.Apis.Drive.v3.Data.File file, string saveTo)
        {

            var request = service.Files.Get(file.Id);
            var stream = new System.IO.MemoryStream();

            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download complete.");
                            GoogleDriveHelpers.SaveStream(stream, saveTo);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream);
        }

        /// <summary>
        /// Insert a new permission.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <param name="fileId">ID of the file to insert permission for.</param>
        /// <param name="who">
        /// User or group e-mail address, domain name or null for "default" type.
        /// </param>
        /// <param name="type">The value "user", "group", "domain" or "default".</param>
        /// <param name="role">The value "owner", "writer" or "reader".</param>
        /// <returns>The inserted permission, null is returned if an API error occurred</returns>
        public static Permission CreatePermission(DriveService service, String fileId, String who, String type, String role)
        {
            Permission newPermission = new Permission();
            newPermission.Type = type;
            newPermission.Role = role;
            try
            {
                return service.Permissions.Create(newPermission, fileId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return null;
        }
    }

    public class GoogleDriveHelpers
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
                    piShared.SetValue(request, property.GetValue(optional, null), null);
            }

            return request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="saveTo"></param>
        public static void SaveStream(System.IO.MemoryStream stream, string saveTo)
        {
            using (System.IO.FileStream file = new System.IO.FileStream(saveTo, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }
    }

}
