using System;
using System.Linq;
using System.Xml.Linq;
using _8BitMusic.core;
using MySpaceID.SDK;
using MySpaceID.SDK.Models.V1;
using MySpaceID.SDK.Api;
using MySpaceID.SDK.Context;
using Jayrock.Json.Conversion;
using MySpaceID.SDK.MySpace;
using MySpaceID.SDK.Models.RoaApi;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace _8BitMusic
{
    public partial class Default : System.Web.UI.Page
    {
        public int UserId { get; set; }
        public MySpace MySpace { get; set; }
        public UserProfile ExtendedProfile { get; set; }
        public FriendProfile Friends { get; set; }
        public XDocument Activities { get; set; }
        public XDocument FriendActivities { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (IsLoggedIn())
                LoggedIn();
            else
                LoggedOut();
        }

        private void LoggedIn()
        {

            this.MySpace = new MySpace(Constants.ConsumerKey, Constants.ConsumerSecret, Session["OAuthRequestToken"].ToString(), string.Empty, true, Session["varifier"].ToString());

            var user = (BasicProfile)JsonConvert.Import(typeof(BasicProfile), MySpace.GetCurrentUser());
            Session["userID"] = this.UserId = user.UserId;


            Dictionary<string, string> profileFields = new Dictionary<string, string>();
            profileFields.Add("fields", "aboutme,age,birthday,books,children,gender,interests,lookingfor,movies,music,profilesong,relationshipstatus,religion,sexualorientation,status,mszodiacsign");

            this.ExtendedProfile = (UserProfile)JsonConvert.Import(typeof(UserProfile), this.MySpace.GetPerson(this.UserId.ToString(), profileFields));

            //this.Friends = (FriendProfile)JsonConvert.Import(typeof(FriendProfile), this.MySpace.GetFriends(this.UserId.ToString(), profileFields));

            Dictionary<string, string> resultformat = new Dictionary<string, string>();
            resultformat.Add("format", "xml");

            this.FriendActivities = XDocument.Parse(this.MySpace.GetFriendsActivities(this.UserId.ToString(), resultformat));
        }

        private void LoggedOut()
        {
            this.Session.Remove("OAuthRequestToken");
            this.Session.Remove("varifier");
        }

        public bool IsLoggedIn()
        {

            return (Session["OAuthRequestToken"] != null && Session["varifier"] != null);

        }


        protected string ConvertStringArrayToStringJoin(string[] array)
        {
            if (array != null)
            {
                string result = string.Join(".", array);
                return result;
            }
            return string.Empty;
        }


        public string GetActivitiesHtml()
        {
            if (this.Activities == null)
                return "ACTIVITIES ERROR";
            XNamespace xns = "http://www.w3.org/2005/Atom";
            var entries = this.FriendActivities.Descendants(xns + "entry");
            var songEntries = entries.Where(x => (x.Element(xns + "category").Attribute("label") != null && (x.Element(xns + "category").Attribute("label").Value == "SongUpload" ||
               x.Element(xns + "category").Attribute("label").Value == "ProfileSongAdd")));
            string activityOutput = "<ul class='activitiesContainer'>";
            foreach (var entry in songEntries)
            {
                activityOutput += string.Format("<li class='activityItem {0}'>{1}</li>", entry.Element(xns + "category").Attribute("label").Value, entry.Element(xns + "content").FirstNode.ToString());

            }
            activityOutput += "</ul>";
            return activityOutput;
        }

        public string GetFriendActivitiesHtml()
        {
            if (this.FriendActivities == null)
                return "ACTIVITIES ERROR";

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            string activityOutput = "<ul class='activitiesContainer'>";
            using (XmlReader reader = XmlReader.Create(new StringReader(FriendActivities.ToString()), settings))
            {

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        if (reader.Value.Contains("myspace.com."))
                        {
                            activityOutput += string.Format("<li class='activityItem {0}'></li>", reader.Value);
                        }
                        else
                        {
                            activityOutput += string.Format("<li class='activityItem '>{0}</li>", reader.Value);
                        }
                    }
                }
            }

            activityOutput += "</ul>";
            return activityOutput;
        }

    }
}
