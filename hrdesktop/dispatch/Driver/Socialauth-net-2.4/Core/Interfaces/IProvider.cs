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
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.Collections.Specialized;
using System.Net;

namespace Brickred.SocialAuth.NET.Core
{
    /// <summary>
    /// Defines properties and methods interfacing a provider
    /// </summary>
    public interface IProvider
    {

        //******** PROVIDER IN CONTEXT
        PROVIDER_TYPE ProviderType { get; }


        //******** END POINTS
        string OpenIdDiscoveryEndpoint { get; }
        string AssocHandleEndpoint { get; }
        string RequestTokenEndpoint { get; }
        string UserLoginEndpoint { get; set; }
        string AccessTokenEndpoint { get; }
        string ProfileEndpoint { get; }
        string ContactsEndpoint { get; }
        string ProfilePictureEndpoint { get; }

        //******** PROVIDER PROPERTIES
        string Consumerkey { get; set; }
        string Consumersecret { get; set; }
        SIGNATURE_TYPE SignatureMethod { get; }
        TRANSPORT_METHOD TransportName { get; }


        //******** SCOPE MANAGEMENT
        string DefaultScope { get; } //What are default scopes covering all features
        string AdditionalScopes { get; set; } // Any additional scopes? Automatically set!
        SCOPE_LEVEL ScopeLevel { get; set; }
        bool IsScopeDefinedAtProvider { get; }
        string GetScope();


        UserProfile GetProfile(); //Data Feed Access of last connected provider
        List<Contact> GetContacts(); //Data Feed Access of last connected provider
        Token ConnectionToken { get; set; }
        Token GetConnectionToken();
        WebResponse ExecuteFeed(string feedUrl, TRANSPORT_METHOD transportMethod);
        WebResponse ExecuteFeed(string feedURL, TRANSPORT_METHOD transportMethod, byte[] content = null, Dictionary<string, string> headers = null);
        string GetLoginRedirectUrl(string url);
        //void Logout();
    }
}
