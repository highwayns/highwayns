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
using System.Collections.Specialized;
using System.Web;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.Net;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using log4net;
namespace Brickred.SocialAuth.NET.Core.Wrappers
{
    public class MSNWrapper : Provider, IProvider
    {
        private OAuthStrategyBase _AuthenticationStrategy = null;
        #region IProvider Members

        //****** PROPERTIES
        private static readonly ILog logger = log4net.LogManager.GetLogger("MSNWrapper");
        public override PROVIDER_TYPE ProviderType { get { return PROVIDER_TYPE.MSN; } }
        public override string UserLoginEndpoint { get { return "https://oauth.live.com/authorize"; } set { } }
        public override string AccessTokenEndpoint { get { return "https://oauth.live.com/token"; } }
        public override OAuthStrategyBase AuthenticationStrategy
        {
            get
            {
                if (_AuthenticationStrategy == null)
                {
                    OAuth2_0server strategy = new OAuth2_0server(this);
                    strategy.AccessTokenRequestType = TRANSPORT_METHOD.POST;
                    strategy.BeforeDirectingUserToServiceProvider += (x) =>
                                    {
                                        if (string.IsNullOrEmpty(GetScope()))
                                            x["scope"] = "wl.signin";
                                    };
                    _AuthenticationStrategy = strategy;
                }
                return _AuthenticationStrategy;
            }
        }
        public override string ProfileEndpoint { get { return "https://apis.live.net/v5.0/me"; } }
        public override string ContactsEndpoint { get { return "https://apis.live.net/v5.0/me/contacts"; } }
        public override SIGNATURE_TYPE SignatureMethod { get { throw new NotImplementedException(); } }
        public override string ProfilePictureEndpoint { get { return "https://apis.live.net/v5.0/me/picture"; } }
        public override TRANSPORT_METHOD TransportName { get { return TRANSPORT_METHOD.POST; } }



        public override string DefaultScope { get { return "wl.basic,wl.emails,wl.birthday"; } }



        //****** OPERATIONS
        public override UserProfile GetProfile()
        {

            //If token already has profile for this provider, we can return it to avoid a call
            Token token = ConnectionToken;
            if (token.Profile.IsSet)
            {
                logger.Debug("Profile successfully returned from session");
                return token.Profile;
            }

            //Fetch Profile
            OAuthStrategyBase strategy = AuthenticationStrategy;
            string response = "";

            try
            {
                logger.Debug("Executing profile feed");
                Stream responseStream = strategy.ExecuteFeed(ProfileEndpoint, this, token, TRANSPORT_METHOD.GET).GetResponseStream();
                response = new StreamReader(responseStream).ReadToEnd();
            }
            catch
            {
                throw;
            }

            try
            {
                JObject profileJson = JObject.Parse(response);
                token.Profile.ID = profileJson.Get("id");
                token.Profile.FirstName = profileJson.Get("first_name");
                token.Profile.LastName = profileJson.Get("last_name");
                token.Profile.Country = profileJson.Get("Location");
                token.Profile.ProfilePictureURL = profileJson.Get("ThumbnailImageLink");
                token.Profile.Email = profileJson.Get("emails.account");
                token.Profile.GenderType = Utility.ParseGender(profileJson.Get("gender"));
                if (!string.IsNullOrEmpty(ProfileEndpoint))
                {
                    token.Profile.ProfilePictureURL = strategy.ExecuteFeed(ProfilePictureEndpoint, this, token, TRANSPORT_METHOD.GET).ResponseUri.AbsoluteUri.Replace("\"", "");
                }
                token.Profile.IsSet = true;
                logger.Info("Profile successfully received");
                return token.Profile;
            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.ProfileParsingError(response), ex);
                throw new DataParsingException(ErrorMessages.ProfileParsingError(response), ex);
            }

        }
        public override List<Contact> GetContacts()
        {
            Token token = ConnectionToken;
            List<Contact> contacts = new List<Contact>();
            string response = "";
            try
            {
                logger.Debug("Executing contacts feed");
                Stream responseStream = AuthenticationStrategy.ExecuteFeed(ContactsEndpoint + "?access_token=" + token.AccessToken, null, token, TRANSPORT_METHOD.GET).GetResponseStream();
                response = new StreamReader(responseStream).ReadToEnd();
            }
            catch { throw; }

            try
            {
                JObject contactsJson = JObject.Parse(response);
                contactsJson.SelectToken("data").ToList().ForEach(x =>
                {
                    contacts.Add(new Contact()
                    {
                        ID = x.SelectToken("id").ToString(),
                        Name = getName(x)
                    });
                }
                    );
                logger.Info("Contacts successfully received");
                return contacts;
            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.ContactsParsingError(response), ex);
                throw new DataParsingException(ErrorMessages.ContactsParsingError(response), ex);
            }
        }

        string getName(JToken contactJT)
        {
            string firstName = "";
            string lastName = "";
            if (contactJT.SelectToken("first_name") != null)
                firstName = contactJT.SelectToken("first_name").ToString().Replace("\"", "");

            if (contactJT.SelectToken("last_name") != null)
                lastName = " " + contactJT.SelectToken("last_name").ToString().Replace("\"", "");

            if (firstName == "" && lastName == "")
                return "";
            else if (firstName == "" & lastName != "")
                return lastName.Substring(1);
            else return firstName + lastName;
        }

        public override WebResponse ExecuteFeed(string feedUrl, TRANSPORT_METHOD transportMethod)
        {
            logger.Debug("Calling execution of " + feedUrl);
            return AuthenticationStrategy.ExecuteFeed(feedUrl, this, ConnectionToken, transportMethod);
        }
        public static WebResponse ExecuteFeed(string feedUrl, string accessToken, TRANSPORT_METHOD transportMethod)
        {
            MSNWrapper msn = new MSNWrapper();
            return msn.AuthenticationStrategy.ExecuteFeed(feedUrl, msn, new Token() { AccessToken = accessToken }, transportMethod);
        }

        #endregion
    }
}
