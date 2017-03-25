using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.Net;

namespace Brickred.SocialAuth.NET.Core
{
    public class SocialAuthManager
    {

        private SocialAuthConfiguration socialAuthConfiguration;

        private readonly Dictionary<PROVIDER_TYPE, Token> accessGrant = new Dictionary<PROVIDER_TYPE, Token>();

  
        #region Constructors & Configuration

        public SocialAuthManager()
        {

        }

        public SocialAuthManager(SocialAuthConfiguration configuration)
        {
            this.socialAuthConfiguration = configuration;
        }

        public void SetConfiguration(SocialAuthConfiguration configuration)
        {
            this.socialAuthConfiguration = configuration;
        }

        public SocialAuthConfiguration GetConfiguration()
        {
            return this.socialAuthConfiguration;
        }

        #endregion

      
        #region Methods specific to Access Grant Token

        /// <summary>
        /// Is there an established connection with specified provider
        /// </summary>
        /// <param name="providerType">Provider Type</param>
        /// <returns></returns>
        public bool IsConnectedWith(PROVIDER_TYPE providerType)
        {
            return accessGrant.ContainsKey(providerType);
        }

        /// <summary>
        /// Returns connection Token of specified provider
        /// </summary>
        /// <param name="providerType">Provider Type</param>
        /// <returns></returns>
        public Token GetConnectionToken(PROVIDER_TYPE providerType)
        {
            return accessGrant[providerType];
        }

        /// <summary>
        /// Allow user to add a connection token. Useful, when user is using a separate persistence for tokens.
        /// </summary>
        /// <param name="connectionToken"></param>
        /// <param name="overrideExisting"></param>
        public void LoadConnection(Token connectionToken, bool overrideExisting = false)
        {
            if (IsConnectedWith(connectionToken.Provider) || overrideExisting)
                accessGrant[connectionToken.Provider] = connectionToken;
            else
                throw new Exception("There is an already established connection with specified provider. If you wish to replace it, set the override argument to true");
        }

        /// <summary>
        /// Allow user to remove an existing connection token.
        /// </summary>
        /// <param name="providerType"></param>
        public void RemoveConnection(PROVIDER_TYPE providerType)
        {
            if (IsConnectedWith(providerType))
                accessGrant.Remove(providerType);
        }

        /// <summary>
        /// Provides URL for redirecting user to specified provider
        /// </summary>
        public string GetLoginRedirectUrl(PROVIDER_TYPE providerType, string returnUrl)
        {
            var provider = ProviderFactory.GetProvider(providerType, socialAuthConfiguration);
            provider.ConnectionToken = new Token()
            {
                Domain = "http://www.chojogakuin.com",
                Profile = new UserProfile(providerType)
            };
            return provider.GetLoginRedirectUrl(returnUrl);
        }

        /// <summary>
        /// Connects and retrieves access token from provider
        /// </summary>
        /// <param name="providerType"></param>
        /// <param name="redirectResponse"></param>
        /// <param name="overrideConnection"></param>
        /// <returns></returns>
        public bool Connect(PROVIDER_TYPE providerType, string redirectResponse, bool overrideConnection = false)
        {
            if (overrideConnection && IsConnectedWith(providerType)) return true;


            var provider = ProviderFactory.GetProvider(providerType, socialAuthConfiguration) as Provider;
            Token token = new Token();
            token.Domain = "http://www.chojogakuin.com";
            accessGrant.Add(providerType, token);
            provider.ConnectionToken = token;
            provider.LoginCallback(Utility.GetQuerystringParameters(redirectResponse), (x,y) => { });
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerType"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public WebResponse ExecuteFeed(PROVIDER_TYPE providerType, string url)
        {
            var provider = ProviderFactory.GetProvider(providerType, socialAuthConfiguration) as Provider;
            //Token token = accessGrant[providerType];
            return provider.ExecuteFeed(url, TRANSPORT_METHOD.GET);
        }
        ///// <summary>
        ///// Execute data feed with current or specified provider
        ///// </summary>
        ///// <param name="feedUrl"></param>
        ///// <param name="transportMethod"></param>
        ///// <returns></returns>
        public WebResponse ExecuteFeed(string feedUrl, TRANSPORT_METHOD transportMethod, PROVIDER_TYPE providerType = PROVIDER_TYPE.NOT_SPECIFIED, byte[] content = null, Dictionary<string, string> headers = null)
        {

            var provider = ProviderFactory.GetProvider(providerType, socialAuthConfiguration) as Provider;
            //Token token = accessGrant[providerType];


            //Call ExecuteFeed
            WebResponse response;
            if (headers == null && content == null)
                response = provider.ExecuteFeed(feedUrl, transportMethod);
            else
                response = provider.ExecuteFeed(feedUrl, transportMethod, content, headers);
            return response;

        }

        /// <summary>
        /// (Optional) If user wants AccessToken (may be for their own persistance)
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public Token GetAccessGrant(PROVIDER_TYPE providerType)
        {
            return accessGrant[providerType];
        }
        #endregion

    }
}
