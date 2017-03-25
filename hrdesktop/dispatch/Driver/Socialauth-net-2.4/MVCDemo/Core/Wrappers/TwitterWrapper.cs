using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using Newtonsoft.Json.Linq;
using log4net;

namespace Brickred.SocialAuth.NET.Core.Wrappers
{
    public class TwitterWrapper : Provider, IProvider
    {
        private OAuthStrategyBase _AuthenticationStrategy = null;
        #region IProvider Members

        //****** PROPERTIES
        private static readonly ILog logger = log4net.LogManager.GetLogger("TwitterWrapper");
        public override PROVIDER_TYPE ProviderType { get { return PROVIDER_TYPE.TWITTER; } }
        public override string RequestTokenEndpoint { get { return "https://api.twitter.com/oauth/request_token"; } }
        public override string UserLoginEndpoint { get { return "https://api.twitter.com/oauth/authenticate"; } set { } }
        public override string AccessTokenEndpoint { get { return "https://api.twitter.com/oauth/access_token"; } }
        public override OAuthStrategyBase AuthenticationStrategy
        {
            get
            {
                if(_AuthenticationStrategy == null)
                {
                    var strategy = new OAuth1_0a(this);
                    strategy.AfterGettingAccessToken += ProcessAccessToken;
                    _AuthenticationStrategy = strategy;
                }
                return _AuthenticationStrategy;
            }
        }
        public override string ProfileEndpoint { get { return "http://api.twitter.com/1.1/users/show.json"; } }
        public override string ContactsEndpoint { get { return "http://api.twitter.com/1.1/friends/ids.json?screen_name={0}&cursor=-1"; } }
        public override SIGNATURE_TYPE SignatureMethod { get { return SIGNATURE_TYPE.HMACSHA1; } }
        public override TRANSPORT_METHOD TransportName { get { return TRANSPORT_METHOD.POST; } }

        public override string DefaultScope { get { return ""; } }

        public void ProcessAccessToken(QueryParameters responseCollection, Token connectionToken)
        {
            if (!string.IsNullOrEmpty(connectionToken.AccessToken))
            {
                connectionToken.Profile.DisplayName = connectionToken.ResponseCollection["screen_name"];
                connectionToken.Profile.ID = connectionToken.ResponseCollection["user_id"];
            }
        }

        bool isAuthenticated = false;
        public void OnAuthenticationCompleting(bool isSuccess, Token connectionToken)
        {
            isAuthenticated = isSuccess;
            connectionToken.Profile.DisplayName = connectionToken.ResponseCollection["screen_name"];
            connectionToken.Profile.ID = connectionToken.ResponseCollection["user_id"];
        }


        //****** OPERATIONS
        public override UserProfile GetProfile()
        {
            Token token = SocialAuthUser.GetCurrentUser().GetConnection(this.ProviderType).GetConnectionToken();
            UserProfile profile = new UserProfile(ProviderType);
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
                string profileUrl = ProfileEndpoint + "?user_id=" + token.Profile.ID;
                Stream responseStream = AuthenticationStrategy.ExecuteFeed(profileUrl, this, token, TRANSPORT_METHOD.GET).GetResponseStream();
                response = new StreamReader(responseStream).ReadToEnd();

            }
            catch
            {
                throw;
            }

            try
            {



                JObject profileJson = JObject.Parse(response);
                profile.ID = profileJson.Get("id_str");
                profile.FirstName = profileJson.Get("name");
                profile.Country = profileJson.Get("location");
                profile.DisplayName = profileJson.Get("screen_name");
                //profile.Email =  not provided
                profile.Language = profileJson.Get("lang");
                profile.ProfilePictureURL = profileJson.Get("profile_image_url");
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
        public override List<BusinessObjects.Contact> GetContacts()
        {
            List<Contact> contacts = new List<Contact>();
            string response = "";
            List<string> sets = new List<string>();

            Token token = SocialAuthUser.GetCurrentUser().GetConnection(this.ProviderType).GetConnectionToken();
            string friendsUrl = string.Format(ContactsEndpoint, token.Profile.Email);
            try
            {
                logger.Debug("Executing contacts feed");
                Stream responseStream = AuthenticationStrategy.ExecuteFeed(friendsUrl, this, token, TRANSPORT_METHOD.GET).GetResponseStream();
                response = new StreamReader(responseStream).ReadToEnd();
            }
            catch { throw; }
            try
            {
                string friendIDs = "";
                var friends = JObject.Parse(response).SelectToken("ids").Children().ToList();
                friendIDs = "";
                foreach (var s in friends)
                    friendIDs += (s.ToString() + ",");

                char[] arr = friendIDs.ToArray<char>();
                var iEnumerator = arr.GetEnumerator();
                int counter = 0;
                string temp = "";
                while (iEnumerator.MoveNext())
                {
                    if (iEnumerator.Current.ToString() == ",")
                        counter += 1;
                    if (counter == 100)
                    {
                        sets.Add(temp);
                        temp = "";
                        counter = 0;
                        continue;
                    }
                    temp += iEnumerator.Current;
                }
                if (temp != "")
                    sets.Add(temp);
            }
            catch (Exception ex)
            {
                logger.Error(ErrorMessages.ContactsParsingError(response), ex);
                throw new DataParsingException(ErrorMessages.ContactsParsingError(response), ex);
            }
            foreach (string set in sets)
            {

                contacts.AddRange(Friends(set, token));
            }
            logger.Info("Contacts successfully received");
            return contacts;
        }
        public override WebResponse ExecuteFeed(string feedUrl, TRANSPORT_METHOD transportMethod)
        {
            logger.Debug("Calling execution of " + feedUrl);
            return AuthenticationStrategy.ExecuteFeed(feedUrl, this, ConnectionToken, transportMethod);
        }
        public static WebResponse ExecuteFeed(string feedUrl, string accessToken, string tokenSecret, TRANSPORT_METHOD transportMethod)
        {
            TwitterWrapper wrapper = new TwitterWrapper();
            return wrapper.AuthenticationStrategy.ExecuteFeed(feedUrl, wrapper, new Token() { AccessToken = accessToken, TokenSecret = tokenSecret }, transportMethod);
        }
        private List<Contact> Friends(string friendUserIDs, Token token)
        {
            string lookupUrl = "http://api.twitter.com/1/users/lookup.json?user_id=" + friendUserIDs;
            OAuthHelper helper = new OAuthHelper();
            string friendsData = "";
            try
            {
                Stream responseStream = AuthenticationStrategy.ExecuteFeed(lookupUrl, this, token, TRANSPORT_METHOD.GET).GetResponseStream();
                friendsData = new StreamReader(responseStream).ReadToEnd();
            }
            catch { throw; }

            List<Contact> friends = new List<Contact>();

            try
            {
                JArray j = JArray.Parse(friendsData);
                j.ToList().ForEach(f => friends.Add(
                    new Contact()
                        {
                            Name = (string)f["name"],
                            ID = (string)f["id_str"],
                            ProfileURL = "http://twitter.com/#!/" + (string)f["screen_name"]
                        }));
            }
            catch
            {
                throw;
            }
            return friends.ToList<Contact>();

        }

        #endregion
    }
}
