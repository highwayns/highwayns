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
using MySpaceID.SDK.OAuth.Common.Enums;
using MySpaceID_OAuth.Core;

namespace MySpaceID_OAuth
{
    public partial class OAuthRequestToServer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            var context = new OnsiteContext("YOUR_CONSUMER_KEY", "YOUR_CONSUMER_SECRET");
            context.OAuthConsumer.ApiServerUri = "http://localhost:9090";
            var response = context.MakeRequest("/oauthvalidation.aspx", ResponseFormatType.Default, HttpMethodType.GET, null, false);

            Response.Write(response);
        }
    }
}
