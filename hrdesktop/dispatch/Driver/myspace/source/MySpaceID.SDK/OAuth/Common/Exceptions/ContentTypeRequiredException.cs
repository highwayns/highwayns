using System;

namespace MySpaceID.SDK.OAuth.Common.Exceptions
{
    public class ContentTypeRequiredException : Exception
    {
        public ContentTypeRequiredException(string message) : base(message)
        {
        }
    }
}