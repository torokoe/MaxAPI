using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Json;
using Google.Apis.Download;
using Google.Apis.Auth;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.Threading;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.PeopleService.v1;
using Google.Apis.Gmail.v1;

using GApis.OAuth;
using GApis.GooglePeople;
using GApis.GoogleDrive;
using GApis.OApis.GoogleMail;
using GApis.OApis.GoogleUrlShortener;
using Google.Apis.Urlshortener.v1;
using Newtonsoft.Json;

namespace GApis
{
    public partial class GoogleR : Form
    {
        public GoogleR()
        {
            InitializeComponent();
        }

        private UserCredential Ucredential;

        private void btnOAuth_Click(object sender, EventArgs e)
        {

            GoogleWebAuthorizationBroker.Folder = "Tasks.Auth.Store";

            OAuth2Services.Auth2Service = OAuth2Services.Auth2Services.FaceBook;

            EmbeddedBrowserCodeReceiver ecr = new EmbeddedBrowserCodeReceiver();
            try
            {
                string[] Scope = new string[] {
                    "email",
                    "public_profile",
                    "user_friends",
                   // BooksService.Scope.Books,
                   // DriveService.Scope.Drive,
                   //PlusService.Scope.PlusMe,
                   // DriveService.Scope.DriveReadonly,
                   // PeopleServiceService.Scope.UserPhonenumbersRead,	//View phone numbers
                   // PeopleServiceService.Scope.UserAddressesRead,   	//View street addresses
                   // PeopleServiceService.Scope.UserBirthdayRead,    	//View complete date of birth
                   // PeopleServiceService.Scope.ContactsReadonly,     	//View contacts
                   // PeopleServiceService.Scope.UserEmailsRead,      	//View email addresses
                   // PeopleServiceService.Scope.UserinfoProfile,      	//View basic profile info
                   // PeopleServiceService.Scope.UserinfoEmail,        	//View email address
                   // PeopleServiceService.Scope.PlusLogin,            	//Know basic profile info and list of people in your circles.
                   // PeopleServiceService.Scope.Contacts,                 //for list
                   // GmailService.Scope.MailGoogleCom,
                   // GmailService.Scope.GmailSettingsBasic,
                   // UrlshortenerService.Scope.Urlshortener,
                };

                Ucredential = WebAuthorizationBroker_Ex.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = "{REMOVED}",
                        ClientSecret = "{REMOVED}"
                    },
                    Scope,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("Blogger.Auth.POS"),
                    ecr
                ).Result;
            }catch(Exception ex)
            {
                return;
            }

            txtFileName.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Blogger.Auth.POS\Google.Apis.Auth.OAuth2.Responses.TokenResponse-user");
            txtAccessToken.Text = Ucredential.Token.AccessToken;
            txtRefreshToken.Text = Ucredential.Token.RefreshToken;
            txtIssued.Text = Ucredential.Token.Issued.ToString();
            txtExpiresIn.Text = Ucredential.Token.ExpiresInSeconds.ToString();


            txtpeopleemail.Text = "";
            return;
            #region GoogleDrive

            var Drive_service = GoogleDriveStructure.AuthenticateOauth(Ucredential);

            var allFiles = GoogleDriveStructure.ListAll(Drive_service, new GoogleDriveStructure.FilesListOptionalParms { Q = "('root' in parents)", PageSize = 1000 });
            //var sFilesname = GoogleDriveStructure.ListAll(service, new GoogleDriveStructure.FilesListOptionalParms { Q = "name contains 'Make'", PageSize = 1000 });
            //var sfloder = GoogleDriveStructure.ListAll(service, new GoogleDriveStructure.FilesListOptionalParms { Q = "mimeType = 'application/vnd.google-apps.folder'", PageSize = 1000 });
            //var sshare = GoogleDriveStructure.ListAll(service, new GoogleDriveStructure.FilesListOptionalParms { Q = "(sharedWithMe = true)", PageSize = 1000 });

            //var request = service.Files.List();
            //request.Q = "(sharedWithMe = true)";
            //var results = request.Execute();
            //var myfile = results.Files.Where(a => a.Name.ToLower().Equals("receipt.pdf")).FirstOrDefault();
            //var per = myfile.Permissions.Where(a => a.EmailAddress.ToLower().Equals("laurly71@gmail.com")).FirstOrDefault();
            //service.Permissions.Delete(myfile.Id, per.Id).Execute();

            List<string> lst = new List<string>();
            GoogleDriveExtend.FileList_PrettyPrint(Drive_service, allFiles, "", ref lst);
            listBox1.Items.AddRange(lst.ToArray());

            #endregion

            #region People
            var People_service = GooglePeopleStructure.AuthenticateOauth(Ucredential);

            //PersonFields
            //addresses,ageRanges,biographies,birthdays,braggingRights,coverPhotos,emailAddresses,events,genders
            //imClients,interests,locales,memberships,metadata,names,nicknames,occupations,organizations,phoneNumbers
            //photos,relations,relationshipInterests,relationshipStatuses,residences,skills,taglines,urls
            var people = GooglePeopleStructure.GetPerson(People_service, "people/me",new GooglePeopleStructure.GetPersonOptionalParms { PersonFields = "emailAddresses" });
            var people_list = GooglePeopleStructure.List(People_service, "people/me", new GooglePeopleStructure.ConnectionsListOptionalParms { PersonFields = "emailAddresses" });

            //Contacts
            if (people_list != null)
            {
                txtpeopleCount.Text = people_list.TotalItems.Value.ToString();
                if (people_list.Connections[0].EmailAddresses.Count > 0)
                {
                    string e0 = people_list.Connections[0].EmailAddresses.Last().Value;
                }
            }
            //Profile
            txtpeopleemail.Text = people.EmailAddresses.Last().Value;

            #endregion

            #region GMail
            var GMail_service = GoogleMailStructure.AuthenticateOauth(Ucredential);

            //label list
            listBox2.Items.AddRange(GoogleMailLabelsStructure.List(GMail_service, people.EmailAddresses.Last().Value).Labels.Select(i => i.Name).ToArray());
            //tmp label mails
            var maillist = GoogleMailMessagesStructure.List(GMail_service, people.EmailAddresses.Last().Value, new GoogleMailStructure.MessagesListOptionalParms { Q = "label:tmp" , maxResults = 10000});
            txtMailCount.Text = maillist.Messages.Count.ToString();
            var lastmailbody = GoogleMailMessagesStructure.Get(GMail_service, people.EmailAddresses.Last().Value, maillist.Messages.FirstOrDefault().Id,new GoogleMailStructure.MessagesGetOptionalParms {  Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Raw} );
            txtfirstmailtitle.Text = lastmailbody.Snippet;
            lastmailbody = GoogleMailMessagesStructure.Get(GMail_service, people.EmailAddresses.Last().Value, maillist.Messages.FirstOrDefault().Id, new GoogleMailStructure.MessagesGetOptionalParms { Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Metadata });
            txtfirstmailtitle.Text += Environment.NewLine + "---------------------------"+ Environment.NewLine + lastmailbody.Payload.Headers.Where(i => i.Name == "Subject").Last().Value;
            lastmailbody = GoogleMailMessagesStructure.Get(GMail_service, people.EmailAddresses.Last().Value, maillist.Messages.FirstOrDefault().Id, new GoogleMailStructure.MessagesGetOptionalParms { Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full });
            txtfirstmailtitle.Text += Environment.NewLine + "---------------------------" + Environment.NewLine + lastmailbody.Id;

            #endregion

        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false; 
            checkBox1.Checked = Ucredential.RefreshTokenAsync(CancellationToken.None).Result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var url_service = GoogleUrlShortenerStructure.AuthenticateOauth(Ucredential);

            //Must apikey
            var shorturl = GoogleUrlShortenerStructure.Insert(url_service, new Google.Apis.Urlshortener.v1.Data.Url {  LongUrl = txtlongurl.Text}, "AIzaSyCMrhR9Np2tcXFzu6UOTrMglP-q_-oko98");
            if (shorturl != null)
            {
                txtshorturl.Text = shorturl.Id;
            }
        }

        public class JSo
        {
            [JsonProperty("iiiiiiiiiD")]
            public int id { get; set; }
            public string txt { get; set; }
            public object a { get; set; }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            List<JSo> j = new List<JSo>();
            JSo s = new JSo();
            s.id = 99999;
            s.txt = textBox1.Text;
            TextBox t = new TextBox();
            s.a = t.Text;
            j.Add(s);

            string output = JsonConvert.SerializeObject(j);
            textBox2.Text = output;
        }
    }
}
