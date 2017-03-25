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

namespace Brickred.SocialAuth.NET.Core
{
    public class Constants
    {

        public class OAUTH
        {
            static public readonly string OAUTH_TOKEN = "oauth_token";
            static public readonly string OAUTH_TOKEN_SECRET = "oauth_token";
            static public readonly string OAUTH_VERIFIER = "oauth_token";

        }

        public class OpenID
        {

            //Openid Specification fields
            static public readonly string OPENID_AX_FIRSTNAME = "openid.ax.firstname";
            static public readonly string OPENID_AX_LASTNAME = "openid.ax.lastname";
            static public readonly string OPENID_AX_EMAIL = "openid.ax.email";
            static public readonly string OPENID_AX_COUNTRY = "openid.ax.country";
            static public readonly string OPENID_AX_LANGUAGE = "openid.ax.language";

            //Openid fields as extensions (Used by Google)
            static public readonly string OPENID_EXT1_FIRSTNAME = "openid.ext1.firstname";
            static public readonly string OPENID_EXT1_LASTNAME = "openid.ext1.lastname";
            static public readonly string OPENID_EXT1_EMAIL = "openid.ext1.email";
            static public readonly string OPENID_EXT1_COUNTRY = "openid.ext1.country";
            static public readonly string OPENID_EXT1_LANGUAGE = "openid.ext1.language";

        }

    }


    public static class ErrorMessages
    {
        public static string RequestTokenRequestError(string url, QueryParameters collection)
        {
            return "An error occurred while requesting Request Token at " + url + Environment.NewLine + "with parameters " + ((collection == null) ? "" : collection.ToString());
        }

        public static string RequestTokenResponseInvalid(QueryParameters collection)
        {
            return "Invalid Request Token received." + Environment.NewLine + "Provider returned: " + ((collection == null) ? "" : collection.ToString());
        }

        public static string AccessTokenRequestError(string url, QueryParameters collection)
        {
            string message = "An error occurred while requesting Access Token at " + url;
            if (collection != null)
                if (collection.Count > 0)
                    message += Environment.NewLine + "with parameters " + collection.ToString();
            return message;
        }

        public static string AccessTokenResponseInvalid(QueryParameters collection)
        {
            return "Invalid Access Token received." + Environment.NewLine + "Provider returned: " + ((collection == null) ? "" : collection.ToString());
        }

        public static string UserDeniedAccess(PROVIDER_TYPE providerType, QueryParameters collection)
        {
            return "User denied access to share his details with this application";
        }

        public static string InvalidConnectionUsed(PROVIDER_TYPE providerType)
        {
            return "There is no active connection with " + providerType.ToString();
        }

        public static string ProfileParsingError(string response)
        {
            return "An error occurred while parsing profile data." + Environment.NewLine + "Provider returned: " + response;
        }

        public static string ContactsParsingError(string response)
        {
            return "An error occurred while parsing contacts data." + Environment.NewLine + "Provider returned: " + response;
        }

        public static string CustomFeedExecutionError(string feedUrl, QueryParameters collection)
        {
            return "An error occurred while executing " + feedUrl + Environment.NewLine + "Request Parameters: " + ((collection == null) ? "" : ((collection == null) ? "" : collection.ToString()));
        }

        public static string CustomFeedResultParsingError()
        {
            return "An error occurred while parsing result of feed execution";
        }

        public static string UserLoginRedirectionError(string url)
        {
            return "An error occurred while redirecting user for login to " + url;
        }

        public static string UserLoginResponseError(PROVIDER_TYPE providerType, QueryParameters collection)
        {
            return "An error occurred in user login." + Environment.NewLine + "Provider returned: " + ((collection == null) ? "" : collection.ToString());
        }


    }


}
