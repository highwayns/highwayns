using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using HPSWizard;
using NC.HPS.Lib;
using System.IO;
using QWeiboSDK;

namespace HPSManagement.PublishMagzine
{
	public partial class WizardPage42 : HPSWizard.WizardPage
	{
        private CmWinServiceAPI db;
        private string configpath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "qq.ini");
        private const string APPKEY = "801444133";
        private const string APPSECRET = "20439d14a62473155f269cfbcfa87dae";

        private string appKey = null;
        private string appSecret = null;
        private string accessKey = null;
        private string accessSecret = null;

        private string OauthVerify = null;
        private string tokenKey = null;
        private string tokenSecret = null;
        //--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor that assumes the page type is "intermediate"
		/// </summary>
		/// <param name="parent">The parent WizardFormBase-derived form</param>
        public WizardPage42(WizardFormBase parent, CmWinServiceAPI db) 
					: base(parent)
		{
            this.db = db;
			InitPage();
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor that allows the programmer to specify the page type. In 
		/// the case of the sample app, we use this constructor for this oject, 
		/// and specify it as the start page.
		/// </summary>
		/// <param name="parent">The parent WizardFormBase-derived form</param>
		/// <param name="pageType">The type of page this object represents (start, intermediate, or stop)</param>
        public WizardPage42(WizardFormBase parent, WizardPageType pageType, CmWinServiceAPI db) 
					: base(parent, pageType)
		{
            this.db = db;
			InitPage();
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// This method serves as a common constructor initialization location, 
		/// and serves mainly to set the desired size of the container panel in 
		/// the wizard form (see WizardFormBase for more info).  I didn't want 
		/// to do this here but it was the only way I could get the form to 
		/// resize itself appropriately - it needed to size itself according 
		/// to the size of the largest wizard page.
		/// </summary>
		public void InitPage()
		{
			InitializeComponent();
			base.Size = this.Size;
			this.ParentWizardForm.DiscoverPagePanelSize(this.Size);
		}
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, System.Text.StringBuilder lpReturnedString, int nSize, string lpFileName);

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

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);

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

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Overriden method that allows this wizard page to save page-specific data.
		/// </summary>
		/// <returns>True if the data was saved successfully</returns>
		public override bool SaveData()
		{
			return true;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// This is an overriden method that performs special processing when 
		/// it's time to select the next page to display. In the case of our 
		/// Page 1, a different "next" page is displayed depending on which 
		/// radio button has been selected.
		/// </summary>
		/// <returns>The new current page that we will show</returns>
		public override WizardPage GetNextPage()
		{
            NextPages[0].PageData["MagId"] = PageData["MagId"].ToString();
            NextPages[0].PageData["SMagId"] = PageData["SMagId"].ToString();
            NextPages[0].PageData["MagNo"] = PageData["MagNo"].ToString();
            NextPages[0].PageData["MediaType"] = PageData["MediaType"].ToString();
            NextPages[0].PageData["QQWeiboAccessKey"] = accessKey;
            NextPages[0].PageData["QQWeiboAccessSecret"] = accessSecret;
            NextPages[0].PageData["QQWeiboAppKey"] = appKey;
            NextPages[0].PageData["QQWeiboAppSecret"] = appSecret;
            return NextPages[0];
		}
        /// <summary>
        /// 成功登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (webBrowser1.DocumentText.IndexOf("oauth_verifier") > -1)
            {
                if (accessKey == "" || accessSecret == "")
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

                ButtonStateNext |= WizardButtonState.Enabled;
                ParentWizardForm.UpdateWizardForm(this);
            }
            else
            {
                ButtonStateNext = WizardButtonState.Visible;
            }
        }
        /// <summary>
        /// WizardPage42_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WizardPage42_Load(object sender, EventArgs e)
        {
            //取得RequestToken
            if (GetRequestToken(APPKEY, APPSECRET) == false)
            {
                MessageBox.Show("获取token key出错");
                return;
            }
            ///根据RequestTokenkey打开页面
            webBrowser1.Navigate("http://open.t.qq.com/cgi-bin/authorize?oauth_token=" + tokenKey);
            appKey = APPKEY;
            appSecret = APPSECRET;

        }

	}
}
