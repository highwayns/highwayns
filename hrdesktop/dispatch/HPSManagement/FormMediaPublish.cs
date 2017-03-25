using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Facebook;
using NC.HPS.Lib;
using Brickred.SocialAuth.NET.Core;
using Brickred.SocialAuth.NET.Core.Wrappers;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using TweetSharp;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Api;
namespace HPSManagement
{
    public partial class FormMediaPublish : Form
    {
        private CmWinServiceAPI db;
        private SocialAuthManager manager;
        /// <summary>
        /// 媒体发布
        /// </summary>
        /// <param name="db"></param>
        public FormMediaPublish(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
            getManager();
        }
        /// <summary>
        /// 登陆Facebook
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            var fbLoginDlg = new FormFacebookLogin(db);
            fbLoginDlg.ShowDialog();
            if (fbLoginDlg.FacebookOAuthResult == null)
            {
                MessageBox.Show("Cancelled!");
                return;
            }
            if (fbLoginDlg.FacebookOAuthResult.IsSuccess)
            {
                var dlg = new FormFaceBookPublish(db, fbLoginDlg.FacebookOAuthResult.AccessToken);
                dlg.ShowDialog();
            }
            else
            {
                MessageBox.Show(fbLoginDlg.FacebookOAuthResult.ErrorDescription);
            }

        }
        /// <summary>
        /// 終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 登陆到QQ微博
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTencent_Click(object sender, EventArgs e)
        {
            FormQWeiboLogin LoginDlg = new FormQWeiboLogin(db);
            LoginDlg.ShowDialog();
            if (LoginDlg.Comfirm)
            {
                FormQWeiboPublish qweiboFrom = new FormQWeiboPublish(db);
                qweiboFrom.SetAccessKey(LoginDlg.AccessKey);
                qweiboFrom.SetAccessSecret(LoginDlg.AccessSecret);
                qweiboFrom.SetAppKey(LoginDlg.AppKey);
                qweiboFrom.SetAppSecret(LoginDlg.AppSecret);
                qweiboFrom.ShowDialog();
            }
        }
        /// <summary>
        /// Publish to wordpress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWordpress_Click(object sender, EventArgs e)
        {
            FormWordpressPublish form = new FormWordpressPublish(db);
            form.ShowDialog();
        }
        /// <summary>
        /// Publish to`Twitter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTwitter_Click(object sender, EventArgs e)
        {
            string _consumerKey = "oJs923I3JS4ztSaRpXj5Vg";
            string _consumerSecret = "h2bG02PmE52BEOlIINdy8M5WKLkDKWBxB0zCH220eQ";
            TwitterService service = new TwitterService(_consumerKey,_consumerSecret);
            FormTwitterLogin form = new FormTwitterLogin(db, service);
            if (form.ShowDialog() == DialogResult.OK)
            {
                FormTwitterPublish publish = new FormTwitterPublish(db, service);
                publish.ShowDialog();
            }
            //FormAuthSocialLogin loginForm = new FormAuthSocialLogin(db, PROVIDER_TYPE.TWITTER, manager);
            //if (loginForm.ShowDialog() == DialogResult.OK)
            //{
            //    FormAuthSocialPublish publishForm = new FormAuthSocialPublish(db, PROVIDER_TYPE.TWITTER, manager);
            //    publishForm.ShowDialog();
            //}
        }
        /// <summary>
        /// getManager
        /// </summary>
        private void getManager()
        {
            SocialAuthConfiguration config = new SocialAuthConfiguration();
            
            ProviderElement tpe = new ProviderElement();
            tpe.ConsumerKey = "oJs923I3JS4ztSaRpXj5Vg";
            tpe.ConsumerSecret = "h2bG02PmE52BEOlIINdy8M5WKLkDKWBxB0zCH220eQ";
            tpe.WrapperName = "TwitterWrapper";
            config.Providers.Add(tpe);

            ProviderElement gpe = new ProviderElement();
            gpe.ConsumerKey = "859090616008.apps.googleusercontent.com";
            gpe.ConsumerSecret = "kx-Iwp-TOXzc26wKpTff7dav";
            gpe.WrapperName = "GoogleWrapper";
            gpe.AdditionalScopes = "https://picasaweb.google.com/data/";
            config.Providers.Add(gpe);

            ProviderElement mpe = new ProviderElement();
            mpe.ConsumerKey = "000000004C108B16";
            mpe.ConsumerSecret = "kdJasG9HD8nj9OLyoJXxV81zg4S4TU3m";
            mpe.WrapperName = "MSNWrapper";
            config.Providers.Add(mpe);

            //ProviderElement ype = new ProviderElement();
            //ype.ConsumerKey = "000000004C108B16";
            //ype.ConsumerSecret = "kdJasG9HD8nj9OLyoJXxV81zg4S4TU3m";
            //ype.WrapperName = "YahooWrapper";
            //config.Providers.Add(ype);

            ProviderElement lpe = new ProviderElement();
            lpe.ConsumerKey = "753j58ks4sphrx";
            lpe.ConsumerSecret = "eQwl5uaT99WcDLw2";
            lpe.WrapperName = "LinkedInWrapper";
            config.Providers.Add(lpe);

            manager = new SocialAuthManager(config);

        }
        /// <summary>
        /// Google
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoogle_Click(object sender, EventArgs e)
        {
            FormAuthSocialLogin loginForm = new FormAuthSocialLogin(db, PROVIDER_TYPE.GOOGLE, manager);
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                FormAuthSocialPublish publishForm = new FormAuthSocialPublish(db, PROVIDER_TYPE.GOOGLE, manager);
                publishForm.ShowDialog();
            }

        }
        /// <summary>
        /// Linkedin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLinkedin_Click(object sender, EventArgs e)
        {
            FormAuthSocialLogin loginForm = new FormAuthSocialLogin(db, PROVIDER_TYPE.LINKEDIN, manager);
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                FormAuthSocialPublish publishForm = new FormAuthSocialPublish(db, PROVIDER_TYPE.LINKEDIN, manager);
                publishForm.ShowDialog();
            }

        }
        /// <summary>
        /// MSN
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMSN_Click(object sender, EventArgs e)
        {
            FormAuthSocialLogin loginForm = new FormAuthSocialLogin(db, PROVIDER_TYPE.MSN, manager);
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                FormAuthSocialPublish publishForm = new FormAuthSocialPublish(db, PROVIDER_TYPE.MSN, manager);
                publishForm.ShowDialog();
            }

        }
        /// <summary>
        /// MySpace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMyspace_Click(object sender, EventArgs e)
        {
            OffsiteContext context = new OffsiteContext("", "");
            RestV1 service = new RestV1(context);
            FormMySpaceLogin form = new FormMySpaceLogin(db, service);
            if (form.ShowDialog() == DialogResult.OK)
            {
                FormMySpacePublish form2 = new FormMySpacePublish(db, service);
                form2.ShowDialog();
            }
        }
        /// <summary>
        /// Yahoo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYahoo_Click(object sender, EventArgs e)
        {
            FormAuthSocialLogin loginForm = new FormAuthSocialLogin(db, PROVIDER_TYPE.YAHOO, manager);
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                FormAuthSocialPublish publishForm = new FormAuthSocialPublish(db, PROVIDER_TYPE.YAHOO, manager);
                publishForm.ShowDialog();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnOpenpne_Click(object sender, EventArgs e)
        {

        }
    }
}
