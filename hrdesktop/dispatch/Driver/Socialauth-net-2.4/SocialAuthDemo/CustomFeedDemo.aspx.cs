using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace Brickred.SocialAuth.NET.Demo
{
    public partial class CustomFeedDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SocialAuthUser.IsConnectedWith(PROVIDER_TYPE.FACEBOOK))
            {
                lblAlbum.Text = "You need to login with Facebook to view this demo!";
                btnCustomFeed.Visible = false;
            }
        }

        protected void btnCustomFeed_Click(object sender, EventArgs e)
        {
            if (SocialAuthUser.IsConnectedWith(PROVIDER_TYPE.FACEBOOK))
            {

                WebResponse wr = SocialAuthUser.GetCurrentUser().ExecuteFeed("https://graph.facebook.com/me/albums", TRANSPORT_METHOD.GET, PROVIDER_TYPE.FACEBOOK);
                StreamReader reader = new StreamReader(wr.GetResponseStream());
                string albumJson = reader.ReadToEnd();
                JObject jsonObject = JObject.Parse(albumJson);
                List<Album> albums = new List<Album>();

                jsonObject["data"].Children().ToList().ForEach(x =>
                {
                    albums.Add(new Album()
                    {
                        ID = (string)x["id"],
                        PhotoCount = x["count"] == null ? 0 : Convert.ToInt32(x["count"].ToString().Replace("\"", "")),
                        Name = (string)x["name"].ToString().Replace("\"", ""),
                        Location = (string)x["location"] == null ? "" : x["location"].ToString().Replace("\"", ""),
                        CoverPhoto = (string)x["cover_photo"] == null ? "" : x["cover_photo"].ToString().Replace("\"", "")
                    });

                });

                foreach (var item in albums)
                {
                    Label lbl = new Label();
                    lbl.Text = "<h3>" + item.Name + "</h3>(" + item.PhotoCount + ") : " + "<img src='https://graph.facebook.com/" + item.CoverPhoto + "/picture?type=album&access_token=" + SocialAuthUser.GetCurrentUser().GetAccessToken() + "'>";
                    lbl.CssClass = "album";
                    lblAlbum.Controls.Add(lbl);

                }

                lblJson.Text = "Executed custom feed: <b>https://graph.facebook.com/me/albums</b><br>Result:<br>" + albumJson;
            }
        }
    }
}