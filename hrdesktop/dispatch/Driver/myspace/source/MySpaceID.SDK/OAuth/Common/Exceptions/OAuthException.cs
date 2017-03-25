using System;

namespace MySpaceID.SDK.OAuth.Common.Exceptions
{
    public class OAuthException : Exception
    {
        public OAuthException(string message) : base(message)
        {
        }
    }
}