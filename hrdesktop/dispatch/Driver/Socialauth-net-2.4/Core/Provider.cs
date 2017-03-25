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
using Microsoft.IdentityModel.Claims;
using System.Threading;
using System.Collections.Specialized;
using System.Net;

namespace Brickred.SocialAuth.NET.Core
{

    public abstract class Provider : IProvider, IProviderConnect
    {

        #region IProvider Members

        //******** PROVIDER IN CONTEXT
        public abstract PROVIDER_TYPE ProviderType
        {
            get;
        }

        //******** AUTHENTICATION STRATEGY
        public abstract OAuthStrategyBase AuthenticationStrategy
        {
            get;
        }

        //******** END POINTS
        public virtual string OpenIdDiscoveryEndpoint
        {
            get { return ""; }
        }
        public virtual string AssocHandleEndpoint
        {
            get { return ""; }
        }
        public virtual string RequestTokenEndpoint
        {
            get { return ""; }
        }
        public abstract string UserLoginEndpoint
        {
            get;
            set;
        }
        public abstract string AccessTokenEndpoint
        {
            get;

        }
        public abstract string ProfileEndpoint
        {
            get;
        }
        public abstract string ContactsEndpoint
        {
            get;
        }
        public virtual string ProfilePictureEndpoint
        {
            get { return ""; }
        }

        //******** PROVIDER PROPERTIES
        public string Consumerkey
        {
            get;
            set;
        }
        public string Consumersecret
        {
            get;
            set;
        }
        public abstract SIGNATURE_TYPE SignatureMethod
        {
            get;
        }
        public abstract TRANSPORT_METHOD TransportName
        {
            get;
        }

        //******** SCOPE MANAGEMENT
        public virtual string AdditionalScopes
        {
            get;
            set;
        }
        public abstract string DefaultScope
        {
            get;

        }
        public virtual SCOPE_LEVEL ScopeLevel
        { get; set; }

        public virtual bool IsProfileSupported
        {
            get { return true; }

        }
        public virtual string ScopeDelimeter
        {
            get { return ","; }
        }

        public virtual bool IsScopeDefinedAtProvider
        {
            get { return false; }
        }

        public string GetScope()
        {

            List<string> scopes = new List<string>();
            scopes.AddRange(AdditionalScopes.Split(new char[] { ',' }).ToList<string>());

            if (ScopeLevel == SCOPE_LEVEL.DEFAULT)
                scopes.AddRange(DefaultScope.Split(new char[] { ',' }).ToList<string>());

            string strScopes = String.Join(ScopeDelimeter, scopes.ToArray());
            if (strScopes.EndsWith(ScopeDelimeter))
                strScopes = strScopes.Substring(0, strScopes.Length - 1);
            if (strScopes.StartsWith(ScopeDelimeter))
                strScopes = strScopes.Substring(ScopeDelimeter.Length);
            return strScopes;
        }



        //******** PROVIDER OPERATIONS
        public virtual void Connect()
        {
            AuthenticationStrategy.ConnectionToken = ConnectionToken;
            AuthenticationStrategy.Login();
        }

        public virtual void Connect(Token connectionToken)
        {

        }
        public virtual void LoginCallback(QueryParameters responseCollection, Action<bool,Token> AuthenticationHandler)
        {
            AuthenticationStrategy.ConnectionToken = this.ConnectionToken;
            AuthenticationStrategy.LoginCallback(responseCollection, AuthenticationHandler);
        }
        public abstract UserProfile GetProfile();
        public abstract List<Contact> GetContacts();
        public abstract WebResponse ExecuteFeed(string feedUrl, TRANSPORT_METHOD transportMethod);

        public virtual WebResponse ExecuteFeed(string feedURL, TRANSPORT_METHOD transportMethod, byte[] content = null, Dictionary<string, string> headers = null)
        {
            if (AuthenticationStrategy != null)
                return AuthenticationStrategy.ExecuteFeed(feedURL, (IProvider)this, GetConnectionToken(), transportMethod, content, headers);
            else
                throw new NotImplementedException("This method is not implemented!;");
        }

        public Token ConnectionToken { get; set; }

        public Token GetConnectionToken()
        {
            return ConnectionToken;
        }
        public virtual void AuthenticationCompleting(bool isSuccess)
        {

        }

        public string GetLoginRedirectUrl(string returnUrl)
        {
            AuthenticationStrategy.ConnectionToken = ConnectionToken;
            return AuthenticationStrategy.GetLoginUrl(returnUrl);
        }

        #endregion
    }
}
