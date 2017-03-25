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
using System.Web.Security;
using System.Web.SessionState;
using System.Security.Principal;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.Text.RegularExpressions;

namespace Brickred.SocialAuth.NET.Core
{
    class SocialAuthHttpModule : IHttpModule, IReadOnlySessionState
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        //Hook our module to httprequest pipeline
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
            context.PreRequestHandlerExecute += new EventHandler(context_PreRequestHandlerExecute);

        }

        private void context_AuthenticateRequest(object sender, EventArgs e)
        {
            ///*************************
            // * If Request is of type .sauth OR any type as specified in Config, allow and skip.
            // * If Request is of LoginURL, skip
            // * OTHERWISE:::::::::::::::::::::
            // * <<<<IF USER IS NOT LOGGED IN>>>
            // * If AuthenticationOption = SocialAuth
            // *          Redirect in Priority - ConfigurationLoginURL,  "LoginForm.sauth"
            // * If AuthenticationOption = FormsAuthentication
            // *          Don't do anything. Let .NET handle it as per user's setting in Web.Config
            // * If AuthenticationOption = Everything Custom
            // *          Don't do anything. User will put checking code on every page himself.
            // * **********************/

            //AUTHENTICATION_OPTION option = Utility.GetAuthenticationOption();


            //if (option == AUTHENTICATION_OPTION.SOCIALAUTH_SECURITY_CUSTOM_SCREEN || option == AUTHENTICATION_OPTION.SOCIALAUTH_SECURITY_SOCIALAUTH_SCREEN)
            //{
            //    //block any .aspx page. Rest all is allowed.
            //    //TODO: Better Implementation of this
            //    if (VirtualPathUtility.GetExtension(HttpContext.Current.Request.RawUrl) != ".aspx")
            //        return;

            //    //If requested page is login URL only, allow it
            //    string currentUrl = HttpContext.Current.Request.Url.AbsolutePath;
            //    string loginurl = Utility.GetSocialAuthConfiguration().Authentication.LoginUrl;
            //    loginurl = string.IsNullOrEmpty(loginurl) ? "socialauth/loginform.sauth" : loginurl;
            //    if (currentUrl.EndsWith(loginurl))
            //        return;

            //    //If Url is pointing to a .aspx page, authorize it!
            //    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            //    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //    if (cookie != null)
            //    {
            //        HttpContext.Current.User = new GenericPrincipal(new FormsIdentity(FormsAuthentication.Decrypt(cookie.Value)), null);
            //    }
            //    else
            //    {
            //        //User is not logged in
            //        SocialAuthUser.RedirectToLoginPage();
            //    }
            //}

        }


        protected void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            /*************************
     * If Request is of type .sauth OR any type as specified in Config, allow and skip.
     * If Request is of LoginURL, skip
     * OTHERWISE:::::::::::::::::::::
     * <<<<IF USER IS NOT LOGGED IN>>>
     * If AuthenticationOption = SocialAuth
     *          Redirect in Priority - ConfigurationLoginURL,  "LoginForm.sauth"
     * If AuthenticationOption = FormsAuthentication
     *          Don't do anything. Let .NET handle it as per user's setting in Web.Config
     * If AuthenticationOption = Everything Custom
     *          Don't do anything. User will put checking code on every page himself.
     * **********************/

            AUTHENTICATION_OPTION option = Utility.GetAuthenticationOption();


            if (option == AUTHENTICATION_OPTION.SOCIALAUTH_SECURITY_CUSTOM_SCREEN || option == AUTHENTICATION_OPTION.SOCIALAUTH_SECURITY_SOCIALAUTH_SCREEN)
            {
                //block any .aspx page. Rest all is allowed.
                //TODO: Better Implementation of this
                string requestUrlExtension = VirtualPathUtility.GetExtension(HttpContext.Current.Request.RawUrl);
                string urlWithoutParameters = (new Uri(HttpContext.Current.Request.Url.ToString()).GetLeftPart(UriPartial.Path)).ToLower();
                string host = (new Uri(HttpContext.Current.Request.GetBaseURL())).ToString().ToLower();
                if (requestUrlExtension != ".aspx" && !string.IsNullOrEmpty(requestUrlExtension))
                    return;
                //Check for excludes
                //Allowed Folders
                if (!string.IsNullOrEmpty(Utility.GetSocialAuthConfiguration().Allow.Folders))
                {
                    string[] foldersToExclude = Utility.GetSocialAuthConfiguration().Allow.Folders.Split(new char[] { '|' });
                    foreach (string folderName in foldersToExclude)
                        if (urlWithoutParameters.Contains(host + (host.EndsWith("/") ? "" : "/") + folderName))
                            return;
                }
                
                //Allowed Files
                if (!string.IsNullOrEmpty(Utility.GetSocialAuthConfiguration().Allow.Files))
                {

                    string[] filesToExclude = Utility.GetSocialAuthConfiguration().Allow.Files.Split(new char[] { '|' });
                    foreach (string fileName in filesToExclude)
                        if (Regex.IsMatch(urlWithoutParameters, "/" + fileName.ToLower() + "$"))
                            return;
                }



                //If requested page is login URL only, allow it
                string currentUrl = HttpContext.Current.Request.Url.AbsolutePath;
                string loginurl = Utility.GetSocialAuthConfiguration().Authentication.LoginUrl;
                loginurl = string.IsNullOrEmpty(loginurl) ? "socialauth/loginform.sauth" : loginurl;
                if (currentUrl.ToLower().EndsWith(loginurl.ToLower()))
                    return;

                //If Url is pointing to a .aspx page, authorize it!
                HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                if (cookie != null)
                {
                    HttpContext.Current.User = new GenericPrincipal(new FormsIdentity(FormsAuthentication.Decrypt(cookie.Value)), null);
                }
                else
                {
                    //User is not logged in
                    SocialAuthUser.RedirectToLoginPage();
                }

                if (HttpContext.Current.Session != null)
                    if (SocialAuthUser.IsLoggedIn() && HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] == null)
                    {
                        FormsAuthenticationTicket ticket =
                    new FormsAuthenticationTicket(SessionManager.GetUserSessionGUID().ToString(), false, HttpContext.Current.Session.Timeout);

                        string EncryptedTicket = FormsAuthentication.Encrypt(ticket);
                        cookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptedTicket);
                        HttpContext.Current.Response.Cookies.Add(cookie);

                    }
            }

            //Often, Forms Cookie persist even where there is no connection. To avoid that!!
            if (HttpContext.Current.Session != null)
                if (SessionManager.ConnectionsCount == 0)
                    if (HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] != null && Utility.GetAuthenticationOption() != AUTHENTICATION_OPTION.FORMS_AUTHENTICATION)
                        if (SessionManager.GetUserSessionGUID().ToString() != FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name)
                            SocialAuthUser.Disconnect();

            if (HttpContext.Current.ApplicationInstance.IsSTSaware())
                if (HttpContext.Current.Session != null)
                    if (SocialAuthUser.IsLoggedIn())
                        if (SocialAuthUser.GetCurrentUser().GetProfile() != null)
                            SocialAuthUser.SetClaims();
        }
    }
}
