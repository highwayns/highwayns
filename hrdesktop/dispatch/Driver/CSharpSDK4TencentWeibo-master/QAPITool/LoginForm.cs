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


namespace QAPITool
{
    public partial class LoginForm : Form
    {
        private string configpath = System.Environment.CurrentDirectory + "\\QAPITool.ini";

        private string appKey = null;
        private string appSecret = null;
        private string accessKey = null;
        private string accessSecret = null;

        private string OauthVerify = null;
        private string tokenKey = null;
        private string tokenSecret = null;


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

            textAppKey.Text = appKey;//界面显示
            textAppSecret.Text = appSecret;

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

        private void buttonOK_Click(object sender, EventArgs e)
        {
            /*1.授权认证*/
            /*2.保存配置文件*/

            if (string.IsNullOrEmpty(textAppKey.Text))
            {
                MessageBox.Show("AppKey不能为空");
                return;
            }

            if(string.IsNullOrEmpty(textAppSecret.Text))
            {
                MessageBox.Show("AppSecret不能为空");
                return;
            }

            if(textAppKey.Text != appKey || textAppSecret.Text != appSecret)
            {
                accessKey = "";
                accessSecret = "";
                if (GetRequestToken(textAppKey.Text, textAppSecret.Text) == false)
                {
                    MessageBox.Show("获取token key出错");
                    return;
                }

                System.Diagnostics.Process.Start("http://open.t.qq.com/cgi-bin/authorize?oauth_token=" + tokenKey);
            } 
            else if(accessKey =="" ||accessSecret == "")
            {
                if (GetRequestToken(textAppKey.Text, textAppSecret.Text) == false)
                {
                    MessageBox.Show("获取token key出错");
                    return;
                }
                System.Diagnostics.Process.Start("http://open.t.qq.com/cgi-bin/authorize?oauth_token=" + tokenKey);
            }
            appKey = textAppKey.Text;
            appSecret = textAppSecret.Text;

            if (accessKey == "" || accessSecret == "")
            {
                InputForm inputdlg = new InputForm();
                inputdlg.ShowDialog();
                if(inputdlg.Comfirm == false)
                {
                    return;
                }
                OauthVerify = inputdlg.Input;

                if(GetAccessToken(appKey,appSecret,tokenKey,tokenSecret,OauthVerify) == false)
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            comfirm = false;
            Close();
        }

        public LoginForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            ReadConfig();
        }

    }
}