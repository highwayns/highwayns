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
using System.Runtime.Serialization;
using System.Collections.Specialized;

namespace Brickred.SocialAuth.NET.Core.BusinessObjects
{
    [Serializable]
    public class Token
    {
        public Token()
        {
            Profile = new UserProfile();
            ResponseCollection = new QueryParameters();
        }

        //Properties we want to be serializable
        public SCOPE_LEVEL ScopeLevel { get; set; }
        public PROVIDER_TYPE Provider { get; set; }
        public string UserReturnURL { get; set; }
        public string ProviderCallbackUrl { get { return Domain + "socialauth/validate.sauth"; } }
        public DateTime ExpiresOn { get; set; }
        public string AccessToken { get; set; }
        public string TokenSecret { get; set; }
        public Guid SessionGUID { get; set; }
        public string Scope { get; set; }
        public string Domain { get; set; }
        public string OauthVerifier { get; set; }
        public QueryParameters ResponseCollection { get; set; }
        public UserProfile Profile { get; set; }


        //Properties we do not want to be serializable
        [NonSerialized]
        private string requestToken;
        [NonSerialized]
        private string authorizationToken;
        [NonSerialized]
        private string assocHandle;
        [NonSerialized]
        private string code;

        internal string RequestToken { get { return requestToken; } set { requestToken = value; } }
        internal string AuthorizationToken { get { return authorizationToken; } set { authorizationToken = value; } }
        internal string AssocHandle { get { return assocHandle; } set { assocHandle = value; } }
        internal string Code { get { return code; } set { code = value; } }

    }


   
}
