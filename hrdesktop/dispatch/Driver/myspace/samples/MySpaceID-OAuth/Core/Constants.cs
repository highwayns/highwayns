using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace MySpaceID_OAuth.Core
{
    public class Constants
    {
        
        public static readonly string MySpaceID = "http://api.myspace.com/VANITY_NAME";
        public static readonly string API_MYSPACE_COM = "http://api.myspace.com";


        /// <summary>
        /// insert offsite consumer key here
        /// </summary>
        public static readonly string ConsumerKey = "INSERT_CONSUMER_KEY_HERE";

        /// <summary>
        /// Insert offsite consumer secret here
        /// </summary>
        public static readonly string ConsumerSecret = "INSERT_CONSUMER_SECRET_HERE";

        /// <summary>
        /// insert onsite consumer key here
        /// </summary>
        public static readonly string OnSiteConsumerKey = "INSERT_ONSITE_CONSUMER_KEY_HERE";

        /// <summary>
        /// Insert onsite consumer secret here
        /// </summary>
        public static readonly string OnSiteConsumerSecret = "INSERT_ONSITE_CONSUMER_SECRET_HERE";

    }
}
