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
    /// Test cases for  for ActivityStream Class
    /// </summary>
    [TestClass]
    public class ActivityStreamTest
    {
        OffsiteContext context;
        MySpaceID.SDK.Api.ActivityStream activityStream;
        int userId = Convert.ToInt32(Constants.userId);
        public ActivityStreamTest()
        {
            context = new OffsiteContext(Constants.ConsumerKey, Constants.ConsumerSecret);


            activityStream = new MySpaceID.SDK.Api.ActivityStream(context);
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
        public void GetMyActivityStreamTest()
        {
            string testResult = activityStream.GetMyActivityStream(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetFriendsActivityStreamTest()
        {
            string testResult = activityStream.GetFriendsActivityStream(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }
    }
}
