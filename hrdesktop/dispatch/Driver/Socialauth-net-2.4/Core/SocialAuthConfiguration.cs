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
using System.Configuration;

namespace Brickred.SocialAuth.NET.Core
{

    /*************************
     * <SocialAuthProviders>
     * <add name="GoogleWrapper" ConsumerKey="sdfsdfsdf" ConsumerSecret="asdasdasd"/>
     * <add name="GoogleWrapper" ConsumerKey="sdfsdfsdf" ConsumerSecret="asdasdasd"/>
     * <add name="GoogleWrapper" ConsumerKey="sdfsdfsdf" ConsumerSecret="asdasdasd"/>
     * </SocialAuthProviders>
     * **********************/

    public class SocialAuthConfiguration : ConfigurationSection
    {
        public SocialAuthConfiguration()
        {

        }

        // Default Accessor Implementation
        public SocialAuthConfiguration this[int index]
        {
            get
            {

                return null;
            }
            set { /* Do Nothing */ }
        }


        [ConfigurationProperty("Providers")]
        [ConfigurationCollection(typeof(ProviderCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ProviderCollection Providers
        {
            get { return (ProviderCollection)this["Providers"]; }
        }

        [ConfigurationProperty("Logging")]
        public LoggingElement Logging
        {
            get { return (LoggingElement)this["Logging"]; }
        }

        [ConfigurationProperty("Authentication")]
        public AuthtneticationElement Authentication
        {
            get { return (AuthtneticationElement)this["Authentication"]; }
        }

        [ConfigurationProperty("IconFolder")]
        public IconFolderElement IconFolder
        {
            get { return (IconFolderElement)this["IconFolder"]; }
        }

        [ConfigurationProperty("BaseURL")]
        public BaseURLElement BaseURL
        {
            get { return (BaseURLElement)this["BaseURL"]; }
        }

        [ConfigurationProperty("Allow")]
        public AllowElement Allow { 
            get
            {
                return (AllowElement)this["Allow"];
            }
        }
    
    }


    public class ProviderElement : ConfigurationElement
    {

        // Default Accessor Implementation
        public ProviderElement this[int index]
        {
            get
            {

                return null;
            }
            set { /* Do Nothing */ }
        }

        [ConfigurationProperty("WrapperName", IsRequired = true)]
        public string WrapperName
        {
            get { return (string)this["WrapperName"]; }
            set { this["WrapperName"] = value; }
        }

        [ConfigurationProperty("ConsumerKey", IsRequired = true)]
        public string ConsumerKey
        {
            get { return (string)this["ConsumerKey"]; }
            set { this["ConsumerKey"] = value; }
        }

        [ConfigurationProperty("ConsumerSecret", IsRequired = true)]
        public string ConsumerSecret
        {
            get { return (string)this["ConsumerSecret"]; }
            set { this["ConsumerSecret"] = value; }
        }

        [ConfigurationProperty("AdditionalScopes")]
        public string AdditionalScopes
        {
            get { return (string)this["AdditionalScopes"]; }
            set { this["AdditionalScopes"] = value; }
        }

        [ConfigurationProperty("ScopeLevel")]
        public string ScopeLevel
        {
            get { return (string)this["ScopeLevel"]; }
            set { this["ScopeLevel"] = value; }
        }

    }


    public class ProviderCollection : ConfigurationElementCollection
    {

        public ProviderCollection()
        {

        }



        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public ProviderElement this[int index]
        {
            get { return (ProviderElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public void Add(ProviderElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ProviderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProviderElement)element).WrapperName;
        }

        public void Remove(ProviderElement element)
        {
            BaseRemove(element.WrapperName);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
    }

    public class LoggingElement : ConfigurationElement
    {
        [ConfigurationProperty("Enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["Enabled"]; }
            set { this["Enabled"] = value; }
        }
    }

    public class AuthtneticationElement : ConfigurationElement
    {
        [ConfigurationProperty("Enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["Enabled"]; }
            set { this["Enabled"] = value; }
        }

        [ConfigurationProperty("AllowModificationToUserIdentity", IsRequired = false, DefaultValue = true)]
        public bool AllowModificationToUserIdentity
        {
            get { return (bool)this["AllowModificationToUserIdentity"]; }
            set { this["AllowModificationToUserIdentity"] = value; }
        }

        [ConfigurationProperty("LoginUrl")]
        public string LoginUrl
        {
            get { return (string)this["LoginUrl"]; }
            set { this["LoginUrl"] = value; }
        }

        [ConfigurationProperty("DefaultUrl")]
        public string DefaultUrl
        {
            get { return (string)this["DefaultUrl"]; }
            set { this["DefaultUrl"] = value; }
        }

        [ConfigurationProperty("AllowedUrls")]
        public string AllowedUrls
        {
            get { return (string)this["AllowedUrls"]; }
            set { this["AllowedUrls"] = value; }
        }
    }


    public class IconFolderElement : ConfigurationElement
    {
        [ConfigurationProperty("Path", IsRequired = true)]
        public string Path
        {
            get { return (string)this["Path"]; }
            set { this["Path"] = value; }
        }
    }

    public class BaseURLElement : ConfigurationElement
    {
        [ConfigurationProperty("Domain", IsRequired = false)]
        public string Domain
        {
            get { return (string)this["Domain"]; }
            set { this["Domain"] = value; }

        }
        [ConfigurationProperty("Protocol", IsRequired = false)]
        public string Protocol
        {
            get { return (string)this["Protocol"]; }
            set { this["Protocol"] = value; }

        }
        [ConfigurationProperty("Port", IsRequired = false)]
        public string Port
        {
            get { return (string)this["Port"]; }
            set { this["Port"] = value; }

        }
        [ConfigurationProperty("Path", IsRequired = false)]
        public string Path
        {
            get { return (string)this["Path"]; }
            set { this["Path"] = value; }

        }
    }

    public class AllowElement : ConfigurationElement
    {
        [ConfigurationProperty("Files")]
        public string Files
        {
            get { return (string)this["Files"]; }
            set { this["Files"] = value; }
        }

        [ConfigurationProperty("Folders")]
        public string Folders
        {
            get { return (string)this["Folders"]; }
            set { this["Folders"] = value; }
        }
    }
}
