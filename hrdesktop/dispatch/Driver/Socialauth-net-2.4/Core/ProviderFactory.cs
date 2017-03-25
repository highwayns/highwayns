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
using System.Xml.Linq;
using System.Xml;
using Brickred.SocialAuth.NET.Core.BusinessObjects;
using System.Web;

namespace Brickred.SocialAuth.NET.Core
{



    public class ProviderFactory
    {
        //Static list of all available providers
        private static Dictionary<string, IProvider> availableProviders;

        public static List<Provider> Providers
        {
            get
            {
                if (availableProviders == null)
                    LoadProviders();
                return availableProviders.Select(x => x.Value).Cast<Provider>().ToList();
            }
        }

        //Returns a provider object for specified PROVIDER_TYPE
        public static IProvider GetProvider(PROVIDER_TYPE requestedProvider)
        {
            //Provider's list not initialized yet
            if (availableProviders == null)
                LoadProviders();


            return (IProvider)(availableProviders.Where
                        (p => ((Provider)p.Value).ProviderType == requestedProvider).Single().Value);

        }


        //Load all providers from the Providers.config file
        private static void LoadProviders()
        {
            availableProviders = new Dictionary<string, IProvider>();

            //Load all providers from ConfigSection
            SocialAuthConfiguration config = System.Configuration.ConfigurationManager.GetSection("SocialAuthConfiguration") as SocialAuthConfiguration;
            var providers = config.Providers;

            foreach (ProviderElement provider in providers)
            {
                string providerName = provider.WrapperName;
                IProvider providerType = Utility.GetInstance<Provider>(providerName);
                if (providerType != null)
                {
                    //set the properties of provider
                    ConfigureProvider(provider, providerType);

                    //add the providers to static provider list
                    availableProviders.Add(providerName, providerType);
                }

            }
        }


        private static void ConfigureProvider(ProviderElement configProvider, IProvider provider)
        {
            if (string.IsNullOrEmpty(configProvider.ConsumerKey))
                throw new Exception("Please specify Consumer Key for " + provider.ProviderType);
            if (string.IsNullOrEmpty(configProvider.ConsumerSecret))
                throw new Exception("Please specify Consumer Secret for " + provider.ProviderType);

            provider.Consumerkey = configProvider.ConsumerKey;
            provider.Consumersecret = configProvider.ConsumerSecret;
            provider.AdditionalScopes = configProvider.AdditionalScopes;
            if (!string.IsNullOrEmpty(configProvider.ScopeLevel))
                provider.ScopeLevel = (SCOPE_LEVEL)Enum.Parse(typeof(SCOPE_LEVEL), configProvider.ScopeLevel);
            else
                provider.ScopeLevel = SCOPE_LEVEL.DEFAULT;
        }


        //Utility method to break singleton and force re-loading of Providers from xml
        public static void ForceReload()
        {
            availableProviders = null;
            LoadProviders();
        }

    }
}
