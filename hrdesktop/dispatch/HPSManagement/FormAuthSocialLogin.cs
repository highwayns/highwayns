using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Brickred.SocialAuth.NET.Core;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using NC.HPS.Lib;

namespace HPSManagement
{
    public partial class FormAuthSocialLogin : Form
    {
        private PROVIDER_TYPE provider_type;
        private SocialAuthManager manager;
        private CmWinServiceAPI db;
        /// <summary>
        /// Login to Auth Social
        /// </summary>
        /// <param name="provider"></param>
        public FormAuthSocialLogin(CmWinServiceAPI db,PROVIDER_TYPE provider_type, SocialAuthManager manager)
        {
            this.db = db;
            this.provider_type = provider_type;
            this.manager = manager;
            InitializeComponent();
        }
        /// <summary>
        /// FormAuthSocialLogin_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormAuthSocialLogin_Load(object sender, EventArgs e)
        {
            string loginUrl = manager.GetLoginRedirectUrl(provider_type, "");
            webBrowser1.Navigate(loginUrl);
        }
        /// <summary>
        /// webBrowser1_Navigated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //get webBrowser1
            string connectUrl = e.Url.ToString();
            if (manager.Connect(provider_type, connectUrl))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("获取acesskey出错");
            }

        }
    }
}
