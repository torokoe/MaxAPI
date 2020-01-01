using GApis.OAuth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OApis
{
    public partial class AuthorizationDialog : Form
    {
        public AuthorizationCodeResponseUrl ResponseUrl { get; protected set; }

        public AuthorizationDialog(Uri authUri)
        {
            InitializeComponent();
            authWebBrowser.Navigate(authUri);
            authWebBrowser.DocumentTitleChanged += authWebBrowser_TitleChanged;
        }
        private void authWebBrowser_TitleChanged(object sender, EventArgs e) {
            var browser = (WebBrowser)sender;
            if (browser.Url.AbsolutePath == new Uri(GoogleAuthConsts.ApprovalUrl).AbsolutePath)
            {
                string query="";
                switch (OAuth2Services.Auth2Service)
                {
                    case OAuth2Services.Auth2Services.Google:
                        query = browser.DocumentTitle.Substring(browser.DocumentTitle.IndexOf(" ") + 1);
                        break;
                    case OAuth2Services.Auth2Services.FaceBook://SetPage = https://accounts.google.com/o/oauth2/approval
                        query = browser.Url.AbsoluteUri.Split('?')[1];
                        break;
                    case OAuth2Services.Auth2Services.Twitter:
                        break;
                    default:
                        break;
                }
                ResponseUrl = new AuthorizationCodeResponseUrl(query);
                this.Close();
            }else
            {
                System.Diagnostics.Trace.TraceInformation(browser.Url.AbsolutePath);
            }
        }

        protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
        }

    }
}
