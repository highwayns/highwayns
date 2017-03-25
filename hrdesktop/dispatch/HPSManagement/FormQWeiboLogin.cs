using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using QWeiboSDK;
using NC.HPS.Lib;

namespace HPSManagement
{
    public partial class FormQWeiboLogin : Form
    {
        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, System.Text.StringBuilder lpReturnedString, int nSize, string lpFileName);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);

        private bool submited;
        private string configpath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "qq.ini");
        private string APPKEY = "801444133";
        private string APPSECRET = "20439d14a62473155f269cfbcfa87dae";

        private string appKey = null;
        private string appSecret = null;
        private string accessKey = null;
        private string accessSecret = null;

        private string OauthVerify = null;
        private string tokenKey = null;
        private string tokenSecret = null;

        private string userName=null;
        private string password=null;
        private CmWinServiceAPI db;
        /// <summary>
        /// 登陆QWeiboLogin
        /// </summary>
        public FormQWeiboLogin(string appkey,string appsecret,string userName,string password)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.APPKEY = appkey;
            this.APPSECRET = appsecret;
            this.userName = userName;
            this.password = password;
        }
        /// <summary>
        /// 登陆QWeiboLogin
        /// </summary>
        public FormQWeiboLogin(CmWinServiceAPI db)
        {
            InitializeComponent();
            this.db = db;
            this.StartPosition = FormStartPosition.CenterScreen;
            init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (db.GetMedia(0, 0, "id,MediaType,MediaURL,MediaAppKey,MediaAppPassword,MediaUser,MediaPassword,other,createtime,published,UserID", "MediaType='TENCENT'", "", ref ds))
            {
                this.APPKEY = ds.Tables[0].Rows[0]["MediaAppKey"].ToString();
                this.APPSECRET = ds.Tables[0].Rows[0]["MediaAppPassword"].ToString();
                this.userName = ds.Tables[0].Rows[0]["MediaUser"].ToString();
                this.password = ds.Tables[0].Rows[0]["MediaPassword"].ToString();
            }

        }

        private bool comfirm = false;

        public string AppKey
        {
            get { return appKey; }
        }

        public string AppSecret
        {
            get { return appSecret; }
        }

        public string AccessKey
        {
            get { return accessKey; }
        }

        public string AccessSecret
        {
            get { return accessSecret; }
        }

        public bool Comfirm
        {
            get { return comfirm; }
        }


       
        private void ReadConfig()
        {
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString("Config", "AppKey", "", sb, sb.Capacity, configpath);
            appKey = sb.ToString();

            GetPrivateProfileString("Config", "AppSecret", "", sb, sb.Capacity, configpath);
            appSecret = sb.ToString();

            GetPrivateProfileString("Config", "AccessKey", "", sb, sb.Capacity, configpath);
            accessKey = sb.ToString();

            GetPrivateProfileString("Config", "AccessSecret", "", sb, sb.Capacity, configpath);
            accessSecret = sb.ToString();

        }


        private void WriteConfig()
        {
            WritePrivateProfileString("Config", "AppKey", appKey, configpath);
            WritePrivateProfileString("Config", "AppSecret", appSecret, configpath);
            WritePrivateProfileString("Config", "AccessKey", accessKey, configpath);
            WritePrivateProfileString("Config", "AccessSecret", accessSecret, configpath);
        }
        /// <summary>
        /// 取得RequestToken
        /// </summary>
        /// <param name="customKey"></param>
        /// <param name="customSecret"></param>
        /// <returns></returns>
        private bool GetRequestToken(string customKey, string customSecret)
        {
            string url = "https://open.t.qq.com/cgi-bin/request_token";
            List<Parameter> parameters = new List<Parameter>();
            OauthKey oauthKey = new OauthKey();
            oauthKey.customKey = customKey;
            oauthKey.customSecret = customSecret;
            oauthKey.callbackUrl = "http://www.qq.com";
            QWeiboRequest request = new QWeiboRequest();
            return ParseToken(request.SyncRequest(url, "GET", oauthKey, parameters, null));
        }
        /// <summary>
        /// 取得AccessToken
        /// </summary>
        /// <param name="customKey"></param>
        /// <param name="customSecret"></param>
        /// <param name="requestToken"></param>
        /// <param name="requestTokenSecrect"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        private bool GetAccessToken(string customKey, string customSecret, string requestToken, string requestTokenSecrect, string verify)
        {
            string url = "https://open.t.qq.com/cgi-bin/access_token";
            List<Parameter> parameters = new List<Parameter>();
            OauthKey oauthKey = new OauthKey();
            oauthKey.customKey = customKey;
            oauthKey.customSecret = customSecret;
            oauthKey.tokenKey = requestToken;
            oauthKey.tokenSecret = requestTokenSecrect;
            oauthKey.verify = verify;
            QWeiboRequest request = new QWeiboRequest();
            return ParseToken(request.SyncRequest(url, "GET", oauthKey, parameters, null));
        }
        /// <summary>
        /// 解析TOKEN
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool ParseToken(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                return false;
            }

            string[] tokenArray = response.Split('&');

            if (tokenArray.Length < 2)
            {
                return false;
            }

            string strTokenKey = tokenArray[0];
            string strTokenSecrect = tokenArray[1];

            string[] token1 = strTokenKey.Split('=');
            if (token1.Length < 2)
            {
                return false;
            }
            tokenKey = token1[1];

            string[] token2 = strTokenSecrect.Split('=');
            if (token2.Length < 2)
            {
                return false;
            }
            tokenSecret = token2[1];

            return true;
        }
        ///// <summary>
        ///// 登陆QWeiboLogin
        ///// </summary>
        //public FormQWeiboLogin()
        //{
        //    InitializeComponent();
        //    this.StartPosition = FormStartPosition.CenterScreen;
        //    ReadConfig();
        //}
        /// <summary>
        /// 网页移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //"http://www.qq.com/?oauth_token=9ad46e17a4e84d8fab07db42426d0585&oauth_verifier=641446&openid=2452A9907B8F73AD9FE369F6478CF1BB&openkey=FCAD28B1E4B248CB8DAD3867DDF8FAC1"
            if (webBrowser1.DocumentText.IndexOf("oauth_verifier") > -1)
            {
                if (string.IsNullOrEmpty( accessKey) || string.IsNullOrEmpty( accessSecret))
                {
                    //取得OauthVerify
                    int startIndex = webBrowser1.DocumentText.IndexOf("oauth_verifier");
                    OauthVerify = webBrowser1.DocumentText.Substring(startIndex + 15, 6);
                    //取得AccessToken
                    if (GetAccessToken(appKey, appSecret, tokenKey, tokenSecret, OauthVerify) == false)
                    {
                        MessageBox.Show("获取acesskey出错");
                        return;
                    }

                    accessKey = tokenKey;
                    accessSecret = tokenSecret;
                }

                WriteConfig();

                comfirm = true;
                Close();
            }

        }
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormQWeiboLogin_Load(object sender, EventArgs e)
        {
            //取得RequestToken
            if (GetRequestToken(APPKEY, APPSECRET) == false)
            {
                MessageBox.Show("获取token key出错");
                return;
            }
            //清空cookies
            ShellExecute(IntPtr.Zero, "open", "rundll32.exe", " InetCpl.cpl,ClearMyTracksByProcess 2", "", 0);

            ///根据RequestTokenkey打开页面
            webBrowser1.Navigate("http://open.t.qq.com/cgi-bin/authorize?oauth_token=" + tokenKey);
            appKey = APPKEY;
            appSecret = APPSECRET;

        }
        /// <summary>
        /// 用户和密码输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            timer1.Start();           
        }
        /// <summary>
        /// ユーザとパスワード登録
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            HtmlElement user = webBrowser1.Document.GetElementById("u");
            if (user != null)
                user.SetAttribute("value", userName);

            HtmlElement pass = webBrowser1.Document.GetElementById("p");
            if (pass != null)
                pass.SetAttribute("value", password);

        }

    }
}