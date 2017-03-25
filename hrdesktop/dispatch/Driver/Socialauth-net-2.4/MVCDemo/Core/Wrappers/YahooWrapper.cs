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
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Web;
using System.Xml.Linq;
using log4net;

namespace Brickred.SocialAuth.NET.Core.Wrappers
{
    public class YahooWrapper : Provider, IProvider
    {
        private OAuthStrategyBase _AuthenticationStrategy = null;
        #region IProvider Members

        //****** PROPERTIES
        private static readonly ILog logger = log4net.LogManager.GetLogger("YahooWrapper");
        public override PROVIDER_TYPE ProviderType { get { return PROVIDER_TYPE.YAHOO; } }
        public override string RequestTokenEndpoint { get { return "https://api.login.yahoo.com/oauth/v2/get_request_token"; } }
        string userloginendpoint = "https://api.login.yahoo.com/oauth/v2/request_auth";
        public override string UserLoginEndpoint { get { return userloginendpoint; } set { userloginendpoint = value; } }
        public override string AccessTokenEndpoint { get { return "https://api.login.yahoo.com/oauth/v2/get_token"; } }
        public override OAuthStrategyBase AuthenticationStrategy { get { return _AuthenticationStrategy ?? (_AuthenticationStrategy = new OAuth1_0Hybrid(this)); } }
        public override string ProfileEndpoint { get { return "http://social.yahooapis.com/v1/user/{0}/profile"; } }
        public override string ContactsEndpoint { get { return "http://social.yahooapis.com/v1/user/{0}/contacts"; } }
        public override SIGNATURE_TYPE SignatureMethod { get { return SIGNATURE_TYPE.HMACSHA1; } }
        public override TRANSPORT_METHOD TransportName { get { return TRANSPORT_METHOD.GET; } }
        public override string OpenIdDiscoveryEndpoint { get { return "http://open.login.yahooapis.com/openid20/www.yahoo.com/xrds"; } }
        public override string DefaultScope { get { return ""; } }
        public override bool IsScopeDefinedAtProvider { get { return true; } }



        //****** OPERATIONS
        public override UserProfile GetProfile()
        {
            Token token = ConnectionToken;
            UserProfile profile = new UserProfile(ProviderType);
            string response = "";

            //If token already has profile for this provider, we can return it to avoid a call
            if (token.Profile.IsSet)
                return token.Profile;

            try
            {
                logger.Debug("Executing Profile feed");
                Stream responseStream = AuthenticationStrategy.ExecuteFeed(
                    string.Format(ProfileEndpoint, token.ResponseCollection["xoauth_yahoo_guid"]), this, token, TRANSPORT_METHOD.GET).GetResponseStream();
                response = new StreamReader(responseStream).ReadToEnd();
            }
            catch
            {
                throw;
            }

            try
            {

                XDocument xDoc = XDocument.Parse(response);
                XNamespace xn = xDoc.Root.GetDefaultNamespace();
                profile.ID = xDoc.Root.Element(xn + "guid") != null ? xDoc.Root.Element(xn + "guid").Value : string.Empty;
                profile.FirstName = xDoc.Root.Element(xn + "givenName") != null ? xDoc.Root.Element(xn + "givenName").Value : string.Empty;
                profile.LastName = xDoc.Root.Element(xn + "familyName") != null ? xDoc.Root.Element(xn + "familyName").Value : string.Empty;
                profile.DateOfBirth = xDoc.Root.Element(xn + "birthdate") != null ? xDoc.Root.Element(xn + "birthdate").Value : "/";
                profile.DateOfBirth = xDoc.Root.Element(xn + "birthyear") != null ? "/" + xDoc.Root.Element(xn + "birthyear").Value : "/";
                profile.Country = xDoc.Root.Element(xn + "location") != null ? xDoc.Root.Element(xn + "location").Value : string.Empty;
                profile.ProfileURL = xDoc.Root.Element(xn + "profileUrl") != null ? xDoc.Root.Element(xn + "profileUrl").Value : string.Empty;
                profile.ProfilePictureURL = xDoc.Root.Element(xn + "image") != null ? xDoc.Root.Element(xn + "image").Element(xn + "imageUrl").Value : string.Empty;
                profile.Language = xDoc.Root.Element(xn + "lang") != null ? xDoc.Root.Element(xn + "lang").Value : string.Empty;
                if (xDoc.Root.Element(xn + "gender") != null)
                    profile.GenderType = Utility.ParseGender(xDoc.Root.Element(xn + "gender").Value);

                if (string.IsNullOrEmpty(profile.FirstName))
                    profile.FirstName = token.ResponseCollection["openid.ax.value.firstname"];
                if (string.IsNullOrEmpty(profile.FirstName))
                    profile.LastName = token.ResponseCollection["openid.ax.value.lastname"];
                profile.Email = token.ResponseCollection.Get("openid.ax.value.email");
                profile.Country = token.ResponseCollection.Get("openid.ax.value.country");
                profile.Language = token.ResponseCollection.Get("openid.ax.value.language");
                profile.IsSet = true;

                profile.IsSet = true;
                token.Profile = profile;
                logger.Info("Profile successfully received");
            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.ProfileParsingError(response), ex);
                throw new DataParsingException(ErrorMessages.ProfileParsingError(response), ex);
            }
            return profile;
        }
        public override List<Contact> GetContacts()
        {
            Token token = ConnectionToken;
            List<Contact> contacts = new List<Contact>();
            string response = "";
            try
            {
                logger.Debug("Executing contacts feed");
                Stream responseStream = AuthenticationStrategy.ExecuteFeed(
                    string.Format(ContactsEndpoint, token.ResponseCollection["xoauth_yahoo_guid"]), this, token, TRANSPORT_METHOD.GET).GetResponseStream();
                response = new StreamReader(responseStream).ReadToEnd();
            }
            catch { throw; }

            try
            {

                //Extract information from XML

                XDocument xdoc = XDocument.Parse(response);
                XNamespace xn = xdoc.Root.GetDefaultNamespace();
                XNamespace attxn = "http://www.yahooapis.com/v1/base.rng";

                xdoc.Root.Descendants(xdoc.Root.GetDefaultNamespace() + "contact").ToList().ForEach(x =>
                {
                    IEnumerable<XElement> contactFields = x.Elements(xn + "fields").ToList();
                    foreach (var field in contactFields)
                    {
                        Contact contact = new Contact();

                        if (field.Attribute(attxn + "uri").Value.Contains("/yahooid/"))
                        {
                            //contact.Name = field.Element(xn + "value").Value;
                            //contact.Email = field.Element(xn + "value").Value + "@yahoo.com";
                        }
                        else if (field.Attribute(attxn + "uri").Value.Contains("/name/"))
                        {
                            //Contact c = contacts.Last<Contact>();
                            //c.Name = field.Element(xn + "value").Element(xn + "givenName").Value + " " + field.Element(xn + "value").Element(xn + "familyName").Value;
                            //contacts[contacts.Count - 1] = c;
                            //continue;
                        }
                        else if (field.Attribute(attxn + "uri").Value.Contains("/email/"))
                        {
                            contact.Name = field.Element(xn + "value").Value.Replace("@yahoo.com", "");
                            contact.Email = field.Element(xn + "value").Value;
                        }
                        if (!string.IsNullOrEmpty(contact.Name) && !contacts.Exists(y => y.Email == contact.Email))
                            contacts.Add(contact);
                    }
                });
                logger.Info("Contacts successfully received");
            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.ContactsParsingError(response), ex);
                throw new DataParsingException(ErrorMessages.ContactsParsingError(response), ex);
            }
            return contacts;
        }
        public override WebResponse ExecuteFeed(string feedUrl, TRANSPORT_METHOD transportMethod)
        {
            return AuthenticationStrategy.ExecuteFeed(feedUrl, this, ConnectionToken, transportMethod);
        }
        public static WebResponse ExecuteFeed(string feedUrl, string accessToken, string tokenSecret, TRANSPORT_METHOD transportMethod)
        {
            YahooWrapper wrapper = new YahooWrapper();
            return wrapper.AuthenticationStrategy.ExecuteFeed(feedUrl, wrapper, new Token() { AccessToken = accessToken, TokenSecret = tokenSecret }, transportMethod);
        }

        #endregion
    }

}
