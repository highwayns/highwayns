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
using System.Web.UI.WebControls;
using System.Xml.Linq;
using MySpaceID.SDK.Models.V1;


namespace MyspaceUnitTest
{
    /// <summary>
    /// Test cases for  for RestV1 Class
    /// </summary>
    [TestClass]
    public class RestV1Test
    {
        OffsiteContext context;
        MySpaceID.SDK.Api.RestV1 restV1;

        int albumid = Convert.ToInt32(Constants.albumid);
        int mediaItemId = Convert.ToInt32(Constants.mediaItemId);
        int FriendId = Convert.ToInt32(Constants.friendId);
        int appId = Convert.ToInt32(Constants.appId);
        int userId = Convert.ToInt32(Constants.userId);
        int videoId = Convert.ToInt32(Constants.videoId_V1);
        int pageId = Convert.ToInt32(Constants.pageId);
        int PageSize = Convert.ToInt32(Constants.pageSize);
        string statudId = Constants.statusId;
        public RestV1Test()
        {
            context = new OffsiteContext(Constants.ConsumerKey, Constants.ConsumerSecret);


            restV1 = new MySpaceID.SDK.Api.RestV1(context);
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
        public void GetActivitiesTest()
        {
            string testResult = restV1.GetActivities(userId);
            Assert.AreNotEqual(testResult.Length,0);
        }

        [TestMethod]
        public void GetAlbumTest()
        {
            string testResult = restV1.GetAlbum(userId, albumid);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetAlbumsTest()
        {
            string testResult = restV1.GetAlbums(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetBasicProfileTest()
        {
            string testResult = restV1.GetBasicProfile(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetCurrentUserTest()
        {
            string testResult = restV1.GetCurrentUser();
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetExtendedProfileTest()
        {
            string testResult = restV1.GetExtendedProfile(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetFriendActivitiesTest()
        {
            string testResult = restV1.GetFriendActivities(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetFriendAppDataTest()
        {
            string testResult = restV1.GetFriendAppData(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetFriendsTest()
        {
            string testResult = restV1.GetFriends(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetFriendshipTest()
        {
            string testResult = restV1.GetFriendship(userId, new int[] { FriendId });
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetFriendStatusTest()
        {
            string testResult = restV1.GetFriendStatus(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetFullProfileTest()
        {
            string testResult = restV1.GetFullProfile(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetGlobalAppDataTest()
        {
            string testResult = restV1.GetGlobalAppData();
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetIndicatorsTest()
        {
            string testResult = restV1.GetIndicators(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetMoodTest()
        {
            string testResult = restV1.GetMood(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetMoodsListTest()
        {
            string testResult = restV1.GetMoodsList(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetPhotoTest()
        {
            string testResult = restV1.GetPhoto(userId, mediaItemId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

        [TestMethod]
        public void GetPhotosTest()
        {
            string testResult = restV1.GetPhotos(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }
        [TestMethod]
        public void GetStatusTest()
        {
            string testResult = restV1.GetStatus(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }
        [TestMethod]
        public void GetStatusHistoryActivitiesTest()
        {
            string testResult = restV1.GetStatusHistoryActivities(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }
        [TestMethod]
        public void GetUserTest()
        {
            string testResult = restV1.GetUser(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }
        [TestMethod]
        public void GetUserAppDataTest()
        {
            string testResult = restV1.GetUserAppData(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }
        [TestMethod]
        public void GetVideoTest()
        {
            string testResult = restV1.GetVideo(userId, videoId);
            Assert.AreNotEqual(testResult.Length, 0);
        }
        [TestMethod]
        public void GetVideosTest()
        {
            string testResult = restV1.GetVideos(userId);
            Assert.AreNotEqual(testResult.Length, 0);
        }

    }
}
