using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HEDAS.Lib;
using System.Collections;

namespace HPSTest.Admin
{
    public partial class FormTestManager : Form
    {
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        private int id = -1;
        public FormTestManager()
        {
            InitializeComponent();
            getUsers();
            btnConfirm.Text = "追加";
        }
        public FormTestManager(int id)
        {
            InitializeComponent();
            this.id = id;
            getTestById(id);
            getUsers(id);
            btnConfirm.Text = "更新";
        }
        /// <summary>
        /// 读取用户信息
        /// </summary>
        private void getTestById(int id)
        {
            string sql = "select * from tbl_Test where id ="+id;
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtTestName.Text = ds.Tables[0].Rows[0]["testName"].ToString();
                dtpTest.Text = ds.Tables[0].Rows[0]["testDate"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["testAddress"].ToString();
                nudBidati.Text = ds.Tables[0].Rows[0]["Bidati"].ToString();
                nudQiangdati.Text = ds.Tables[0].Rows[0]["Qiangdati"].ToString();
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (check())
            {
                if (id > -1)//更新
                {
                    string sql = "delete from tbl_test where id="+id;
                    if (db.ExeSQL(sql))
                    {
                        sql = "delete from tbl_Test_Detail where testid=" + id;
                        if (!db.ExeSQL(sql))
                        {
                            MessageBox.Show("清除会议答题选题失败！");
                            return;
                        }
                        sql = "delete from tbl_Test_User where testid=" + id;
                        if (!db.ExeSQL(sql))
                        {
                            MessageBox.Show("清除会议参赛者失败！");
                            return;
                        }
                        sql = "delete from tbl_User_Test where testid=" + id;
                        if (!db.ExeSQL(sql))
                        {
                            MessageBox.Show("清除参赛排名失败！");
                            return;
                        }
                        sql = "delete from tbl_User_Test_Detail where testid=" + id;
                        if (!db.ExeSQL(sql))
                        {
                            MessageBox.Show("清除参赛答题详细失败！");
                            return;
                        }
                    }
                }
                if (CreateTest())
                {
                    int testId = getTestId();
                    if (testId > 0)
                    {
                        if (CreateTestUser(testId))
                        {
                            MessageBox.Show("会议创建成功！");
                        }
                        else
                        {
                            MessageBox.Show("会议参赛人员添加失败！");
                        }
                        if (CreateTestDetail(testId))
                        {
                            MessageBox.Show("选题成功！");
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("选题失败！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("会议创建失败！");
                    }
                }
                else{
                    MessageBox.Show("会议创建失败！");
                }

            }
        }
        /// <summary>
        /// 读取用户
        /// </summary>
        private void getUsers()
        {
            string sql = "select * from tbl_User where isAdmin=False";
            DataSet ds = db.ReturnDataSet(sql);
            for(int i =0;i<ds.Tables[0].Rows.Count;i++)
            {
                clbTestUser.Items.Add(ds.Tables[0].Rows[i]["ID"].ToString() +":"
                    +ds.Tables[0].Rows[i]["UserName"].ToString());
            }
        }
        /// <summary>
        /// 读取用户
        /// </summary>
        private void getUsers(int id)
        {
            string sql = "select * from tbl_User where isAdmin=False";
            DataSet ds = db.ReturnDataSet(sql);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sql = "select * from tbl_Test_User where userid =" 
                    + ds.Tables[0].Rows[i]["ID"].ToString()
                    + " and testId = "+id.ToString();
                DataSet ds_ = db.ReturnDataSet(sql);
                if (ds_.Tables[0].Rows.Count > 0)
                {
                    clbTestUser.Items.Add(ds.Tables[0].Rows[i]["ID"].ToString() + ":"
                        + ds.Tables[0].Rows[i]["UserName"].ToString(),true);
                }
                else
                {
                    clbTestUser.Items.Add(ds.Tables[0].Rows[i]["ID"].ToString() + ":"
                        + ds.Tables[0].Rows[i]["UserName"].ToString());
                }
            }
        }
        /// <summary>
        /// 输入检查
        /// </summary>
        private bool check()
        {
            bool ret = false;
            if (string.IsNullOrWhiteSpace(txtTestName.Text))
            {
                MessageBox.Show("请输入会议名称！");
                txtTestName.Focus();
                return ret;
            }
            int Total = getTestCount();
            int Bidati = Convert.ToInt32(nudBidati.Value);
            if (Bidati > Total)
            {
                MessageBox.Show("必答题数超过题库总数！");
                nudBidati.Focus();
                return ret;
            }
            int Qiangdati = Convert.ToInt32(nudQiangdati.Value);
            if (Qiangdati > Total)
            {
                MessageBox.Show("抢答题数超过题库总数！");
                nudQiangdati.Focus();
                return ret;
            }
            if (Bidati + Qiangdati == 0)
            {
                MessageBox.Show("必答题和抢答题总数必须大于0！");
                nudQiangdati.Focus();
                return ret;
            }
            if (Bidati + Qiangdati > Total)
            {
                MessageBox.Show("必答题和抢答题总数超过题库总数！");
                nudQiangdati.Focus();
                return ret;
            }
            if (clbTestUser.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择参赛人员！");
                clbTestUser.Focus();
                return ret;
            }
            ret = true;
            return ret;
        }

        /// <summary>
        /// 读取题库记录数
        /// </summary>
        private int getTestCount()
        {
            string sql = "select count(*) from tbl_Question_Single";
            DataSet ds = db.ReturnDataSet(sql);
            return (int)ds.Tables[0].Rows[0][0];
        }
        /// <summary>
        /// 读取会议编号
        /// </summary>
        private int getTestId()
        {
            string sql = "select Id from tbl_Test where TestName ='"+txtTestName.Text+"'";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (int)ds.Tables[0].Rows[0][0];
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 新建会议记录
        /// </summary>
        private bool CreateTest()
        {
            string TestName = txtTestName.Text;
            string TestDate = dtpTest.Value.ToShortDateString();
            string TestAddress = txtAddress.Text;
            int Bidati =Convert.ToInt32( nudBidati.Value) ;
            int Qiangdati = Convert.ToInt32(nudQiangdati.Value);

            string sql = "insert into tbl_Test(TestName,TestDate,TestAddress,Bidati,Qiangdati) values("
            + "'" + TestName + "'"
            + ",'" + TestDate + "'"
            + ",'" + TestAddress + "'"
            + "," + Bidati.ToString() + ""
            + "," + Qiangdati.ToString() + ")";

            bool ret = db.ExeSQL(sql);
            return ret;
        }
        /// <summary>
        /// 新建考试人员
        /// </summary>
        private bool CreateTestUser(int TestId)
        {
            bool ret = true;
            for (int i = 0; i < clbTestUser.CheckedItems.Count; i++)
            {
                int userId = Convert.ToInt32(clbTestUser.CheckedItems[i].ToString().Split(':')[0]);

                string sql = "insert into tbl_Test_User(UserId,TestId) values("
                + "" + userId.ToString() + ""
                + "," + TestId.ToString() + ")";
                ret = ret && db.ExeSQL(sql);
            }
            return ret;
        }
        /// <summary>
        /// 考试选题插入
        /// </summary>
        /// <param name="TestId"></param>
        /// <returns></returns>
        private bool InsertTestDetail(int TestId, int idx, DataSet ds,int QuestionType)
        {
            bool ret = true;
            string sql = "insert into tbl_Test_Detail(TestId,QuestionId,QuestionType) values("
            + "" + TestId.ToString() + ""
            + "," + ds.Tables[0].Rows[idx]["ID"].ToString() + ""
            + "," + QuestionType.ToString() + ")";
            ret = ret && db.ExeSQL(sql);

            return ret;
        }
        /// <summary>
        /// 考试选题
        /// </summary>
        private bool CreateTestDetail(int TestId)
        {
            bool ret = true;
            string sql = "select id from tbl_Question_Single";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Random rdm = new Random();
                Hashtable ht = new Hashtable();
                int total = ds.Tables[0].Rows.Count;

                int userNum = clbTestUser.CheckedItems.Count;
                int Bidati = Convert.ToInt32(nudBidati.Value)*userNum;
                int i = 0;
                while ( i < Bidati)
                {
                    int select = rdm.Next(0, total - 1);
                    if (ht[select.ToString()] == null)
                    {
                        ht[select.ToString()] = select.ToString();
                        i++;
                        ret = ret && InsertTestDetail(TestId, select, ds, 1);
                    }
                }

                int Qiangdati = Convert.ToInt32(nudQiangdati.Value);
                i = 0;
                while (i < Qiangdati)
                {
                    int select = rdm.Next(0, total - 1);
                    if (ht[select.ToString()] == null)
                    {
                        ht[select.ToString()] = select.ToString();
                        i++;
                        ret = ret && InsertTestDetail(TestId, select, ds, 2);
                    }
                }
            }
            else
            {
                MessageBox.Show("题库为空！");
                ret = false;
            }
            return ret;
        }

    }
}
