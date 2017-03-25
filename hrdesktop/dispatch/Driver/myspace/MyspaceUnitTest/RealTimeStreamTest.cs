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
using Jayrock.Json.Conversion;


namespace MyspaceUnitTest
{
    /// <summary>
    /// Test cases for  for RealTimeStream Class
    /// </summary>
    [TestClass]
    public class RealTimeStreamTest
    {

        OffsiteContext context;
        MySpaceID.SDK.Api.RealTimeStream realTimeStream;
        string handler = Constants.handler;
        string startRate = Constants.startRate;
        string updateRate = Constants.updateRate;
        string requestId = string.Empty;


        public RealTimeStreamTest()
        {
            context = new OffsiteContext(Constants.ConsumerKey, Constants.ConsumerSecret);

            realTimeStream = new MySpaceID.SDK.Api.RealTimeStream(context);
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
        public void RealtimeStreamTest()
        {

            string deleteAllResult = realTimeStream.DeleteAllRealtimeStream();
            string testResult = (realTimeStream.AddRealtimeStream(startRate, "ALL", handler, "", "", "1"));
            requestId = testResult.Substring(testResult.LastIndexOf("/") + 1, 4);
            string updateTestResult = realTimeStream.UpdateRealtimeStream(startRate, "ALL", handler, "", "", "1", requestId);
            string getTestResult = realTimeStream.GetRealtimeStream(requestId);
            string deleteSpecificResult = realTimeStream.DeleteRealtimeStream(requestId);
            string newDeleteAllResult = realTimeStream.DeleteAllRealtimeStream();

            Assert.AreNotEqual(0, testResult.Length);
            Assert.AreNotEqual(0, updateTestResult.Length);
            Assert.AreNotEqual(0, getTestResult.Length);
            Assert.IsNotNull(deleteSpecificResult);
            Assert.IsNotNull(newDeleteAllResult);
        }

    }




}
