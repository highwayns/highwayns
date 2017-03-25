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

namespace Brickred.SocialAuth.NET.Core.BusinessObjects
{
    /// <summary>
    /// Enumeration for available providers
    /// </summary>
    public enum PROVIDER_TYPE
    {
        NOT_SPECIFIED=0,
        FACEBOOK,
        GOOGLE,
        YAHOO,
        MSN,
        TWITTER,
        LINKEDIN1,
        LINKEDIN,
        MYSPACE,
        GOOGLEHYBRID
      
    }

    /// <summary>
    /// Enumeration for available transport methods
    /// </summary>
    public enum TRANSPORT_METHOD
    {
        POST,
        GET,
        DELETE
    }

    /// <summary>
    /// Enumeration for available signing mechanisms
    /// </summary>
    public enum SIGNATURE_TYPE
    {
        HMACSHA1,
        PLAINTEXT,
        RSASHA1
    }

    /// <summary>
    /// Configuration Option for SocialAuth.NET
    /// </summary>
    public enum AUTHENTICATION_OPTION
    {
        SOCIALAUTH_SECURITY_SOCIALAUTH_SCREEN, //Everything by socialauth including screen generation
        SOCIALAUTH_SECURITY_CUSTOM_SCREEN, //Everything by socialauth but user's login url
        FORMS_AUTHENTICATION, //Forms authentication
        CUSTOM_SECURITY_CUSTOM_SCREEN, //User handles everything by calling API
        NOT_SUPPORTED //Error
    }

    /// <summary>
    /// Level of scope set for provider. 
    /// DEFAULT => All Features scope 
    /// CUSTOM=>User specified scope only
    /// </summary>
    public enum SCOPE_LEVEL
    {
        DEFAULT,
        CUSTOM
    }

    /// <summary>
    /// Enumeration for GENDER
    /// </summary>
    public enum GENDER
    {
        NOT_SPECIFIED = 0,
        MALE = 1,
        FEMALE = 2
    }


}
