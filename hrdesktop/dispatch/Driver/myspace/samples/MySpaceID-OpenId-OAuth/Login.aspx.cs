using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.OAuth;
using DotNetOpenAuth.Messaging;
using MySpaceID_OpenId_OAuth.Core;
using System.Net;

namespace MySpaceID_OpenId_OAuth
{
    public partial class Login : System.Web.UI.Page
    {
        protected void openidValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // This catches common typos that result in an invalid OpenID Identifier.
            args.IsValid = Identifier.IsValid(args.Value);
        }

        private void CheckConsumerKey_Secret()
        {
            if (string.IsNullOrEmpty(Constants.ConsumerKey) || string.IsNullOrEmpty(Constants.ConsumerSecret))
            {
                lblErrorMessage.Visible = true;
                loginButton.Enabled = false;
                linkDirectIdentity.Enabled = false;
                string format = "The following settings were not set: {0}. Please set them in /Core/Constants.cs in order to proceed with the sample.";
                string missingSettings = string.Empty;
                if (string.IsNullOrEmpty(Constants.ConsumerKey))
                    missingSettings += "ConsumerKey,";
                if (string.IsNullOrEmpty(Constants.ConsumerSecret))
                    missingSettings+= "ConsumerSecret,";
                missingSettings = missingSettings.Remove(missingSettings.Length-1);
                lblErrorMessage.Text = string.Format(format, missingSettings);
               
            }
            else
            {
                lblErrorMessage.Visible = false;
                loginButton.Enabled = true;
                linkDirectIdentity.Enabled = true;
            }
        }

        protected void loginButton_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return; // don't login if custom validation failed.
            }
            try
            {
                using (OpenIdRelyingParty openid = this.createRelyingParty())
                {
                    IAuthenticationRequest request = openid.CreateRequest(this.openIdBox.Text);
                    // Here is where we add the OpenID+OAuth Extension
                    request.AddExtension(new OAuthRequest(Constants.ConsumerKey));

                    // Send your visitor to their Provider for authentication.
                    request.RedirectToProvider();
                }
            }
            catch (ProtocolException ex)
            {
                // The user probably entered an Identifier that 
                // was not a valid OpenID endpoint.
                this.openidValidator.Text = ex.Message;
                this.openidValidator.IsValid = false;
            }
            catch (WebException ex)
            {
                // The user probably entered an Identifier that 
                // was not a valid OpenID endpoint.
                this.openidValidator.Text = ex.Message;
                this.openidValidator.IsValid = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            this.openIdBox.Focus();


            CheckConsumerKey_Secret();

            //Construct the Direct Identity button.
            string myServer = "http://localhost:9090/";
            this.linkDirectIdentity.Text = "Login to MySpace via Direct Identity";
            this.linkDirectIdentity.NavigateUrl = Constants.API_MYSPACE_COM + "/openid?openid.ns=" + HttpUtility.UrlEncode("http://specs.openid.net/auth/2.0") +
                                                                                "&openid.ns.oauth=" + HttpUtility.UrlEncode("http://specs.openid.net/extensions/oauth/1.0") +
                                                                                "&openid.oauth.consumer=" + HttpUtility.UrlEncode(Constants.ConsumerKey) +
                                                                                "&openid.mode=checkid_setup" +
                                                                                "&openid.claimed_id=" + HttpUtility.UrlEncode("http://specs.openid.net/auth/2.0/identifier_select") +
                                                                                "&openid.identity=" + HttpUtility.UrlEncode("http://specs.openid.net/auth/2.0/identifier_select") +
                                                                                "&openid.return_to=" + HttpUtility.UrlEncode(myServer + "login.aspx") +
                                                                                "&openid.realm=" + HttpUtility.UrlEncode(myServer);

            // For debugging/testing, we allow remote clearing of all associations...
            // NOT a good idea on a production site.
            if (Request.QueryString["clearAssociations"] == "1")
            {
                Application.Remove("DotNetOpenId.RelyingParty.RelyingParty.AssociationStore");

                // Force a redirect now to prevent the user from logging in while associations
                // are constantly being cleared.
                UriBuilder builder = new UriBuilder(Request.Url);
                builder.Query = null;
                Response.Redirect(builder.Uri.AbsoluteUri);
            }

            OpenIdRelyingParty openid = this.createRelyingParty();
            var response = openid.GetResponse();
            if (response != null)
            {
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        //Here is where we get the data that comes back due to the use of our OpenID+OAuth extension.
                        //We save it into state for later use.
                        State.OAuthOpenID = response.GetExtension<OAuthResponse>();
                        if (State.OAuthOpenID == null)
                            throw new Exception("There was a problem with the OpenID+OAuth Extension. A likely problem may be that your application's " +
                                "realm registered with developer.myspace.com does not have a closing slash (e.g. http://localhost:9090/ is needed " +
                                "rather than http://localhost:9090).");
                        //Redirect the user to the page he intended.
                        FormsAuthentication.RedirectFromLoginPage(response.ClaimedIdentifier, false);
                        break;
                    case AuthenticationStatus.Canceled:
                        this.loginCanceledLabel.Visible = true;
                        break;
                    case AuthenticationStatus.Failed:
                        this.loginFailedLabel.Visible = true;
                        break;
                }
            }
        }

        private OpenIdRelyingParty createRelyingParty()
        {
            OpenIdRelyingParty openid = new OpenIdRelyingParty();
            int minsha, maxsha, minversion;
            if (int.TryParse(Request.QueryString["minsha"], out minsha))
            {
                openid.SecuritySettings.MinimumHashBitLength = minsha;
            }
            if (int.TryParse(Request.QueryString["maxsha"], out maxsha))
            {
                openid.SecuritySettings.MaximumHashBitLength = maxsha;
            }
            if (int.TryParse(Request.QueryString["minversion"], out minversion))
            {
                switch (minversion)
                {
                    case 1: openid.SecuritySettings.MinimumRequiredOpenIdVersion = ProtocolVersion.V10; break;
                    case 2: openid.SecuritySettings.MinimumRequiredOpenIdVersion = ProtocolVersion.V20; break;
                    default: throw new ArgumentOutOfRangeException("minversion");
                }
            }
            return openid;
        }
    }
}
