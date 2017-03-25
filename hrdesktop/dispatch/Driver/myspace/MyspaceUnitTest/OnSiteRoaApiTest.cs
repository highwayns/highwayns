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
    /// Test cases for  for Onsite RoaApi Class
    /// </summary>
    [TestClass]
    public class OnSiteRoaApiTest
    {
        OnsiteContext context;
        MySpaceID.SDK.Api.RoaApi openSocialv9;
        string albumid = Convert.ToString(Constants.albumid);
        string mediaItemId = Convert.ToString(Constants.mediaItemId);
        string FriendId = Convert.ToString(Constants.friendId);
        int appId = Convert.ToInt32(Constants.appId);
        string userId = Convert.ToString(Constants.userId);
        int videoId = Convert.ToInt32(Constants.videoId);
        int pageId = Convert.ToInt32(Constants.pageId);
        int PageSize = Convert.ToInt32(Constants.pageSize);
        string photoPath = Convert.ToString(Constants.photoPath);
        string videoPath = Convert.ToString(Constants.videoPath);
        string statudId = Constants.statusId;

        public OnSiteRoaApiTest()
        {
            context = new OnsiteContext(Constants.OnSiteConsumerKey, Constants.OnSiteConsumerSecret);
            openSocialv9 = new MySpaceID.SDK.Api.RoaApi(context);


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
        public void GetAllPhotosTest()
        {
            string testResult = openSocialv9.GetPhotos(userId, albumid);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetPhotoTest()
        {
            string testResult = openSocialv9.GetPhoto(userId, albumid, mediaItemId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetVideoTest()
        {
            string testResult = openSocialv9.GetVideo(userId, videoId);
            Assert.AreNotEqual(0, testResult.Length);
        }


        [TestMethod]
        public void GetAllVideosTest()
        {
            string testResult = openSocialv9.GetVideos(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetAlbumTest()
        {
            string testResult = openSocialv9.GetAlbum(userId, albumid);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetalbumFeildsTest()
        {
            string testResult = openSocialv9.GetAlbumFields();
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetAllAlbumsTest()
        {
            string testResult = openSocialv9.GetAlbums(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetFriendsActivitiesTest()
        {
            string testResult = openSocialv9.GetFriendsActivities(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }
        [TestMethod]
        public void GetFriendsProfileTest()
        {
            string testResult = openSocialv9.GetFriends(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }
        [TestMethod]
        public void GetFriendStatusMoodTest()
        {
            string testResult = openSocialv9.GetFriendStatusMood(userId, FriendId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetFriendStatusMoodHistoryTest()
        {
            string testResult = openSocialv9.GetFriendStatusMoodHistory(userId, FriendId);
            Assert.AreNotEqual(0, testResult.Length);
        }
        [TestMethod]
        public void GetMediaItemcommentsTest()
        {
            string testResult = openSocialv9.GetMediaItemcomments(userId, albumid, mediaItemId);
            Assert.AreNotEqual(0, testResult.Length);
        }
        [TestMethod]
        public void GetMyActivitiesTest()
        {
            string testResult = openSocialv9.GetMyActivities(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }


        [TestMethod]
        public void GetMyGroupsDataTest()
        {
            string testResult = openSocialv9.GetMyGroupsData(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetMyMoodCommentsTest()
        {
            string testResult = openSocialv9.GetMyMoodComments(userId, statudId);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void GetMyProfileCommentsTest()
        {
            string testResult = openSocialv9.GetMyProfileComments(userId);
            Assert.AreNotEqual(0, testResult.Length);
        }



        [TestMethod]
        public void AddNotificationTest()
        {
            string testResult = openSocialv9.AddNotification(userId, string.Empty, userId, null);
            Assert.AreNotEqual(0, testResult.Length);
        }



    }
}
