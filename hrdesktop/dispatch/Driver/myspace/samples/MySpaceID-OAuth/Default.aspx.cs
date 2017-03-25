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
using MySpaceID.SDK;
using MySpaceID_OAuth.Core;
using MySpaceID.SDK.OAuth.Tokens;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Api;
using MySpaceID.SDK.Models.V2;
using Jayrock.Json.Conversion;
using MySpaceID.SDK.Models.V1;
using System.Collections.Generic;
using MySpaceID.SDK.MySpace;
using MySpaceID.SDK.Models.RoaApi;

namespace MySpaceID_OAuth
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["oauthreturn"] != null)
            {
                divLoggedOut.Visible = false;
                divLoggedIn.Visible = true;
                PopulateUserProfile();

            }
            else
            {
                divLoggedOut.Visible = true;
                divLoggedIn.Visible = false;
                CheckConsumerKey_Secret();
            }
        }
        private void CheckConsumerKey_Secret()
        {
            if (string.IsNullOrEmpty(Constants.ConsumerKey) || string.IsNullOrEmpty(Constants.ConsumerSecret))
            {
                lblErrorMessage.Visible = true;
                btnOAuth.Enabled = false;
                string format = "The following settings were not set: {0}. Please set them in /Core/Constants.cs in order to proceed with the sample.";
                string missingSettings = string.Empty;
                if (string.IsNullOrEmpty(Constants.ConsumerKey))
                    missingSettings += "ConsumerKey,";
                if (string.IsNullOrEmpty(Constants.ConsumerSecret))
                    missingSettings += "ConsumerSecret,";
                missingSettings = missingSettings.Remove(missingSettings.Length - 1);
                lblErrorMessage.Text = string.Format(format, missingSettings);

            }
            else
            {
                lblErrorMessage.Visible = false;
                btnOAuth.Enabled = true;
            }
        }
        protected void btnOAuth_Click(object sender, EventArgs e)
        {
            GetRequestToken();
        }

        private void PopulateUserProfile()
        {
            try
            {
                var requestTokenKey = Session["requesttokenkey"].ToString();
                var requestTokenSecret = Session["requesttokensecret"].ToString();
                var verifier = Request.QueryString["oauth_verifier"];
                MySpace offSiteMySpace = new MySpace(Constants.ConsumerKey, Constants.ConsumerSecret, requestTokenKey, requestTokenSecret, true, verifier);


                Session["accesstokenkey"] = offSiteMySpace.OAuthTokenKey;
                Session["accesstokensecret"] = offSiteMySpace.OAuthTokenSecret;
                Application["accesstokensecret"] = offSiteMySpace.OAuthTokenSecret;

                Session["offSiteMySpace"] = offSiteMySpace;

                var user = (BasicProfile)JsonConvert.Import(typeof(BasicProfile), offSiteMySpace.GetCurrentUser());
                Dictionary<string, string> profileFields = new Dictionary<string, string>();
                // Add Items to UserProfile Class which are to be featched in user profile
                profileFields.Add("fields", "aboutme,age,birthday,books,children,gender,interests,lookingfor,movies,music,profilesong,relationshipstatus,religion,sexualorientation,status,mszodiacsign,currentlocation");
                var profile = (UserProfile)JsonConvert.Import(typeof(UserProfile), offSiteMySpace.GetPerson(user.UserId.ToString(), profileFields));

                offSiteMySpace.GetVideosCategory(user.UserId.ToString(), "11");
                ExtendedP.Text = "My Books are : " + ConvertStringArrayToStringJoin(profile.person.books)
                                  + "<br/>  Intrests:" + ConvertStringArrayToStringJoin(profile.person.interests)
                                  + "<br/>  Movies I Like:" + ConvertStringArrayToStringJoin(profile.person.movies)
                                  + "<br/>  My Music:" + ConvertStringArrayToStringJoin(profile.person.music) + "<br/>  Relationship Status:" + profile.person.relationshipstatus;

                Session["UserID"] = user.UserId;
                imageProfile.ImageUrl = user.LargeImage;
                lblAge.Text = profile.person.age;
                lblCity.Text = profile.person.currentlocation.locality;
                lblCountry.Text = profile.person.currentlocation.country;
                lblGender.Text = profile.person.gender;
                lblMarital.Text = profile.person.relationshipstatus;
                lblName.Text = user.Name;
                lblProfileUrl.Text = user.WebUri;
                lblRegion.Text = profile.person.sexualorientation;
            }
            catch (Exception ex)
            {
                ExtendedP.Text = "An Error occured: " + ex.Message;

            }


        }

        static string ConvertStringArrayToStringJoin(string[] array)
        {
            if (array != null)
            {
                string result = string.Join(".", array);
                return result;
            }
            else
                return string.Empty;
        }

        private void GetRequestToken()
        {
            var context = new OffsiteContext(Constants.ConsumerKey, Constants.ConsumerSecret);
            //Get a Request Token
            var callbackUrl = "http://localhost:9090/default.aspx?oauthreturn=true";
            var requestToken = context.GetRequestToken(callbackUrl);
            Session["requesttokenkey"] = requestToken.TokenKey;
            Session["requesttokensecret"] = requestToken.TokenSecret;

            //Get the MySpace authentication page for the user to go to in order to authorize the Request Token.
            var authenticationUrl = context.GetAuthorizationUrl(requestToken, callbackUrl);

            Response.Redirect(authenticationUrl);
        }

        protected void btnUpdateStatus_Click(object sender, EventArgs e)
        {


        }
    }
}
