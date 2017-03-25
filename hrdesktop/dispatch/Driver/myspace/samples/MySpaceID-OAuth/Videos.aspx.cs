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
    public partial class Videos : System.Web.UI.Page
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

                    PopulateVideos(myspaceClient, user.UserId.ToString());

                }
                catch (Exception ex)
                {
                    divPhotos.InnerHtml = "An Error occured: " + ex.Message;

                }

            }
            else
            {
                Response.Redirect("/default.aspx");
            }
        }

        private void PopulateVideos(MySpace myspaceClient, string userId)
        {

            var videos = (MySpaceID.SDK.Models.RoaApi.UserMediaItems)JsonConvert.Import(typeof(MySpaceID.SDK.Models.RoaApi.UserMediaItems), myspaceClient.GetVideos(userId));
            if (videos.TotalResults > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<table>");

                foreach (var video in videos.entry)
                {

                    sb.Append("<tr>");
                    sb.Append(string.Format("<td><img src='{0}'/> </td>", video.mediaItem.thumbnailUrl));
                    sb.Append(string.Format("<td>Title: {0}<br/>Video Url: {1}</td>", video.mediaItem.title, video.mediaItem.url));

                    sb.Append("</tr>");
                }
                sb.Append("</table>");

                divPhotos.InnerHtml = sb.ToString();
            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                try
                {
                    {
                        if (FileUploadControl.PostedFile.ContentLength < 10240000)
                        {
                            if (Session["offSiteMySpace"] != null)
                            {

                                MySpace myspaceClient = (MySpace)Session["offSiteMySpace"];

                                if (Session["UserID"] != null)
                                {
                                    string userId = Convert.ToString(Session["UserID"]);

                                    var roaApi = myspaceClient;

                                    roaApi.AddVideo(userId, "Test", "tags", 11, "en-US", "Test Photo From App", FileUploadControl.FileBytes);

                                    StatusLabel.Text = "Upload status: File uploaded!";

                                }
                            }
                        }
                        else
                            StatusLabel.Text = "Upload status: The file has to be less than 10000 kb!";
                    }
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
        }
    }
}
