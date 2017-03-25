using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;
using MySpaceID.SDK.Api;
using MySpaceID.SDK.OAuth.Tokens;
using MySpaceID.SDK.Context;

namespace HPSManagement
{
    public partial class FormMySpaceLogin : Form
    {
        private RestV1 service = null;
        OAuthToken requestToken = null;
        private CmWinServiceAPI db;
        /// <summary>
        /// Login to Auth Social
        /// </summary>
        /// <param name="provider"></param>
        public FormMySpaceLogin(CmWinServiceAPI db, RestV1 service)
        {
            this.db = db;
            this.service = service;
            InitializeComponent();
        }
        /// <summary>
        /// FormAuthSocialLogin_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormAuthSocialLogin_Load(object sender, EventArgs e)
        {
            requestToken = ((OffsiteContext)service.Context).GetRequestToken(null);
            if (requestToken != null)
            {
                var uri = ((OffsiteContext)service.Context).GetAuthorizationUrl(requestToken, null);
                webBrowser1.Navigate(uri.ToString());
            }
        }
        /// <summary>
        /// webBrowser1_Navigated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (webBrowser1.DocumentText.IndexOf("oauth_verifier") > -1)
            {
                //取得OauthVerify
                int startIndex = webBrowser1.DocumentText.IndexOf("oauth_verifier");
                int endIndex=webBrowser1.DocumentText.IndexOf('\"',startIndex);
                string verifier = webBrowser1.DocumentText.Substring(startIndex + 15, endIndex - startIndex - 15);

                OAuthToken accessToken = ((OffsiteContext)service.Context).GetAccessToken(requestToken.TokenKey, requestToken.TokenSecret, verifier);
                if (accessToken != null)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }
    }
}
