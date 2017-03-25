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
using System.Web;

namespace HPSManagement
{
    public partial class FormLinkedInLogin : Form
    {
        private OAuth1 _oauth;
        private String _token;
        private String _verifier;
        private String _tokenSecret;
        private string userName;
        private string password;
        public String Token
        {
            get
            {
                return _token;
            }
        }

        public String Verifier
        {
            get
            {
                return _verifier;
            }
        }

        public String TokenSecret
        {
            get
            {
                return _tokenSecret;
            }
        }
        /// <summary>
        /// Login to Auth Social
        /// </summary>
        /// <param name="provider"></param>
        public FormLinkedInLogin(OAuth1 o)
        {
            _oauth = o;
            _token = null;
            InitializeComponent();
            _token = _oauth.Token;
            _tokenSecret = _oauth.TokenSecret;
            init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (_oauth.db.GetMedia(0, 0, "id,MediaType,MediaURL,MediaAppKey,MediaAppPassword,MediaUser,MediaPassword,other,createtime,published,UserID", "MediaType='LINKEDIN'", "", ref ds))
            {
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
            webBrowser1.Navigate(new Uri(_oauth.AuthorizationLink));
        }
        /// <summary>
        /// webBrowser1_Navigated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (webBrowser1.Url.ToString().Contains(_oauth.Settings_Redirect_URL))
            {
                string queryParams = e.Url.Query;
                if (queryParams.Length > 0)
                {
                    //Store the Token and Token Secret
                    System.Collections.Specialized.NameValueCollection qs = HttpUtility.ParseQueryString(queryParams);
                    if (qs["oauth_token"] != null)
                    {
                        _token = qs["oauth_token"];
                    }
                    if (qs["oauth_verifier"] != null)
                    {
                        _verifier = qs["oauth_verifier"];
                    }
                }
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
    }
}
