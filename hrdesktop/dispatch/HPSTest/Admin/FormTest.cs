using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HEDAS.Lib;
using HPSTest.Controls;

namespace HPSTest.Admin
{
    public partial class FormTest : Form
    {
        private int testId = 0;
        private int num;//试题编号
        private string answer;//
        UserTest[] userTests;
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        private const int Max_User_Count = 10;
        public FormTest(int testId)
        {
            this.testId = testId;
            InitializeComponent();
            getTestUser();
            getFirst();
        }
        /// <summary>
        /// 读取分数
        /// </summary>
        private void getTestUser()
        {
            string sql = "select a.userId,b.userName from tbl_Test_User a,tbl_User b where a.userId=b.ID and a.testId=" + testId.ToString();
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int userCount = ds.Tables[0].Rows.Count;
                if (userCount > Max_User_Count) userCount = Max_User_Count;
                userTests = new UserTest[userCount];
                for (int i = 0; i < userCount; i++)
                {
                    userTests[i] = new UserTest(db); 
                    string userName = (string)ds.Tables[0].Rows[i]["userName"];
                    userTests[i].setUserName(userName);
                    int userId = (int)ds.Tables[0].Rows[i]["userId"];
                    userTests[i].setUserId(userId);
                    userTests[i].Parent = this;
                    userTests[i].Top = 346 + (i / 5 ) * (userTests[i].Height+2);
                    userTests[i].Left = 44 + (i%5) * (userTests[i].Width+2);
                    userTests[i].OnClick += new UserTest.ClickHandler(btn_OnClick);
                }
            }
            else
            {
                MessageBox.Show("没有参赛者！");
            }
        }
        /// <summary>
        /// 设置考题信息
        /// </summary>
        private void setTestInfo(int testId, int questionId, int questionType, string real_answer)
        {
            for (int i = 0; i < userTests.Length; i++)
            {
                userTests[i].clear();
                userTests[i].setTestInfo(testId,questionId,questionType,real_answer);
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你真的要退出吗？", "提问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Close();
            }
        }
        /// <summary>
        /// 读取第一条记录
        /// </summary>
        private void getFirst()
        {
            string sql = "select top 1 a.*,b.QuestionType from tbl_Question_Single a,tbl_Test_Detail b where a.ID = b.QuestionId and b.testId="
                + testId.ToString() + " order by a.ID";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                clear();
                num = (int)ds.Tables[0].Rows[0]["ID"];
                lblNo.Text = num.ToString();
                txtContent.Text = (string)ds.Tables[0].Rows[0]["Content"];
                txtA.Text = (string)ds.Tables[0].Rows[0]["SelectionA"];
                txtB.Text = (string)ds.Tables[0].Rows[0]["SelectionB"];
                txtC.Text = (string)ds.Tables[0].Rows[0]["SelectionC"];
                txtD.Text = (string)ds.Tables[0].Rows[0]["SelectionD"];
                answer = (string)ds.Tables[0].Rows[0]["Answer"];
                int questionType = (int)ds.Tables[0].Rows[0]["QuestionType"];
                switch (questionType)
                {
                    case 1: lblQuestionType.Text = "必答题"; break;
                    case 2: lblQuestionType.Text = "抢答题"; break;
                }
                setTestInfo(testId, num, questionType, answer);
                setBtnEnableByQuestionType(questionType);
            }
        }
        /// <summary>
        /// 读取最后一条记录
        /// </summary>
        private void getLast()
        {
            string sql = "select top 1 a.*,b.QuestionType from tbl_Question_Single a,tbl_Test_Detail b where a.ID = b.QuestionId and b.testId="
                + testId.ToString()+" order by a.ID desc";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                clear();
                num = (int)ds.Tables[0].Rows[0]["ID"];
                lblNo.Text = num.ToString();
                txtContent.Text = (string)ds.Tables[0].Rows[0]["Content"];
                txtA.Text = (string)ds.Tables[0].Rows[0]["SelectionA"];
                txtB.Text = (string)ds.Tables[0].Rows[0]["SelectionB"];
                txtC.Text = (string)ds.Tables[0].Rows[0]["SelectionC"];
                txtD.Text = (string)ds.Tables[0].Rows[0]["SelectionD"];
                answer = (string)ds.Tables[0].Rows[0]["Answer"];
                int questionType = (int)ds.Tables[0].Rows[0]["QuestionType"];
                switch (questionType)
                {
                    case 1: lblQuestionType.Text = "必答题"; break;
                    case 2: lblQuestionType.Text = "抢答题"; break;
                }
                setTestInfo(testId, num, questionType, answer);
                setBtnEnableByQuestionType(questionType);
            }
        }

        /// <summary>
        /// 读取下一条记录
        /// </summary>
        private void getNext()
        {
            string sql = "select top 1 a.*,b.QuestionType from tbl_Question_Single a,tbl_Test_Detail b where a.ID = b.QuestionId and b.testId="
                + testId.ToString() + " and a.ID>" + num.ToString() + " order by a.ID";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                clear();
                num = (int)ds.Tables[0].Rows[0]["ID"];
                lblNo.Text = num.ToString();
                txtContent.Text = (string)ds.Tables[0].Rows[0]["Content"];
                txtA.Text = (string)ds.Tables[0].Rows[0]["SelectionA"];
                txtB.Text = (string)ds.Tables[0].Rows[0]["SelectionB"];
                txtC.Text = (string)ds.Tables[0].Rows[0]["SelectionC"];
                txtD.Text = (string)ds.Tables[0].Rows[0]["SelectionD"];
                answer = (string)ds.Tables[0].Rows[0]["Answer"];
                int questionType = (int)ds.Tables[0].Rows[0]["QuestionType"];
                switch (questionType)
                {
                    case 1: lblQuestionType.Text = "必答题"; break;
                    case 2: lblQuestionType.Text = "抢答题"; break;
                }
                setTestInfo(testId, num, questionType, answer);
                setBtnEnableByQuestionType(questionType);
            }
            else
            {
                MessageBox.Show("下一条记录不存在!");
            }
        }
        /// <summary>
        /// 读取上一条记录
        /// </summary>
        private void getPrev()
        {
            if (num <= 1) return;
            string sql = "select top 1 a.*,b.QuestionType from tbl_Question_Single a,tbl_Test_Detail b where a.ID = b.QuestionId and b.testId="
                + testId.ToString() +" and a.ID<" + num.ToString()+ " order by a.ID desc";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                clear();
                num = (int)ds.Tables[0].Rows[0]["ID"];
                lblNo.Text = num.ToString();
                txtContent.Text = (string)ds.Tables[0].Rows[0]["Content"];
                txtA.Text = (string)ds.Tables[0].Rows[0]["SelectionA"];
                txtB.Text = (string)ds.Tables[0].Rows[0]["SelectionB"];
                txtC.Text = (string)ds.Tables[0].Rows[0]["SelectionC"];
                txtD.Text = (string)ds.Tables[0].Rows[0]["SelectionD"];
                answer = (string)ds.Tables[0].Rows[0]["Answer"];
                int questionType = (int)ds.Tables[0].Rows[0]["QuestionType"];
                switch (questionType)
                {
                    case 1: lblQuestionType.Text = "必答题"; break;
                    case 2: lblQuestionType.Text = "抢答题"; break;
                }
                setTestInfo(testId, num, questionType, answer);
                setBtnEnableByQuestionType(questionType);
            }
            else
            {
                MessageBox.Show("上一条记录不存在!");
            }
        }

        /// <summary>
        /// 读取第一条记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFirst_Click(object sender, EventArgs e)
        {
            getFirst();
        }
        /// <summary>
        /// 读取下一条记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            getNext();
        }
        /// <summary>
        /// 读取前一条记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click(object sender, EventArgs e)
        {
            getPrev();
        }
        /// <summary>
        /// 最后一条记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLast_Click(object sender, EventArgs e)
        {
            getLast();
        }
        /// <summary>
        /// 显示答案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            switch (answer)
            {
                case "A": rdoA.Checked = true; break;
                case "B": rdoB.Checked = true; break;
                case "C": rdoC.Checked = true; break;
                case "D": rdoD.Checked = true; break;
            }

        }
        /// <summary>
        /// 清除答案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clear()
        {
            switch (answer)
            {
                case "A": rdoA.Checked = false; break;
                case "B": rdoB.Checked = false; break;
                case "C": rdoC.Checked = false; break;
                case "D": rdoD.Checked = false; break;
            }
        }
        /// <summary>
        /// 确定用户排名
        /// </summary>
        public void btn_OnClick(object sender, EventArgs e)
        {
            string sql = "select ID,userId,Score from tbl_User_Test where testId=" + testId.ToString() + " order by Score desc";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int prevScore=-1;
                int rank = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int Id = (int)ds.Tables[0].Rows[i]["ID"];
                    int userId = (int)ds.Tables[0].Rows[i]["userId"];
                    int Score = (int)ds.Tables[0].Rows[i]["Score"];
                    if (Score != prevScore)
                    {
                        rank++;
                        prevScore = Score;
                    }
                    sql = "update tbl_User_Test set rank = " + rank + " where Id=" + Id.ToString();
                    if (!db.ExeSQL(sql))
                    {
                        MessageBox.Show("排名更新失败！");
                    }
                    for (int j = 0; j < userTests.Length; j++)
                    {
                        if (userTests[j].getUserId() == userId)
                        {
                            userTests[j].setRank(rank);
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 必答题评分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetScore_Click(object sender, EventArgs e)
        {
            //判分
            for (int j = 0; j < userTests.Length; j++)
            {
                userTests[j].setScore();
            }
            //更新排名
            btn_OnClick(null, null);
        }
        /// <summary>
        /// 根据题型设置按钮
        /// </summary>
        /// <param name="Questiontype"></param>
        private void setBtnEnableByQuestionType(int Questiontype)
        {
            if (Questiontype == 1)//"必答题"
            {
                btnSetScore.Enabled = true;
                //根据题型设置按钮
                for (int j = 0; j < userTests.Length; j++)
                {
                    userTests[j].setBtnEnable(false);
                }                
            }
            else//"抢答题"
            {
                btnSetScore.Enabled = false;
                //根据题型设置按钮
                for (int j = 0; j < userTests.Length; j++)
                {
                    userTests[j].setBtnEnable(true);
                }
            }
        }
    }
}
