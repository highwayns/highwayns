/*
===========================================================================
Copyright (c) 2010 BrickRed Technologies Limited

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sub-license, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
===========================================================================

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.Web.SessionState;
using log4net;
using System.Web.Security;
using System.Security.Principal;
using System.IO;
using System.Web.UI;

namespace Brickred.SocialAuth.NET.Core
{
    class CallbackHandler : IHttpHandler, IRequiresSessionState
    {
        #region HttpHandler Implementation

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            //Call for Logout
            if (current.Request.Url.ToString().IndexOf("logout.sauth") > -1)
            { SocialAuthUser.Disconnect(); }


            //Call for login (likely from HTML clients) with provider in parameter "p"
            else if (HttpContext.Current.Request.RawUrl.ToLower().Contains("login.sauth"))
            {
                string returnUrl = "";
                if (current.Request["returnUrl"] != null)
                    returnUrl = current.Request["returnUrl"];
                
                if (current.Request["p"] != null)
                {
                    SocialAuthUser.Connect((PROVIDER_TYPE)Enum.Parse(typeof(PROVIDER_TYPE), HttpContext.Current.Request["p"].ToUpper()), returnURL:returnUrl);
                }
            }

            //call to display login Form
            else if (HttpContext.Current.Request.RawUrl.ToLower().Contains("loginform.sauth"))
            {
                RenderHtml();
                HttpContext.Current.Response.End();
            }

            //call to process response received from Providers
            else if (HttpContext.Current.Request.RawUrl.ToLower().Contains("validate.sauth"))
            {
                SocialAuthUser.LoginCallback(HttpContext.Current.Request.Url.ToString());
            }
        }


        #endregion

        #region Helper Methods

        private static HttpContext current
        {
            get { return HttpContext.Current; }
        }

        private void RenderHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<script>");
            sb.Append("function Login(providerName){window.location.href='login.sauth?p=' + providerName}");
            sb.Append("</script>");
            sb.AppendLine(@"<style>");
            sb.AppendLine("input");
            sb.AppendLine("{");
            sb.AppendLine("font-family:Verdana;");
            sb.AppendLine("font-size:14px;");
            sb.AppendLine("width:200;");
            sb.AppendLine("height:50;");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine(".formContainer");
            sb.AppendLine("{");
            sb.AppendLine("font-family:Verdana;");
            sb.AppendLine("font-size:15px;");
            sb.AppendLine("background:purple;");
            sb.AppendLine("color:White;");
            sb.AppendLine("padding-bottom:10px;");
            sb.AppendLine("border: solid 7px #FF112F;");
            sb.AppendLine("text-align:center;");
            sb.AppendLine("width:700;");
            sb.AppendLine("}");
            sb.AppendLine("</style>");

            sb.Append("</head>");
            sb.Append(@"<body><form name=""form1"">");
            sb.Append(@"<div align=""center""><div class=formContainer>");
            sb.AppendLine(@"<div style=""width:inherit;background-color:#FF112F;height:30px;""><b>Please select a provider to login</b></div><br/>	");

            Brickred.SocialAuth.NET.Core.ProviderFactory.Providers.ForEach(p =>
                 sb.Append(@"<input type=""button"" value=""" + p.ProviderType.ToString().ToLower() + @""" OnClick=""Login('" + p.ProviderType.ToString().ToLower() + @"')""/><br/>"));
            sb.Append("</div>");
            sb.Append("</div></form></body>");
            sb.Append("</html>");

            HttpContext.Current.Response.Write(sb.ToString());

        }


        #endregion



    }
}
