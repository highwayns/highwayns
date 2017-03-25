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
using System.IO;
using MySpaceID.SDK.Models.RoaApi;
using System.Collections.Generic;
using MySpaceID.SDK.MySpace;
using MySpaceID.SDK.Models.Search;

namespace MySpaceID_OAuth
{
    public partial class Search : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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

                    }
                    catch (Exception ex)
                    {
                        dvReslut.InnerHtml = "An Error occured: " + ex.Message;

                    }
                }
        }

        private void getSearchResults(string textToSearch, int type)
        {
            if (Session["offSiteMySpace"] != null)
            {
                MySpace os = (MySpace)Session["offSiteMySpace"];
                Dictionary<string, string> ht = new Dictionary<string, string>();
                ht.Add("count", "20");
                SearchResultVideos searchResult = new SearchResultVideos();
                SearchResultImages searchResultImages = new SearchResultImages();
                SearchResultPeople searchResultPeople = new SearchResultPeople();
                switch (type)
                {
                    case 1:
                        searchResult = (SearchResultVideos)JsonConvert.Import(typeof(SearchResultVideos), os.SearchVideos(txtSearch.Text, ht));
                        break;


                    case 2:
                        searchResultImages = (SearchResultImages)JsonConvert.Import(typeof(SearchResultImages), os.SearchImages(txtSearch.Text, ht));
                        break;


                    case 3:
                        searchResultPeople = (SearchResultPeople)JsonConvert.Import(typeof(SearchResultPeople), os.SearchPeople(txtSearch.Text, ht));
                        break;


                    default:
                        break;
                }
                if (searchResult.TotalResults > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    foreach (var result in searchResult.entry)
                    {
                        sb.Append("<tr>");
                        sb.Append(string.Format("<td><img src='{0}' /></td>", result.thumbnailUrl));
                        sb.Append(string.Format("<td>Title: {0}<br/>Video URL: <a href='{1}'>Click here to View</a><br/>Categories: {2}<br/>Description: {3}<br/></td>", result.title, result.videoUrl, result.categories, result.description));
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");

                    dvReslut.InnerHtml = sb.ToString();
                }
                else if (searchResultImages.TotalResults > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    foreach (var result in searchResultImages.entry)
                    {
                        sb.Append("<tr>");
                        sb.Append(string.Format("<td><img src='{0}' /></td>", result.thumbnailUrl));
                        sb.Append(string.Format("<td>Title: {0}<br/>Image URL: <a href='{1}'>Click here to View</a><br/></td>", result.displayName, result.imageUrl));
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");

                    dvReslut.InnerHtml = sb.ToString();
                }
                else if (searchResultPeople.TotalResults > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    foreach (var result in searchResultPeople.entry)
                    {
                        sb.Append("<tr>");
                        sb.Append(string.Format("<td><img src='{0}' /></td>", result.thumbnailUrl));
                        sb.Append(string.Format("<td>Name: {0}<br/>Gender: {1}<br/>Location: {2}<br/>Profile URL: {3}<br/></td>", result.displayName, result.gender, result.location, result.profileUrl));
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");

                    dvReslut.InnerHtml = sb.ToString();
                }
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            getSearchResults(txtSearch.Text, Convert.ToInt32(ddValue.SelectedItem.Value));
        }

    }
}
