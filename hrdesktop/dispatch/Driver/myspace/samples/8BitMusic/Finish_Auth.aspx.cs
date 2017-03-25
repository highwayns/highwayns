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
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.OAuth;
using MySpaceID.SDK;
using MySpaceID.SDK.OAuth.Tokens;
using DotNetOpenAuth.OpenId;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Api;
using MySpaceID.SDK.Models.V1;
using Jayrock.Json.Conversion;

namespace _8BitMusic
{
    public partial class Finish_Auth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OpenIdRelyingParty openid = this.createRelyingParty();
            var response = openid.GetResponse();
            if (response != null)
            {
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        //Here is where we get the data that comes back due to the use of our OpenID+OAuth extension.
                        //We save it into state for later use.
                        var oAuthOpenID = response.GetExtension<OAuthResponse>();
                        if (oAuthOpenID == null)
                            throw new Exception("There was a problem with the OpenID+OAuth Extension. A likely problem may be that your application's " +
                                "realm registered with developer.myspace.com does not have a closing slash (e.g. http://localhost:9090/ is needed " +
                                "rather than http://localhost:9090).");
                        var verifier = Request.QueryString["openid.oauth.verifier"];
                        Session["varifier"] = verifier;
                        Session["OAuthRequestToken"] = Request.QueryString["openid.oauth.request_token"];
                        break;
                    case AuthenticationStatus.Canceled:
                        break;
                    case AuthenticationStatus.Failed:
                        break;
                }
            }
        }
        private OpenIdRelyingParty createRelyingParty()
        {
            OpenIdRelyingParty openid = new OpenIdRelyingParty();
            int minsha, maxsha, minversion;
            if (int.TryParse(Request.QueryString["minsha"], out minsha))
            {
                openid.SecuritySettings.MinimumHashBitLength = minsha;
            }
            if (int.TryParse(Request.QueryString["maxsha"], out maxsha))
            {
                openid.SecuritySettings.MaximumHashBitLength = maxsha;
            }
            if (int.TryParse(Request.QueryString["minversion"], out minversion))
            {
                switch (minversion)
                {
                    case 1: openid.SecuritySettings.MinimumRequiredOpenIdVersion = ProtocolVersion.V10; break;
                    case 2: openid.SecuritySettings.MinimumRequiredOpenIdVersion = ProtocolVersion.V20; break;
                    default: throw new ArgumentOutOfRangeException("minversion");
                }
            }
            return openid;
        }
    }
}
