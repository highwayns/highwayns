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


namespace Brickred.SocialAuth.NET.Core
{

    [Serializable]
    class SocialAuthSession
    {
        internal Action callback;
        internal Guid userGUID;
        internal List<Token> connectedTokens;
        internal Token inProgressToken;
        internal Action callbackAction
        {
            get
            {
                if (callback == null)
                    return new Action(() => { });
                else
                    return callback;
            }
            set
            {
                callback = value;
            }
        }
        internal string errorURL;
    }



    public class SessionManager
    {



        static SocialAuthSession userSession
        {
            get
            {

                if (HttpContext.Current.Session["socialauthsession"] == null)
                    HttpContext.Current.Session["socialauthsession"] = new SocialAuthSession()
                                {
                                    userGUID = Guid.NewGuid(),
                                    connectedTokens = new List<Token>()
                                };
                return (SocialAuthSession)HttpContext.Current.Session["socialauthsession"];
            }
        }


        internal static void SetCallback(Action callback)
        {
            userSession.callback = callback;
        }

        internal static void ExecuteCallback()
        {
            userSession.callbackAction.Invoke();
        }

        internal static Token InProgressToken
        {
            get
            {
                return userSession.inProgressToken;
            }
            set
            {
                userSession.inProgressToken = value;
            }
        }

        internal static void AddConnectionToken(Token token)
        {
            Token t = userSession.connectedTokens.Find(x => x.Provider == token.Provider);
            if (t != null)
                userSession.connectedTokens.Remove(t);
            userSession.connectedTokens.Add(token);
        }

        internal static void RemoveConnectionToken(PROVIDER_TYPE providerType)
        {
            userSession.connectedTokens.RemoveAll(x => x.Provider == providerType);

        }

        internal static void RemoveAllConnections()
        {
            userSession.connectedTokens.RemoveAll(x => true);
        }

        internal static List<PROVIDER_TYPE> GetConnectedProviders()
        {
            return userSession.connectedTokens.Select((x) => { return x.Provider; }).ToList();
        }

        internal static int ConnectionsCount
        {
            get
            {
                return userSession.connectedTokens.Count();
            }
        }

        internal static bool IsConnected
        {
            get
            {
                return userSession.connectedTokens.Count() > 0;
            }
        }

        internal static bool IsConnectedWith(PROVIDER_TYPE providerType)
        {
            return (userSession.connectedTokens.Exists(x => x.Provider == providerType));

        }

        internal static void AbandonSession()
        {
            HttpContext.Current.Session.Abandon();
        }

        internal static IProvider GetCurrentConnection()
        {
            if (ConnectionsCount > 0)
            {
                var lastConnection = userSession.connectedTokens.Last();
                return ProviderFactory.GetProvider(lastConnection.Provider);
            }
            else
            {
                return null;
            }
        }

        internal static IProvider GetConnection(PROVIDER_TYPE providerType)
        {
            IProvider provider = null;
            //There are no connections
            var lastConnection = userSession.connectedTokens.Find(x => x.Provider == providerType);
            if (lastConnection != null)
            {
                provider = ProviderFactory.GetProvider(lastConnection.Provider);
                return provider;
            }
            else
                return null;

        }

        internal static Token GetConnectionToken(PROVIDER_TYPE providerType)
        {
            var connectionToken = userSession.connectedTokens.Find(x => x.Provider == providerType);
            return connectionToken;
        }

        internal static Guid GetUserSessionGUID()
        {
            return  userSession.userGUID;
        }

        public static string ErrorURL
        {
            get { return userSession.errorURL; }
            set { userSession.errorURL = value; }
        }
    }
}
