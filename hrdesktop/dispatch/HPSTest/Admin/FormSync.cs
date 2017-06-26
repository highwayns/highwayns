using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CookComputing.XmlRpc;
using NC.HEDAS.Lib;
using System.IO;
using NC.HPS.Lib;
namespace HPSTest.Admin
{
    public partial class FormSync : Form
    {
        private static NC.HEDAS.Lib.CmWinServiceAPI db = new NC.HEDAS.Lib.CmWinServiceAPI();
        public FormSync()
        {
            InitializeComponent();
            getAllTest();
        }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerUrl
        {
            get { return txtFile.Text; }
            set { txtFile.Text = value; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return txtUserName.Text; }
            set { txtUserName.Text = value; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 读取会议编号
        /// </summary>
        private void getAllTest()
        {
            string sql = "select Id,TestName,isActive from tbl_Test";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cmbTest.Items.Add(ds.Tables[0].Rows[i]["ID"].ToString() + ":"
                        + ds.Tables[0].Rows[i]["TestName"].ToString());
                    if (ds.Tables[0].Rows[i]["isActive"].ToString() == "1")
                    {
                        btnStart.Text = "结束竞赛";
                        cmbTest.SelectedIndex = i;
                        cmbTest.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSync_Click(object sender, EventArgs e)
        {
            pgbSync.Value = 0;
            pgbSync.Minimum = 1;
            pgbSync.Maximum = 8;
            lblProgressInfor.Text = "同步进度开始...";
            Application.DoEvents();
            //竞赛状态更新
            updateTestStatus();
            //配置値设定
            SetConfigValue();
            pgbSync.Value++;
            Application.DoEvents();
            //竞赛上传
            uploadTest();
            pgbSync.Value++;
            Application.DoEvents();
            //竞赛详细上传
            uploadTestDetail();
            pgbSync.Value++;
            Application.DoEvents();
            //试题上传
            uploadQuestion();
            pgbSync.Value++;
            Application.DoEvents();
            //参赛者下载
            //downloadUser();
            //参赛者上传
            uploadUsers();
            pgbSync.Value++;
            Application.DoEvents();
            //参赛者排名信息下载
            downloadUserTest();
            pgbSync.Value++;
            Application.DoEvents();
            //参赛者成绩下载
            downloadUserTestDetail();
            pgbSync.Value++;
            Application.DoEvents();
            lblProgressInfor.Text = "同步进度结束";
        }
        /// <summary>
        /// 配置値设定
        /// </summary>
        private bool SetConfigValue()
        {
            bool ret = true;
            NC.HPS.Lib.NdnXmlConfig xmlConfig;
            string appName = Path.GetFileNameWithoutExtension(System.Windows.Forms.Application.ExecutablePath);
            xmlConfig = new NdnXmlConfig(string.Format(NCConst.CONFIG_FILE_DIR,appName) + NCUtility.GetAppConfig());
            if (!xmlConfig.WriteValue("config", "ServerUrl", ServerUrl))
            {
                ret = false;
            }
            if (!xmlConfig.WriteValue("config", "UserName", UserName))
            {
                ret = false;
            }
            if (!xmlConfig.WriteValue("config", "Password", Password))
            {
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 竞赛上传
        /// </summary>
        /// <returns></returns>
        private bool uploadTest()
        {
            string mediaUrl = txtFile.Text;
            string usern = txtUserName.Text;
            string pass = txtPassword.Text;

            string sql = "select * from tbl_Test";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Test[] tests = new Test[ds.Tables[0].Rows.Count]; 
                for(int i =0;i<ds.Tables[0].Rows.Count;i++)
                {
                    tests[i] = new Test();
                    tests[i].id= Convert.ToInt32(ds.Tables[0].Rows[i]["id"]);
                    tests[i].testName = ds.Tables[0].Rows[i]["testName"].ToString();
                    tests[i].userId = Convert.ToInt32(ds.Tables[0].Rows[i]["userId"]);
                    tests[i].testDate = ds.Tables[0].Rows[i]["testDate"].ToString();
                    tests[i].testAddress = ds.Tables[0].Rows[i]["testAddress"].ToString(); 
                    tests[i].Bidati = Convert.ToInt32(ds.Tables[0].Rows[i]["Bidati"]);
                    tests[i].Qiangdati = Convert.ToInt32(ds.Tables[0].Rows[i]["Qiangdati"]);
                    tests[i].isActive = Convert.ToInt32(ds.Tables[0].Rows[i]["isActive"]);

                }
                YYTestApi yyTestApi = (YYTestApi)XmlRpcProxyGen.Create(typeof(YYTestApi));
                XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)yyTestApi;
                clientProtocol.XmlEncoding = System.Text.Encoding.UTF8;
                clientProtocol.Url = mediaUrl;
                bool result = yyTestApi.setTests(usern, pass, tests);
                if (!result)
                {
                    lblProgressInfor.Text = "竞赛上传失败！";
                    //MessageBox.Show("竞赛上传失败！");
                }
                else
                {
                    lblProgressInfor.Text = "竞赛上传成功！";
                    //MessageBox.Show("竞赛上传成功！");
                }
                
            }
            return true;
        }
        /// <summary>
        /// 竞赛详细上传
        /// </summary>
        /// <returns></returns>
        private bool uploadTestDetail()
        {
            string mediaUrl = txtFile.Text;
            string usern = txtUserName.Text;
            string pass = txtPassword.Text;

            string sql = "select * from tbl_Test_Detail";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                TestDetail[] testDetails = new TestDetail[ds.Tables[0].Rows.Count];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    testDetails[i] = new TestDetail();
                    testDetails[i].id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"]);
                    testDetails[i].testId = Convert.ToInt32(ds.Tables[0].Rows[i]["testId"]);
                    testDetails[i].questionId = Convert.ToInt32(ds.Tables[0].Rows[i]["questionId"]);
                    testDetails[i].questionType = Convert.ToInt32(ds.Tables[0].Rows[i]["questionType"]);

                }
                YYTestApi yyTestApi = (YYTestApi)XmlRpcProxyGen.Create(typeof(YYTestApi));
                XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)yyTestApi;
                clientProtocol.Url = mediaUrl;
                bool result = yyTestApi.setTestDetails(usern, pass, testDetails);
                if (!result)
                {
                    lblProgressInfor.Text = "竞赛详细上传失败！";
                    //MessageBox.Show("竞赛详细上传失败！");
                }
                else
                {
                    lblProgressInfor.Text = "竞赛详细上传成功！";
                    //MessageBox.Show("竞赛详细上传成功！");
                }

            }

            return true;
        }
        /// <summary>
        /// 试题上传
        /// </summary>
        /// <returns></returns>
        private bool uploadQuestion()
        {
            string mediaUrl = txtFile.Text;
            string usern = txtUserName.Text;
            string pass = txtPassword.Text;

            string sql = "select * from tbl_Question_Single";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                QuestionSingle[] questionSingles = new QuestionSingle[1];
                YYTestApi yyTestApi = (YYTestApi)XmlRpcProxyGen.Create(typeof(YYTestApi));
                XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)yyTestApi;
                clientProtocol.XmlEncoding = System.Text.Encoding.UTF8;
                clientProtocol.Url = mediaUrl;
                bool result = true;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    questionSingles[0] = new QuestionSingle();
                    questionSingles[0].id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"]);
                    questionSingles[0].Content = ds.Tables[0].Rows[i]["Content"].ToString();
                    questionSingles[0].SelectionA = ds.Tables[0].Rows[i]["SelectionA"].ToString();
                    questionSingles[0].SelectionB = ds.Tables[0].Rows[i]["SelectionB"].ToString();
                    questionSingles[0].SelectionC = ds.Tables[0].Rows[i]["SelectionC"].ToString();
                    questionSingles[0].SelectionD = ds.Tables[0].Rows[i]["SelectionD"].ToString();
                    questionSingles[0].Answer = ds.Tables[0].Rows[i]["Answer"].ToString();
                    questionSingles[0].Hardness = ds.Tables[0].Rows[i]["Hardness"].ToString();
                    questionSingles[0].Score = Convert.ToInt32(ds.Tables[0].Rows[i]["Score"]);
                    result = result && yyTestApi.setQuestionSingles(usern, pass, questionSingles);
                }
                if (!result)
                {
                    lblProgressInfor.Text = "试题上传失败！";
                    //MessageBox.Show("试题上传失败！");
                }
                else
                {
                    lblProgressInfor.Text = "试题上传成功！";
                    //MessageBox.Show("试题上传成功！");
                }

            }

            return true;
        }
        /// <summary>
        /// 参赛者下载
        /// </summary>
        /// <returns></returns>
        private bool downloadUser()
        {
            string mediaUrl = txtFile.Text;
            string usern = txtUserName.Text;
            string pass = txtPassword.Text;

            YYTestApi yyTestApi = (YYTestApi)XmlRpcProxyGen.Create(typeof(YYTestApi));
            XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)yyTestApi;
            clientProtocol.Url = mediaUrl;
            UserInfo[] users = yyTestApi.getUsers(usern, pass);
            foreach (UserInfo user in users)
            {
                string userName = user.userName;
                string password = user.password;
                string birthday = "2016/01/01"; 
                bool sex = true;
                string mail = user.mail;
                string tel = user.tel;
                string address = user.address;
                int userId = user.id;

                if (isUserExists(userId))
                {
                    string sql = "update [tbl_User] set "
                    + "[UserName] ='" + userName + "'"
                    + ",[Password]='" + password + "'"
                    + ",[Birthday]=" + birthday + ""
                    + ",[Sex]=" + sex + ""
                    + ",[Mail]='" + mail + "'"
                    + ",[Tel]='" + tel + "'"
                    + ",[Address]='" + address + "'"
                    + " where id=" + userId;
                    db.ExeSQL(sql);
                }
                else
                {
                    string sql = "insert into [tbl_User]([id],[UserName],[Password],[Birthday],[Sex],[Mail],[Tel],[Address],[IsAdmin]) values("
                    + "" + userId + ""
                    + ",'" + userName + "'"
                    + ",'" + password + "'"
                    + "," + birthday + ""
                    + "," + sex + ""
                    + ",'" + mail + "'"
                    + ",'" + tel + "'"
                    + ",'" + address + "'"
                    + ",False)";
                    db.ExeSQL(sql);
                }
            }
            lblProgressInfor.Text = "参赛者下载成功！";
            //MessageBox.Show("参赛者下载成功！");
            return true;
        }
        /// <summary>
        /// 用户上传
        /// </summary>
        /// <returns></returns>
        private bool uploadUsers()
        {
            string mediaUrl = txtFile.Text;
            string usern = txtUserName.Text;
            string pass = txtPassword.Text;

            string sql = "select * from tbl_User where id>1";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                UserInfo[] users = new UserInfo[1];
                YYTestApi yyTestApi = (YYTestApi)XmlRpcProxyGen.Create(typeof(YYTestApi));
                XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)yyTestApi;
                clientProtocol.XmlEncoding = System.Text.Encoding.UTF8;
                clientProtocol.Url = mediaUrl;
                bool result = true;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    users[0] = new UserInfo();
                    users[0].id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"]);
                    users[0].userName = ds.Tables[0].Rows[i]["userName"].ToString();
                    users[0].password = ds.Tables[0].Rows[i]["password"].ToString();
                    users[0].mail = ds.Tables[0].Rows[i]["mail"].ToString();
                    users[0].tel = ds.Tables[0].Rows[i]["tel"].ToString();
                    users[0].address = ds.Tables[0].Rows[i]["address"].ToString();
                    result = result && yyTestApi.setUsers(usern, pass, users);
                }
                if (!result)
                {
                    lblProgressInfor.Text = "用户上传失败！";
                }
                else
                {
                    lblProgressInfor.Text = "用户上传成功！";
                }

            }

            return true;
        }

        /// <summary>
        /// 用户存在判断
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private bool isUserExists(int userid)
        {
            string sql = "select * from tbl_User where id=" + userid.ToString();
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 参赛者排名存在判断
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private bool isUserTestExists(int id)
        {
            string sql = "select * from tbl_User_test where id=" + id.ToString();
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 参赛者成绩存在判断
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private bool isUserTestDetailExists(int id)
        {
            string sql = "select * from tbl_User_test_detail where id=" + id.ToString();
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 参赛者排名信息下载
        /// </summary>
        /// <returns></returns>
        private bool downloadUserTest()
        {
            string mediaUrl = txtFile.Text;
            string usern = txtUserName.Text;
            string pass = txtPassword.Text;

            YYTestApi yyTestApi = (YYTestApi)XmlRpcProxyGen.Create(typeof(YYTestApi));
            XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)yyTestApi;
            clientProtocol.Url = mediaUrl;
            UserTest_[] userTests = yyTestApi.getUserTests(usern, pass);
            foreach (UserTest_ userTest in userTests)
            {
                int id = userTest.id;
                int userId = userTest.userId;
                int testId = userTest.testId;
                int score = userTest.Score;
                int rank = userTest.Rank;

                if (isUserTestExists(id))
                {
                    string sql = "update [tbl_User_test] set "
                    + "[userId] =" + userId + ""
                    + ",[testId]=" + testId + ""
                    + ",[score]=" + score + ""
                    + ",[rank]=" + rank + ""
                    + " where id=" + id;
                    db.ExeSQL(sql);
                }
                else
                {
                    string sql = "insert into [tbl_User_test]([userId],[testId],[score],[rank]) values("
                    + "" + userId + ""
                    + "," + testId + ""
                    + "," + score + ""
                    + "," + rank + ")";
                    db.ExeSQL(sql);
                }
            }
            lblProgressInfor.Text = "赛者排名信息下载成功！";
            //MessageBox.Show("赛者排名信息下载成功！");
            return true;
        }
        /// <summary>
        /// 参赛者成绩下载
        /// </summary>
        /// <returns></returns>
        private bool downloadUserTestDetail()
        {
            string mediaUrl = txtFile.Text;
            string usern = txtUserName.Text;
            string pass = txtPassword.Text;

            YYTestApi yyTestApi = (YYTestApi)XmlRpcProxyGen.Create(typeof(YYTestApi));
            XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)yyTestApi;
            clientProtocol.Url = mediaUrl;
            UserTestDetail[] userTestDetails = yyTestApi.getUserTestDetails(usern, pass);
            foreach (UserTestDetail userTestDetail in userTestDetails)
            {
                int id = userTestDetail.id;
                int userId = userTestDetail.userId;
                int testId = userTestDetail.testId;
                int questionId = userTestDetail.QuestionId;
                int questionType = userTestDetail.QuestionType;
                string Answer = userTestDetail.Answer;

                if (isUserTestDetailExists(id))
                {
                    string sql = "update [tbl_User_test_detail] set "
                    + "[userId] =" + userId + ""
                    + ",[testId]=" + testId + ""
                    + ",[questionId]=" + questionId + ""
                    + ",[questionType]=" + questionType + ""
                    + ",[Answer]='" + Answer + "'"
                    + " where id=" + id;
                    db.ExeSQL(sql);
                }
                else
                {
                    string sql = "insert into [tbl_User_test_detail]([userId],[testId],[questionId],[QuestionType],[Answer]) values("
                    + "" + userId + ""
                    + "," + testId + ""
                    + "," + questionId + ""
                    + "," + questionType + ""
                    + ",'" + Answer + "')";
                    db.ExeSQL(sql);
                }
            }
            lblProgressInfor.Text = "参赛者成绩下载成功！";
            //MessageBox.Show("参赛者成绩下载成功！");
            return true;
        }
        /// <summary>
        /// 开始竞赛
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbTest.Text))
            {
                MessageBox.Show("请选择会议！");

                return ;
            }
            //开始竞赛
            if (btnStart.Text == "开始竞赛")
            {
                if (StartTest())
                {
                    int testId = Convert.ToInt32(cmbTest.Text.ToString().Split(':')[0]);
                    setActiveTest(testId, true);
                    cmbTest.Enabled = false;
                    btnStart.Text = "结束竞赛";
                    MessageBox.Show("开始竞赛状态更新成功！");
                    
                }
                else
                {
                    MessageBox.Show("开始竞赛状态更新失败！");
                }
            }
            else if (btnStart.Text == "结束竞赛")
            {
                if (EndTest())
                {
                    setActiveTest(0, false);
                    btnStart.Text = "开始竞赛";
                    cmbTest.Enabled = true;
                    MessageBox.Show("结束竞赛状态更新成功！");
                }
                else
                {
                    MessageBox.Show("结束竞赛状态更新失败！");
                }
            }
        }
        /// <summary>
        /// 开始竞赛
        /// </summary>
        /// <returns></returns>
        private bool StartTest()
        {
            int testId = Convert.ToInt32(cmbTest.Text.ToString().Split(':')[0]);
            string mediaUrl = txtFile.Text;
            string usern = txtUserName.Text;
            string pass = txtPassword.Text;

            YYTestApi yyTestApi = (YYTestApi)XmlRpcProxyGen.Create(typeof(YYTestApi));
            XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)yyTestApi;
            clientProtocol.Url = mediaUrl;
            return  yyTestApi.setStartTest(usern, pass,testId);
        }
        /// <summary>
        /// 结束竞赛
        /// </summary>
        /// <returns></returns>
        private bool EndTest()
        {
            int testId = Convert.ToInt32(cmbTest.Text.ToString().Split(':')[0]);
            string mediaUrl = txtFile.Text;
            string usern = txtUserName.Text;
            string pass = txtPassword.Text;

            YYTestApi yyTestApi = (YYTestApi)XmlRpcProxyGen.Create(typeof(YYTestApi));
            XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)yyTestApi;
            clientProtocol.Url = mediaUrl;
            return yyTestApi.setEndTest(usern, pass, testId);
        }
        /// <summary>
        /// 设置当前会议
        /// </summary>
        private void setActiveTest(int testid,bool isActive)
        {
            string sql = "update tbl_Test set isActive = 0 ";
            db.ExeSQL(sql);
            if (isActive)
            {
                sql = "update tbl_Test set isActive = 1 where id=" + testid;
                db.ExeSQL(sql);
            }
        }

        /// <summary>
        /// 竞赛状态更新
        /// </summary>
        /// <returns></returns>
        private bool updateTestStatus()
        {
            string mediaUrl = txtFile.Text;
            string usern = txtUserName.Text;
            string pass = txtPassword.Text;

            YYTestApi yyTestApi = (YYTestApi)XmlRpcProxyGen.Create(typeof(YYTestApi));
            XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)yyTestApi;
            clientProtocol.Url = mediaUrl;
            Test[] tests = yyTestApi.getTestStatus(usern, pass);
            if (tests.Length>0 && tests[0].isActive == 1)
            {
                setActiveTest(tests[0].id,true);
                cmbTest.Enabled = false;
            }
            else
            {
                setActiveTest(0, false);
                cmbTest.Enabled = true;
            }
            lblProgressInfor.Text = "竞赛状态更新成功！";
            //MessageBox.Show("竞赛状态更新成功！");
            return false;
        }
        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInfor_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInfor.Text))
            {
                MessageBox.Show("请输入通知内容！");
                return;
            }
            string mediaUrl = txtFile.Text;
            string usern = txtUserName.Text;
            string pass = txtPassword.Text;

            YYTestApi yyTestApi = (YYTestApi)XmlRpcProxyGen.Create(typeof(YYTestApi));
            XmlRpcClientProtocol clientProtocol = (XmlRpcClientProtocol)yyTestApi;
            clientProtocol.Url = mediaUrl;
            if (yyTestApi.setInfor(usern, pass, txtInfor.Text))
            {
                MessageBox.Show("通知更新成功！");
            }
            else
            {
                MessageBox.Show("通知更新失败！");
            }
        }
    }
}
