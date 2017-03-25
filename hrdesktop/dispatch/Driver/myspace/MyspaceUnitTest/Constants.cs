using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace MyspaceUnitTest
{
    public class Constants
    {
        

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
        /// <summary>
        /// Insert OAuthTokenKey here for test cases
        /// </summary>
        public static readonly string OAuthTokenKey = "INSERT_OAUTH_TOKEN_KEY_HERE";

        /// <summary>
        /// Insert OAuthTokenSecret here for test cases
        /// </summary>
        public static readonly string OAuthTokenSecret = "INSERT_OAUTH_TOKEN_SECRET_HERE";
        /// <summary>
        /// Insert UserId here for test cases
        /// </summary>
        public static readonly string userId = "INSERT_TEST_USERID_HERE";
        /// <summary>
        /// Insert AlbumId here for test cases
        /// </summary>
        public static readonly string albumid = "INSERT_TEST_ALBUMID_HERE";
        /// <summary>
        /// Insert Media ItemId here for test cases
        /// </summary>
        public static readonly string mediaItemId = "INSERT_TEST_MEDIAITEMID_HERE";
        /// <summary>
        /// Insert Friend Id here for test cases
        /// </summary>
        public static readonly string friendId = "INSERT_TEST_FRIENDID_HERE";
        /// <summary>
        /// Insert Application Id here for test cases
        /// </summary>
        public static readonly string appId = "INSERT_TEST_APPLICATIONID_HERE";
        /// <summary>
        /// Insert Video ID here for test cases
        /// </summary>
        public static readonly string videoId = "INSERT_TEST_VIDEOID_HERE";
        /// <summary>
        /// Insert Video Id here for test cases
        /// </summary>
        public static readonly string videoId_V1 = "INSERT_TEST_VIDEOID_HERE";
        /// <summary>
        /// Insert StatusId here for test cases
        /// </summary>
        public static readonly string statusId = "INSERT_TEST_STATUSID_HERE";
        /// <summary>
        /// Insert PageId here
        /// </summary>
        public static readonly string pageId = "INSERT_TEST_PAGEID_HERE";
        /// <summary>
        /// Insert Page Size here
        /// </summary>
        public static readonly string pageSize = "INSERT_TEST_PAGESIZE_HERE";
        /// <summary>
        /// Insert Path to video here for test cases
        /// </summary>
        public static readonly string videoPath = "INSERT_TEST_VIDEOPATH_HERE";
        /// <summary>
        /// Insert Path To photo here for test cases
        /// </summary>
        public static readonly string photoPath = "INSERT_TEST_PHOTOPATH_HERE";
        /// <summary>
        /// Insert Search Term here for test cases
        /// </summary>
        public static readonly string searchTerm = "INSERT_TEST_SEARCHTERM_HERE";
        /// <summary>
        /// Insert Handler URL here for test cases of Real time Stream
        /// </summary>
        public static readonly string handler = "INSERT_TEST_SUBSCRIPTION_RECIVER_HERE";
        /// <summary>
        /// Insert Transfer Rate here for test cases of Real time Stream
        /// </summary>
        public static readonly string startRate = "INSERT_TEST_SUBSCRIPTION_RATE_HERE";
        /// <summary>
        /// Insert Updated Transfer Rate here for test cases of Real time Stream
        /// </summary>
        public static readonly string updateRate = "INSERT_TEST_SUBSCRIPTION_UPDATERATE_HERE";

    }
}
