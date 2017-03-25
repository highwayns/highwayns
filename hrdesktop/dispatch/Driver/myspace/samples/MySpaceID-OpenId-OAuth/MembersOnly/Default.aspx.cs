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
using MySpaceID_OpenId_OAuth.Core;
using MySpaceID.SDK;
using MySpaceID.SDK.OAuth.Tokens;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Api;
using MySpaceID.SDK.Models.V1;
using Jayrock.Json.Conversion;
using MySpaceID.SDK.MySpace;
using MySpaceID.SDK.Models.RoaApi;
using System.Collections.Generic;

namespace MySpaceID_OpenId_OAuth.MembersOnly
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateUserProfile();
        }
        private void PopulateUserProfile()
        {
            if (State.OAuthOpenID != null)
            {
                var verifier = Request.QueryString["oauth_verifier"];
                var myspaceClient = new MySpace(Constants.ConsumerKey, Constants.ConsumerSecret, State.OAuthOpenID.RequestToken, string.Empty, true, verifier); ;
                var user = (BasicProfile)JsonConvert.Import(typeof(BasicProfile), myspaceClient.GetCurrentUser());
                Dictionary<string, string> profileFields = new Dictionary<string, string>();
                // Add Items to UserProfile Class which are to be featched in user profile
                profileFields.Add("fields", "aboutme,age,birthday,books,children,gender,interests,lookingfor,movies,music,profilesong,relationshipstatus,religion,sexualorientation,status,mszodiacsign");

                var profile = (UserProfile)JsonConvert.Import(typeof(UserProfile), myspaceClient.GetPerson(user.UserId.ToString(), profileFields));

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

                PopulatePhotos(myspaceClient, user.UserId);
            }

        }

        private void PopulatePhotos(MySpace myspaceClient, int userId)
        {
            var photos = (UserPhotos)JsonConvert.Import(typeof(UserPhotos), myspaceClient.GetAllPhotos(userId));
            if (photos.Count > 0)
            {
                divPhotos.InnerHtml = "<ul>";
                foreach (var photo in photos.Photos)
                {
                    divPhotos.InnerHtml += string.Format("<li><img src='{0}' </></li>", photo.SmallImageUri);
                }
                divPhotos.InnerHtml += "</ul>";
            }
        }
    }
}
