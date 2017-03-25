using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Brickred.SocialAuth.NET.Core.BusinessObjects;

namespace Brickred.SocialAuth.NET.Demo
{
    public partial class PopupProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Request.QueryString["provider"] != null)
                {
                    PROVIDER_TYPE providerType = (PROVIDER_TYPE)Enum.Parse(typeof(PROVIDER_TYPE), Request.QueryString["provider"].ToUpper());
                    SocialAuthUser.GetCurrentUser().Login(providerType, "popupprocess.aspx", errorRedirectURL: "popupprocess.aspx");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "closeWin", "<script>window.close()</script>");
                }
                else if (Request.QueryString["error_message"] != null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "closeWin", "<script>alert('" + Request.QueryString["error_message"] + "'); window.close()</script>");
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "closeWin", "<script>window.close()</script>");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "closeWin", "<script>window.close()</script>");
            }

            
        }

        private void Page_Error(object sender, EventArgs e)
        {
            
        }
    }
}