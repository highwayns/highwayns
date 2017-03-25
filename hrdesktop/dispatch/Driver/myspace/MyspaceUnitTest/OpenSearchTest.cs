using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Configuration;
using MySpaceID.SDK;
using MySpaceID.SDK.OAuth.Tokens;
using MySpaceID.SDK.Context;
using MySpaceID.SDK.Api;
using MySpaceID.SDK.Models.V2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyspaceUnitTest
{
    /// <summary>
    /// Test cases for  for OpenSearch Class
    /// </summary>
    [TestClass]
    public class OpenSearchTest
    {
        OffsiteContext context;
        MySpaceID.SDK.Api.OpenSearch openSearch;
        string searchTerm = Constants.searchTerm;
        public OpenSearchTest()
        {
            context = new OffsiteContext(Constants.ConsumerKey, Constants.ConsumerSecret);


            openSearch = new MySpaceID.SDK.Api.OpenSearch(context);
            context.OAuthTokenKey = Constants.OAuthTokenKey;
            context.OAuthTokenSecret = Constants.OAuthTokenSecret;
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }



        [TestMethod]
        public void SearchImagesTest()
        {
            string testResult = openSearch.SearchImages(searchTerm);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void SearchPeopleTest()
        {
            string testResult = openSearch.SearchPeople(searchTerm);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void SearchVideosTest()
        {
            string testResult = openSearch.SearchVideos(searchTerm);
            Assert.AreNotEqual(0, testResult.Length);
        }
    }
}
