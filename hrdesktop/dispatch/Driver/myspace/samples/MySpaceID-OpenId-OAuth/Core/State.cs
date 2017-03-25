using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DotNetOpenAuth.OpenId.Extensions.OAuth;
using MySpaceID.SDK.OAuth.Tokens;

namespace MySpaceID_OpenId_OAuth.Core
{
    public class State
    {
        public static string FriendlyLoginName
        {
            get { return HttpContext.Current.Session["FriendlyUsername"] as string; }
            set { HttpContext.Current.Session["FriendlyUsername"] = value; }
        }

        public static OAuthResponse OAuthOpenID
        {
            get { return HttpContext.Current.Session["OAuthOpenID"] as OAuthResponse; }
            set { HttpContext.Current.Session["OAuthOpenID"] = value; }
        }

        public static void Clear()
        {
            FriendlyLoginName = string.Empty;
            OAuthOpenID = null;
            
        }
    }
}
