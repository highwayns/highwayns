using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Brickred.SocialAuth.NET.Core.BusinessObjects;

namespace Brickred.SocialAuth.NET.Demo
{
    public partial class DemoSite : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (SocialAuthUser.IsLoggedIn())
                lbtnLogout.Visible = true;

        }

        protected void lbtnLogout_Click(object sender, EventArgs e)
        {
            SocialAuthUser.GetCurrentUser().Logout("Default.aspx");
            lbtnLogout.Visible = false;
        }
    }
}