using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;

namespace Brickred.SocialAuth.NET.Demo
{
    public partial class FbStatusUpdateDemo : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SocialAuthUser.IsConnectedWith(PROVIDER_TYPE.FACEBOOK))
            {
                btnPOST.Enabled = false;
                btnGET.Enabled = false;
                errLabel.Text = "You need to be logged into Facebook to try this demo!";
                errLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                GetUpdates();
            }
        }

        private void GetUpdates()
        {
            var response = SocialAuthUser.GetCurrentUser().ExecuteFeed("https://graph.facebook.com/me/feed?access_token=" + SocialAuthUser.GetCurrentUser().GetAccessToken(), TRANSPORT_METHOD.GET, PROVIDER_TYPE.FACEBOOK);
            var updatesJson = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var data = JObject.Parse(updatesJson);
            var updates = JArray.Parse(data.SelectToken("data").ToString());
            var i = 0;

            foreach (var tweet in updates)
            {
                HtmlGenericControl newdiv = new HtmlGenericControl("div");
                newdiv.Style.Add("clear", "both");
                newdiv.Style.Add("margin-top", "5px;");
                newdiv.Style.Add("background-color", i % 2 == 0 ? "#CCFF66" : "#FFF999");
                newdiv.Style.Add("height", "100%");
                //newdiv.InnerHtml = "<span style='float:left'><img src=" + tweet.SelectToken("user.profile_image_url_https") + " /></span>";
                newdiv.InnerHtml += "<span><b>" + tweet.SelectToken("from.name") + "</b> says:<br>" + tweet.SelectToken("message") + "</span>";
                divUpdates.Controls.Add(newdiv);
                if (++i == 10)
                    break;
            }
        }

        protected void btnPOST_Click(object sender, EventArgs e)
        {

            string msg = HttpUtility.UrlEncode(txtStatus.Text);
            string endpoint = "https://graph.facebook.com/me/feed?message=" + msg + "&access_token=" + SocialAuthUser.GetCurrentUser().GetConnection(PROVIDER_TYPE.FACEBOOK).GetConnectionToken().AccessToken ;

            string body = String.Empty;
            byte[] reqbytes = new ASCIIEncoding().GetBytes(body);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("contentType", "application/x-www-form-urlencoded");
            var response = SocialAuthUser.GetCurrentUser().ExecuteFeed(
                    endpoint,
                    TRANSPORT_METHOD.POST,
                    PROVIDER_TYPE.FACEBOOK,
                    reqbytes,
                    headers
                 );

            errLabel.ForeColor = System.Drawing.Color.Purple;
            errLabel.Text = "Post successfully posted! Please refresh updates list to confirm or check on Facebook!!";
        }
        protected void btnGET_Click(object sender, EventArgs e)
        {
            GetUpdates();
        }

    }
}