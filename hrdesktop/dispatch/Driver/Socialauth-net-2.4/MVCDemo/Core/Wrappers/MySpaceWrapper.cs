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
using System.Net;
using System.IO;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.Collections.Specialized;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using log4net;

namespace Brickred.SocialAuth.NET.Core.Wrappers
{
    public class MySpaceWrapper : Provider, IProvider
    {
        private OAuthStrategyBase _AuthenticationStrategy = null;
        #region IProvider Members

        //****** PROPERTIES
        private static readonly ILog logger = log4net.LogManager.GetLogger("MySpaceWrapper");
        public override PROVIDER_TYPE ProviderType { get { return PROVIDER_TYPE.MYSPACE; } }
        public override string RequestTokenEndpoint { get { return "http://api.myspace.com/request_token"; } }
        public override string UserLoginEndpoint { get { return "http://api.myspace.com/authorize"; } set { } }
        public override string AccessTokenEndpoint { get { return "http://api.myspace.com/access_token"; } }
        public override OAuthStrategyBase AuthenticationStrategy
        {
            get
            {
                if (_AuthenticationStrategy == null)
                {
                    var strategy = new OAuth1_0a(this);
                    strategy.BeforeDirectingUserToServiceProvider +=
                        (x) =>
                        x.Add(new QueryParameter("oauth_callback",
                                                 Utility.UrlEncode(ConnectionToken.ProviderCallbackUrl)));
                    _AuthenticationStrategy = strategy;
                }
                return _AuthenticationStrategy;
            }
        }
        public override string ProfileEndpoint { get { return "http://api.myspace.com/1.0/people/@me/@self"; } }
        public override string ContactsEndpoint { get { return "http://api.myspace.com/1.0/people/@me/@all"; } }
        public override SIGNATURE_TYPE SignatureMethod { get { return SIGNATURE_TYPE.HMACSHA1; } }
        public override TRANSPORT_METHOD TransportName { get { return TRANSPORT_METHOD.GET; } }
        public override string DefaultScope { get { return ""; } }



        //****** OPERATIONS
        public override UserProfile GetProfile()
        {
            Token token = ConnectionToken;
            string response = "";
            //If token already has profile for this provider, we can return it to avoid a call
            if (token.Profile.IsSet)
                return token.Profile;
            try
            {
                logger.Debug("Executing Profile feed");
                Stream responseStream = AuthenticationStrategy.ExecuteFeed(ProfileEndpoint, this, token, TRANSPORT_METHOD.GET).GetResponseStream();
                response = new StreamReader(responseStream).ReadToEnd();
            }
            catch
            {
                throw;
            }

            try
            {
                JObject profileJson = JObject.Parse(response);
                token.Profile.ID = profileJson.Get("person.id");
                token.Profile.FirstName = profileJson.Get("person.name.givenName");
                token.Profile.LastName = profileJson.Get("person.name.familyName");
                token.Profile.Country = profileJson.Get("person.location");
                token.Profile.Language = profileJson.Get("person.lang");
                token.Profile.ProfilePictureURL = profileJson.Get("person.thumbnailUrl");
                token.Profile.IsSet = true;
                logger.Info("Profile successfully received");
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
            List<Contact> contacts = new List<Contact>();
            string response = "";
            try
            {
                logger.Debug("Executing contacts feed");
                Stream responseStream = AuthenticationStrategy.ExecuteFeed(ContactsEndpoint, this, token, TRANSPORT_METHOD.GET).GetResponseStream();
                response = new StreamReader(responseStream).ReadToEnd();
            }
            catch { throw; }
            try
            {
                JArray contactsJson = JArray.Parse(JObject.Parse(response).SelectToken("entry").ToString());
                if (contactsJson.Count > 0)
                    contactsJson.ToList().ForEach(person =>
                                   contacts.Add(new Contact()
                                   {
                                       ID = person.SelectToken("person.id") != null ? person.SelectToken("person.id").ToString().Replace("\"", "") : "",
                                       ProfileURL = person.SelectToken("person.profileUrl").ToString().Replace("\"", ""),
                                       Name = person.SelectToken("person.name.givenName") != null ? person.SelectToken("person.name.givenName").ToString() : "" + " " + person.SelectToken("person.name.familyName") != null ? person.SelectToken("person.name.familyName").ToString().Replace("\"", "") : ""

                                   }));
                logger.Info("Contacts successfully received");
            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.ContactsParsingError(response), ex);
                throw new DataParsingException(ErrorMessages.ContactsParsingError(response), ex);
            }
            return contacts.ToList();
        }
        public override WebResponse ExecuteFeed(string feedUrl, TRANSPORT_METHOD transportMethod)
        {
            return AuthenticationStrategy.ExecuteFeed(feedUrl, this, SocialAuthUser.GetCurrentUser().GetConnection(ProviderType).GetConnectionToken(), transportMethod);
        }
        public static WebResponse ExecuteFeed(string feedUrl, string accessToken, string tokenSecret, TRANSPORT_METHOD transportMethod)
        {
            MySpaceWrapper wrapper = new MySpaceWrapper();
            return wrapper.AuthenticationStrategy.ExecuteFeed(feedUrl, wrapper, new Token() { AccessToken = accessToken, TokenSecret = tokenSecret }, transportMethod);
        }

        #endregion
    }
}
