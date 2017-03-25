using System;

namespace MySpaceID.SDK.OAuth.Common.Exceptions
{
    public class UnSupportedHttpMethodException : Exception
    {
        public UnSupportedHttpMethodException(string message) : base(message)
        {
        }
    }
}