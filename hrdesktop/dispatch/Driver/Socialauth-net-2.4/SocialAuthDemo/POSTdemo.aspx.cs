using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using Brickred.SocialAuth.NET.Core;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace Brickred.SocialAuth.NET.Demo
{
    public partial class POSTdemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SocialAuthUser.IsConnectedWith(PROVIDER_TYPE.TWITTER))
            {
                btnPOST.Enabled = false;
                btnGET.Enabled = false;
                errLabel.Text = "You need to be logged into Twitter to try this demo!";
                errLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                GetTweets();
            }
        }

        private void GetTweets()
        {
            var response = SocialAuthUser.GetCurrentUser().ExecuteFeed("https://api.twitter.com/1.1/statuses/home_timeline.json?include_entities=true", TRANSPORT_METHOD.GET, PROVIDER_TYPE.TWITTER);
            var tweetsJson = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var tweets = JArray.Parse(tweetsJson);
            var i = 0;

            foreach (var tweet in tweets)
            {
                HtmlGenericControl newdiv = new HtmlGenericControl("div");
                newdiv.Style.Add("clear", "both");
                newdiv.Style.Add("margin-top", "5px;");
                newdiv.Style.Add("background-color", i % 2 == 0 ? "#CCFF66" : "#FFF999");
                newdiv.Style.Add("height", "100%");
                newdiv.InnerHtml = "<span style='float:left'><img src=" + tweet.SelectToken("user.profile_image_url_https") + " /></span>";
                newdiv.InnerHtml += "<span><b>" + tweet.SelectToken("user.name") + "</b> says:<br>" + tweet.SelectToken("text") + "</span>";
                divTweets.Controls.Add(newdiv);
                if (++i == 10)
                    break;
            }
        }

        protected void btnPOST_Click(object sender, EventArgs e)
        {

            string msg = HttpUtility.UrlEncode(txtTweet.Text);
            string endpoint = "http://api.twitter.com/1.1/statuses/update.json?status=" + msg;

            string body = String.Empty;
            byte[] reqbytes = new ASCIIEncoding().GetBytes(body);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("contentType", "application/x-www-form-urlencoded");
            var response = SocialAuthUser.GetCurrentUser().ExecuteFeed(
                    endpoint,
                    TRANSPORT_METHOD.POST,
                    PROVIDER_TYPE.TWITTER,
                    reqbytes,
                    headers
                 );

            errLabel.ForeColor = System.Drawing.Color.Purple;
            errLabel.Text = "Post successfully posted! Please refresh tweets list to confirm or check on Twitter!!";
}
        protected void btnGET_Click(object sender, EventArgs e)
        {
            GetTweets();
        }


    }
}
