using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using HPSWizard;
using NC.HPS.Lib;
using TweetSharp;

namespace HPSManagement.PublishMagzine
{
	public partial class WizardPage41 : HPSWizard.WizardPage
	{
        private CmWinServiceAPI db;
        private string _consumerKey = "oJs923I3JS4ztSaRpXj5Vg";
        private string _consumerSecret = "h2bG02PmE52BEOlIINdy8M5WKLkDKWBxB0zCH220eQ";
        private TwitterService twitterService = null;
        private OAuthRequestToken requestToken=null;

        //--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor that assumes the page type is "intermediate"
		/// </summary>
		/// <param name="parent">The parent WizardFormBase-derived form</param>
        public WizardPage41(WizardFormBase parent, CmWinServiceAPI db) 
					: base(parent)
		{
            this.db = db;
			InitPage();
            twitterService = new TwitterService(_consumerKey, _consumerSecret);
        }

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor that allows the programmer to specify the page type. In 
		/// the case of the sample app, we use this constructor for this oject, 
		/// and specify it as the start page.
		/// </summary>
		/// <param name="parent">The parent WizardFormBase-derived form</param>
		/// <param name="pageType">The type of page this object represents (start, intermediate, or stop)</param>
        public WizardPage41(WizardFormBase parent, WizardPageType pageType, CmWinServiceAPI db) 
					: base(parent, pageType)
		{
            this.db = db;
			InitPage();
            twitterService = new TwitterService(_consumerKey, _consumerSecret);
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
            NextPages[0].PageData["TwitterService"] = twitterService;
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
                //取得OauthVerify
                int startIndex = webBrowser1.DocumentText.IndexOf("oauth_verifier");
                int endIndex = webBrowser1.DocumentText.IndexOf('\"', startIndex);
                string verifier = webBrowser1.DocumentText.Substring(startIndex + 15, endIndex - startIndex - 15);

                OAuthAccessToken accessToken = twitterService.GetAccessToken(requestToken, verifier);
                if (accessToken != null)
                {
                    twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
                    ButtonStateNext |= WizardButtonState.Enabled;
                    ParentWizardForm.UpdateWizardForm(this);
                }
                else
                {
                    ButtonStateNext = WizardButtonState.Visible;
                }
            }
            else
            {
                ButtonStateNext = WizardButtonState.Visible;
            }

        }
        /// <summary>
        /// WizardPage41_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WizardPage41_Load(object sender, EventArgs e)
        {
            requestToken = twitterService.GetRequestToken();

            var uri = twitterService.GetAuthorizationUri(requestToken);

            webBrowser1.Navigate(uri.ToString());

        }

	}
}
