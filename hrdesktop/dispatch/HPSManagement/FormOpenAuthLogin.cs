using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.CJW.Lib;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.ApplicationBlock;

namespace CJWManagement
{
    public partial class FormOpenAuthLogin : Form
    {
        private WebServerClient client;
        private CmWinServiceAPI db;
        /// <summary>
        /// Login to Auth Social
        /// </summary>
        /// <param name="provider"></param>
        public FormOpenAuthLogin(CmWinServiceAPI db, WebServerClient client)
        {
            this.db = db;
            this.client = client;
            InitializeComponent();
        }
        /// <summary>
        /// FormAuthSocialLogin_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormAuthSocialLogin_Load(object sender, EventArgs e)
        {
            string loginUrl = client.AuthorizationServer.AuthorizationEndpoint.ToString();
            webBrowser1.Navigate(loginUrl);
        }
        /// <summary>
        /// webBrowser1_Navigated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            client.ClientIdentifier = "";
            client.PrepareRequestUserAuthorization();
            //{
            //    DialogResult = DialogResult.OK;
            //    Close();
            //}
            //else
            //{
            //    MessageBox.Show("获取acesskey出错");
            //}

        }
    }
}
