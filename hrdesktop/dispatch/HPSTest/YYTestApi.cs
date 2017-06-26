                                                                                                                                                                                                                                                            
using System;
using CookComputing.XmlRpc;

namespace HPSTest
{
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public struct UserInfo
  {
      public int id;
      public string userName;
      public string password;
      public string mail;
      public string tel;
      public string address;
  }
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public struct Test
  {
      public int id;
      public string testName;
      public int userId;
      public string testDate;
      public string testAddress;
      public int Bidati;
      public int Qiangdati;
      public int isActive;
  }
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public struct TestDetail
  {
      public int id;
      public int testId;
      public int questionId;
      public int questionType;
  }
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public struct QuestionSingle
  {
      public int id;
      public string Content;
      public string SelectionA;
      public string SelectionB;
      public string SelectionC;
      public string SelectionD;
      public string Answer;
      public string Hardness;
      public int Score;
  }
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public struct UserTestDetail
  {
      public int id;
      public int userId;
      public int testId;
      public int QuestionId;
      public int QuestionType;
      public string Answer;
  }
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  public struct UserTest_
  {
      public int id;
      public int userId;
      public int testId;
      public int Score;
      public int Rank;
  }
  /// <summary>
  /// YYTestApi
  /// </summary>
  public interface YYTestApi
  {
      /// <summary>
      /// 从服务器上取得用户信息
      /// </summary>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      [XmlRpcMethod("YYTestApi.getUsers",
         Description = "Retrieves a list of valid users for a post "
         + "using the YYTestApi API. Returns the YYTestApi users "
         + "struct collection.")]
      UserInfo[] getUsers(
        string username,
        string password);

      /// <summary>
      /// 从服务器上取得用户竞赛信息
      /// </summary>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      [XmlRpcMethod("YYTestApi.getUserTests",
         Description = "Retrieves a list of valid UserTestDetails for a post "
         + "using the YYTestApi API. Returns the YYTestApi UserTests "
         + "struct collection.")]
      UserTest_[] getUserTests(
        string username,
        string password);

      /// <summary>
      /// 从服务器上取得用户答题信息
      /// </summary>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      [XmlRpcMethod("YYTestApi.getUserTestDetails",
         Description = "Retrieves a list of valid UserTestDetails for a post "
         + "using the YYTestApi API. Returns the YYTestApi UserTestDetails "
         + "struct collection.")]
      UserTestDetail[] getUserTestDetails(
        string username,
        string password);

      /// <summary>
      /// 从服务器上取得竞赛是否开始状态
      /// </summary>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      [XmlRpcMethod("YYTestApi.getTestStatus",
         Description = "Retrieves a list of valid users for a post "
         + "using the YYTestApi API. Returns the YYTestApi tests "
         + "struct collection.")]
      Test[] getTestStatus(
        string username,
        string password);

      /// <summary>
      /// 向服务器发送考试信息
      /// </summary>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      [XmlRpcMethod("YYTestApi.setTests",
         Description = "Retrieves a list of valid Tests for a post "
         + "using the YYTestApi API. Returns the YYTestApi Tests "
         + "struct collection.")]
      bool setTests(
        string username,
        string password,
        Test[] tests);

      /// <summary>
      /// 向服务器发送考试详细信息
      /// </summary>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      [XmlRpcMethod("YYTestApi.setTestDetails",
         Description = "Retrieves a list of valid TestDetails for a post "
         + "using the YYTestApi API. Returns the YYTestApi TestDetails "
         + "struct collection.")]
      bool setTestDetails(
        string username,
        string password,
        TestDetail[] testDetails);

      /// <summary>
      /// 向服务器发送试题信息
      /// </summary>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      [XmlRpcMethod("YYTestApi.setQuestionSingles",
         Description = "Retrieves a list of valid QuestionSingles for a post "
         + "using the YYTestApi API. Returns the YYTestApi QuestionSingles "
         + "struct collection.")]
      bool setQuestionSingles(
        string username,
        string password,
        QuestionSingle[] questionSingles);

      /// <summary>
      /// 向服务器发送用户
      /// </summary>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      [XmlRpcMethod("YYTestApi.setUsers",
         Description = "Retrieves a list of valid Users for a post "
         + "using the YYTestApi API. Returns the YYTestApi Users "
         + "struct collection.")]
      bool setUsers(
        string username,
        string password,
        UserInfo[] users);

      /// <summary>
      /// 向服务器发送开始考试信息
      /// </summary>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      [XmlRpcMethod("YYTestApi.setStartTest",
         Description = "Retrieves a list of valid Tests for a post "
         + "using the YYTestApi API. Returns the YYTestApi Tests "
         + "struct collection.")]
      bool setStartTest(
        string username,
        string password,
        int testid);

      /// <summary>
      /// 向服务器发送结束考试信息
      /// </summary>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      [XmlRpcMethod("YYTestApi.setEndTest",
         Description = "Retrieves a list of valid Tests for a post "
         + "using the YYTestApi API. Returns the YYTestApi Tests "
         + "struct collection.")]
      bool setEndTest(
        string username,
        string password,
        int testid);

      /// <summary>
      /// 向服务器发送通知信息
      /// </summary>
      /// <param name="username"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      [XmlRpcMethod("YYTestApi.setInfor",
         Description = "Retrieves a list of valid infor for a post "
         + "using the YYTestApi API. Returns the YYTestApi Infors "
         + "struct collection.")]
      bool setInfor(
        string username,
        string password,
        string content);

  }
}


