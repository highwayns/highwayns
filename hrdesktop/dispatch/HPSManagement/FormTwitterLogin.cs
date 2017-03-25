using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;
using TweetSharp;

namespace HPSManagement
{
    public partial class FormTwitterLogin : Form
    {
        private TwitterService service = null;
        private CmWinServiceAPI db;
        private OAuthRequestToken requestToken;
        private string userName = null;
        private string password = null;
        /// <summary>
        /// Login to Auth Social
        /// </summary>
        /// <param name="provider"></param>
        public FormTwitterLogin(TwitterService service,string userName,string password)
        {
            this.service = service;
            this.userName = userName;
            this.password = password;
            InitializeComponent();
        }
        /// <summary>
        /// Login to Auth Social
        /// </summary>
        /// <param name="provider"></param>
        public FormTwitterLogin(CmWinServiceAPI db,TwitterService service)
        {
            this.db = db;
            this.service = service;
            InitializeComponent();
            init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (db.GetMedia(0, 0, "id,MediaType,MediaURL,MediaAppKey,MediaAppPassword,MediaUser,MediaPassword,other,createtime,published,UserID", "MediaType='TWITTER'", "", ref ds))
            {
                string appKey = ds.Tables[0].Rows[0]["MediaAppKey"].ToString();
                string appPassword = ds.Tables[0].Rows[0]["MediaAppPassword"].ToString();
                service = new TwitterService(appKey, appPassword);
                this.userName = ds.Tables[0].Rows[0]["MediaUser"].ToString();
                this.password = ds.Tables[0].Rows[0]["MediaPassword"].ToString();
            }

        }
        /// <summary>
        /// FormAuthSocialLogin_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormAuthSocialLogin_Load(object sender, EventArgs e)
        {
            requestToken = service.GetRequestToken();

            var uri = service.GetAuthorizationUri(requestToken);

            webBrowser1.Navigate(uri.ToString());
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

                OAuthAccessToken accessToken = service.GetAccessToken(requestToken, verifier);
                if (accessToken != null)
                {
                    service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }
    }
}
