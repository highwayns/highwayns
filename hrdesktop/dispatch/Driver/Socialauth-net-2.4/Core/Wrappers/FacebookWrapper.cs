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
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using log4net;


namespace Brickred.SocialAuth.NET.Core.Wrappers
{
    public class FacebookWrapper : Provider, IProvider
    {
        #region IProvider Members

        private OAuthStrategyBase _AuthenticationStrategy = null;

        //****** PROPERTIES
        private static readonly ILog logger = log4net.LogManager.GetLogger("FacebookWrapper");
        public override PROVIDER_TYPE ProviderType { get { return PROVIDER_TYPE.FACEBOOK; } }
        public override string UserLoginEndpoint { get { return "https://www.facebook.com/dialog/oauth"; } set { } }
        public override string AccessTokenEndpoint { get { return "https://graph.facebook.com:443/oauth/access_token"; } }
        public override OAuthStrategyBase AuthenticationStrategy
        {
            get { return _AuthenticationStrategy ?? (_AuthenticationStrategy = new OAuth2_0server(this)); }
        }
        public override string ProfileEndpoint { get { return "https://graph.facebook.com/me"; } }

        public override string ContactsEndpoint { get { return "https://graph.facebook.com/me/friends"; } }
        public override string ProfilePictureEndpoint { get { return "https://graph.facebook.com/me/picture"; } }
        public override SIGNATURE_TYPE SignatureMethod { get { throw new NotImplementedException(); } }
        public override TRANSPORT_METHOD TransportName { get { return TRANSPORT_METHOD.POST; } }

        public override string DefaultScope { get { return "user_birthday,user_location,email"; } }


        //****** OPERATIONS
        public override UserProfile GetProfile()
        {
            Token token = ConnectionToken;
            OAuthStrategyBase strategy = AuthenticationStrategy;
            string response = "";


            //If token already has profile for this provider, we can return it to avoid a call
            if (token.Profile.IsSet)
            {
                logger.Debug("Profile successfully returned from session");
                return token.Profile;
            }

            try
            {
                logger.Debug("Executing Profile feed");
                Stream responseStream = strategy.ExecuteFeed(ProfileEndpoint, this, token, TRANSPORT_METHOD.GET).GetResponseStream();
                response = new StreamReader(responseStream).ReadToEnd();
            }
            catch
            {
                throw;
            }

            try
            {

                JObject jsonObject = JObject.Parse(response);
                token.Profile.ID = jsonObject.Get("id");
                token.Profile.FirstName = jsonObject.Get("first_name");
                token.Profile.LastName = jsonObject.Get("last_name");
                token.Profile.Username = jsonObject.Get("username");
                token.Profile.DisplayName = token.Profile.FullName;
                string[] locale = jsonObject.Get("locale").Split(new char[] { '_' });
                if (locale.Length > 0)
                {
                    token.Profile.Language = locale[0];
                    token.Profile.Country = locale[1];
                }
                token.Profile.ProfileURL = jsonObject.Get("link");
                token.Profile.Email = HttpUtility.UrlDecode(jsonObject.Get("email"));
                if (!string.IsNullOrEmpty(jsonObject.Get("birthday")))
                {
                    string[] dt = jsonObject.Get("birthday").Split(new char[] { '/' });
                    token.Profile.DateOfBirth = dt[1] + "/" + dt[0] + "/" + dt[2];
                }
                token.Profile.GenderType = Utility.ParseGender(jsonObject.Get("gender"));
                //get profile picture
                if (!string.IsNullOrEmpty(ProfilePictureEndpoint))
                {
                    token.Profile.ProfilePictureURL = strategy.ExecuteFeed(ProfilePictureEndpoint, this, token, TRANSPORT_METHOD.GET).ResponseUri.AbsoluteUri.Replace("\"", "");
                }
                token.Profile.IsSet = true;
                logger.Info("Profile successfully received");
                //Session token updated with profile
            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.ProfileParsingError(response), ex);
                throw new DataParsingException(ErrorMessages.ProfileParsingError(response), ex);
            }

            return token.Profile;



        }
        public override List<Contact> GetContacts()
        {
            Token token = ConnectionToken;
            OAuthStrategyBase strategy = this.AuthenticationStrategy;
            Stream responseStream = null;
            string response = "";
            try
            {
                logger.Debug("Executing contacts feed");
                responseStream = strategy.ExecuteFeed(ContactsEndpoint, this, token, TRANSPORT_METHOD.GET).GetResponseStream();
                response = new StreamReader(responseStream).ReadToEnd();
            }
            catch
            {
                throw;
            }

            try
            {
                JObject jsonObject = JObject.Parse(response);
                var friends = from f in jsonObject["data"].Children()
                              select new Contact
                              {
                                  Name = (string)f["name"],
                                  ID = (string)f["id"],
                                  ProfileURL = "http://www.facebook.com/profile.php?id=" + (string)f["id"],
                                  ProfilePictureURL = "https://graph.facebook.com/" + (string)f["id"] + "/picture"
                              };
                logger.Info("Contacts successfully received");
                return friends.ToList<Contact>();
            }

            catch (Exception ex)
            {
                logger.Error(ErrorMessages.ContactsParsingError(response), ex);
                throw new DataParsingException(ErrorMessages.ContactsParsingError(response), ex);
            }

        }
        public override WebResponse ExecuteFeed(string feedUrl, TRANSPORT_METHOD transportMethod)
        {
            logger.Debug("Calling execution of " + feedUrl);
            return AuthenticationStrategy.ExecuteFeed(feedUrl, this, ConnectionToken, transportMethod);
        }

        #endregion
    }
}
