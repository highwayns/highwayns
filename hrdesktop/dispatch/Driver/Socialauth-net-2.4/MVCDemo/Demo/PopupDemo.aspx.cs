using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace Brickred.SocialAuth.NET.Demo
{
    public partial class PopupDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        [WebMethod]
        public static void Login(string providername)
        {
            PROVIDER_TYPE providerType = (PROVIDER_TYPE)Enum.Parse(typeof(PROVIDER_TYPE), providername);
            SocialAuthUser.GetCurrentUser().Login(providerType);
        }

        [WebMethod]
        public static string IsUserLoggedIn()
        {
            try
            {
                return SocialAuthUser.IsLoggedIn().ToString();
            }
            catch
            {
                return "error";
            }
        }   

        [WebMethod]
        public static string GetFriends()
        {
            var friends = SocialAuthUser.GetCurrentUser().GetContacts();
            return new JavaScriptSerializer().Serialize(friends);
        }

    }
}