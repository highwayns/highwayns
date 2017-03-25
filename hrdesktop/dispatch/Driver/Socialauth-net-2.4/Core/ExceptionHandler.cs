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
using System.Runtime.Serialization;
using Brickred.SocialAuth.NET.Core.BusinessObjects;

namespace Brickred.SocialAuth.NET.Core
{
    /// <summary>
    /// Exception raised when user tries to perform an operation with a provider, he is not connected with!
    /// </summary>
    public class InvalidSocialAuthConnectionException : Exception
    {
        PROVIDER_TYPE providertype;
        
        public InvalidSocialAuthConnectionException(PROVIDER_TYPE providertype = PROVIDER_TYPE.NOT_SPECIFIED)
        {
            this.providertype = providertype;
        }
        
        public override string Message
        {
            get
            {
                if (providertype == PROVIDER_TYPE.NOT_SPECIFIED)
                    return "Can not get results as User is not connected with any provider.";
                else
                    return "Can not get results from " + providertype.ToString() + " as User is not connected with it.";
            }
        }
    }

    /// <summary>
    /// Exception raised when there is some problem in executing OAuth command
    /// </summary>
    public class OAuthException : Exception
    {
        string message = "";
        Exception ex = null;

        public OAuthException(string message, Exception ex)
        {
            this.message = message;
            this.ex = ex;
        }

        public OAuthException(string message)
        {
            this.message = message;
        }

        public override string Message
        {

            get
            {
                string additionalDetails = "";
                if (ex != null)
                    if (ex.Message.Contains("400")) //BAD REQUEST
                    {
                        additionalDetails += Environment.NewLine + "Please ensure all required parameters are passed, Signature is Url Encoded and Authorization header is properly set!";
                    }
                    else if (ex.Message.Contains("401")) //UNAUTHORIZED
                    {
                        additionalDetails += Environment.NewLine + "Unauthorized! Please ensure:" + Environment.NewLine + "(1) All required parameters are passed";
                        additionalDetails += Environment.NewLine + "(2) Signature is Url Encoded";
                        additionalDetails += Environment.NewLine + "(3) Authorization header is properly set";
                    }
                    else if (ex.Message.Contains("403")) //FORBIDDEN
                    {
                        additionalDetails += Environment.NewLine + "Forbidden! " + Environment.NewLine + ex.Message;
                    }
                    else if (ex.Message.Contains("404")) //NOT FOUND
                    {
                        additionalDetails += Environment.NewLine + " Requested URL could not be found at provider";
                    }
                    else if (ex.Message.Contains("500")) //INTERNAL SERVER ERROR
                    {
                        additionalDetails += Environment.NewLine + " Something is broken at provider. Request broke with error message:" + ex.Message;
                    }
                    else if (ex.Message.Contains("502")) //SERVICE UNAVAILABLE
                    {
                        additionalDetails += Environment.NewLine + " Possibly provider is down. Request broke with error: " + ex.Message;
                    }
                    else if (ex.Message.Contains("503")) //SERVICE UNAVAILABLE
                    {
                        additionalDetails += Environment.NewLine + " Request broke with error message: " + ex.Message;
                    }
                    else
                    {
                        additionalDetails = ex.Message;
                    }

                return message + additionalDetails;
            }
        }


    }

    public class DataParsingException : Exception
    {

        string message = "";
        public DataParsingException(string message, Exception ex)
        {
            this.message = message;
        }

        public DataParsingException(string message)
        {
            this.message = message;
        }

        public override string Message
        {
            get
            {
                return this.message;
            }
        }
    }

    public class UserDeniedPermissionException : Exception
    {
        PROVIDER_TYPE providertype;

        public UserDeniedPermissionException(PROVIDER_TYPE providertype = PROVIDER_TYPE.NOT_SPECIFIED)
        {
            this.providertype = providertype;
        }

        public override string Message
        {
            get { return ErrorMessages.UserDeniedAccess(providertype, null); }
        }
    }
}