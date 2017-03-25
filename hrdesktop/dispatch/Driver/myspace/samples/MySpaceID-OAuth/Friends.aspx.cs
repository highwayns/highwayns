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
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Api;
using Jayrock.Json.Conversion;
using MySpaceID.SDK.Models.V1;
using System.Text;
using MySpaceID.SDK.MySpace;
using MySpaceID.SDK.Models.RoaApi;
using System.Collections.Generic;

namespace MySpaceID_OAuth
{
    public partial class Friends : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["offSiteMySpace"] != null)
            {
                try
                {


                    MySpace myspaceClient = (MySpace)Session["offSiteMySpace"];
                    var user = (BasicProfile)JsonConvert.Import(typeof(BasicProfile), myspaceClient.GetCurrentUser());
                    Dictionary<string, string> profileFields = new Dictionary<string, string>();
                    // Add Items to UserProfile Class which are to be featched in user profile
                    profileFields.Add("fields", "aboutme,age,birthday,books,children,gender,interests,lookingfor,movies,music,profilesong,relationshipstatus,religion,sexualorientation,status,mszodiacsign,currentlocation");
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

                    PopulateFriends(myspaceClient, user.UserId);
                }
                catch (Exception ex)
                {
                    divPhotos.InnerHtml = "An Error occured: " + ex.Message;

                }
            }
        }

        private void PopulateFriends(MySpace myspaceClient, int userId)
        {
            var friends = (FriendsProfile)JsonConvert.Import(typeof(FriendsProfile), myspaceClient.GetFriends(userId.ToString()));
            if (friends.TotalResults > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<table>");

                foreach (var friend in friends.entry)
                {

                    sb.Append("<tr>");
                    sb.Append(string.Format("<td><img src='{0}'/> </td>", friend.person.thumbnailUrl));
                    sb.Append(string.Format("<td>Name: {0}<br/>Status: {1}<br/>Web Url: {2}</td>", friend.person.displayName, friend.person.status, friend.person.profileUrl));

                    sb.Append("</tr>");
                }
                sb.Append("</table>");

                divPhotos.InnerHtml = sb.ToString();
            }
        }
    }
}
