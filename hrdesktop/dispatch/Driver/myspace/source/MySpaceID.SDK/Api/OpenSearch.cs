using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Config;
using Jayrock.Json.Conversion;
using MySpaceID.SDK.Models.V2;
using System.Collections;
using System.Net;
using System.IO;
using MySpaceID.SDK.OAuth.Common.Enums;

namespace MySpaceID.SDK.Api
{

    /// <summary>
    /// <para>This Class will refer to all end points under Open search.</para>
    /// <para>Please refer to http://wiki.developer.myspace.com/index.php?title=Open_Search for full details on Open search</para>
    /// </summary>
    public class OpenSearch : BaseApi
    {
        #region properties
        public string ApiServerUri { get; set; }
        #endregion


        public OpenSearch(SecurityContext context) : base(context) { }

        #region API Methods


        /// <summary>
        /// Search Videos
        /// <para>http://api.myspace.com/opensearch/videos</para>
        /// </summary>
        /// <param name="searchTerm">Text to be searched</param>
        /// <returns></returns>
        public string SearchVideos(string searchTerm)
        {
            return SearchVideos(searchTerm, null);
        }


        /// <summary>
        /// Search Videos
        ///  <para>http://api.myspace.com/opensearch/videos</para>
        /// </summary>
        /// <param name="searchTerm">Text to be searched</param>
        /// <param name="queryParams">List Of All the Parameters in search Query e.g. Dictionary<string, string> ht = new Dictionary<string, string>(); ht.Add("format", "xml"); ht.Add("count", "5"); </param>
        /// <returns></returns>
        public string SearchVideos(string searchTerm, Dictionary<string, string> queryParams)
        {
            string uri = string.Empty;
            string qString = string.Format("searchTerms={0}&", searchTerm);
            qString += getQueryParamFromDictionary(queryParams);

            uri = string.Format(OpenSearchEndpoints.VIDEOS_OPENSEARCH, qString);
            return this.Context.MakeRequest(uri, ResponseFormatType.JSON, HttpMethodType.GET, null, false);

        }


        /// <summary>
        /// Search Images
        ///  <para>http://api.myspace.com/opensearch/images</para>
        /// </summary>
        /// <param name="searchTerm">Text to be searched</param>
        /// <returns></returns>
        public string SearchImages(string searchTerm)
        {
            return SearchImages(searchTerm, null);
        }

        /// <summary>
        /// Search Images
        ///  <para>http://api.myspace.com/opensearch/images</para>
        /// </summary>
        /// <param name="searchTerm">Text to be searched</param>
        /// <param name="queryParams">List Of All the Parameters in search Query e.g. Dictionary<string, string> ht = new Dictionary<string, string>(); ht.Add("format", "xml"); ht.Add("count", "5"); </param>
        /// <returns></returns>
        public string SearchImages(string searchTerm, Dictionary<string, string> queryParams)
        {
            string uri = string.Empty;
            string qString = string.Format("searchTerms={0}&", searchTerm);
            qString += getQueryParamFromDictionary(queryParams);

            uri = string.Format(OpenSearchEndpoints.IMAGES_OPENSEARCH, qString);
            return this.Context.MakeRequest(uri, ResponseFormatType.JSON, HttpMethodType.GET, null, false);

        }


        /// <summary>
        /// Search People
        ///  <para>http://api.myspace.com/opensearch/people</para>
        /// </summary>
        /// <param name="searchTerm">Text to be searched</param>
        /// <returns></returns>
        public string SearchPeople(string searchTerm)
        {
            return SearchPeople(searchTerm, null);
        }

        /// <summary>
        /// Search People
        ///  <para>http://api.myspace.com/opensearch/people</para>
        /// </summary>
        /// <param name="searchTerm">Text to be searched</param>
        /// <param name="queryParams">List Of All the Parameters in search Query </param>
        /// <returns></returns>
        public string SearchPeople(string searchTerm, Dictionary<string, string> queryParams)
        {
            string uri = string.Empty;
            string qString = string.Format("searchTerms={0}&", searchTerm);
            qString += getQueryParamFromDictionary(queryParams);

            uri = string.Format(OpenSearchEndpoints.PEOPLE_OPENSEARCH, qString);
            return this.Context.MakeRequest(uri, ResponseFormatType.JSON, HttpMethodType.GET, null, false);


        }


        #endregion
    }
}
