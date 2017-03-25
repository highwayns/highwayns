using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brickred.SocialAuth.NET.Core.BusinessObjects;

namespace Brickred.SocialAuth.NET.Core
{
    public interface IProviderConnect : IProvider
    {
        //******** PROVIDER OPERATIONS
        void Connect(); //Connect to a provider 1st time
        void LoginCallback(QueryParameters responseCollection, Action<bool,Token> AuthenticationHandler);// used internally to handle external login by user
    }
}
