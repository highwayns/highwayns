using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Brickred.SocialAuth.NET.Core;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.Security.Principal;

namespace Brickred.SocialAuth.NET.Demo
{

    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
           
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        public void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs args)
        {
            
        }

        protected void Application_PostAcquireRequestState(object sender, EventArgs e)
        {
            //Uncomment following line if logging with log4net 
            //log4net.ThreadContext.Properties["SessionID"] = Session.SessionID;
        }
    }
}