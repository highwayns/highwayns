using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HEDAS.Lib;

namespace HPSTest.Controls
{
    public partial class UserTest : UserControl
    {
        private int userId = 0;
        private int testId = 0;
        private int questionId = 0;
        private int questionType = 0;
        private string real_answer = "";
        private  CmWinServiceAPI db;
        public delegate void ClickHandler(object sender, EventArgs e);
        public event ClickHandler OnClick;
        public UserTest(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }
        public UserTest()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 设定姓名
        /// </summary>
        public void setUserName(string userName)
        {
            lblName.Text = userName;
            lblScore.Text = "0分";
        }
        /// <summary>
        /// 设定排名
        /// </summary>
        public void setRank(int rank)
        {
            lblRank.Text = string.Format("第{0}名",rank);
        }
        /// <summary>
        /// 设定编号
        /// </summary>
        public void setUserId(int userId)
        {
            this.userId = userId;
        }
        /// <summary>
        /// 取得编号
        /// </summary>
        public int getUserId()
        {
            return this.userId;
        }
        /// <summary>
        /// 设定答题信息
        /// </summary>
        public void setTestInfo(int testId, int questionId, int questionType, string real_answer)
        {
            this.testId = testId;
            this.questionId = questionId;
            this.questionType = questionType;
            this.real_answer = real_answer;
        }

        /// <summary>
        /// 清除答案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void clear()
        {
            rdoA.Checked = false; 
            rdoB.Checked = false; 
            rdoC.Checked = false; 
            rdoD.Checked = false;             
        }
        /// <summary>
        /// 必答评分
        /// </summary>
        public void setScore()
        {
            string answer = "A";
            if (rdoB.Checked) answer = "B";
            else if (rdoC.Checked) answer = "C";
            else if (rdoD.Checked) answer = "D";
            if (!getAnswer())
            {
                if (setAnswer(answer, answer == real_answer))
                {
                }
            }
        }
        /// <summary>
        /// 设置按钮是否可用
        /// </summary>
        /// <param name="isEnabe"></param>
        public void setBtnEnable(bool isEnabe)
        {
            btnConfirm.Enabled = isEnabe;
        }
        /// <summary>
        /// 抢答评分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string answer = "A";
            if (rdoB.Checked) answer = "B";
            else if (rdoC.Checked) answer = "C";
            else if (rdoD.Checked) answer = "D";
            if (answer == real_answer)
            {
                if (!getAnswer())
                {
                    if (setAnswer(answer,true))
                    {
                        OnClick(sender, e);
                    }
                }
            }
            else
            {
                if (!getAnswer())
                {
                    if (setAnswer(answer,false))
                    {
                        //
                    }
                }
                MessageBox.Show("对不起！答错了！请继续努力！");
            }
        }
        /// <summary>
        /// 读取分数
        /// </summary>
        private int getScore()
        {
            string sql = "select Score from tbl_Question_Single where ID="+questionId.ToString();
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return (int)ds.Tables[0].Rows[0]["Score"];
            }
            return 0;
        }
        /// <summary>
        /// 保存用户分数
        /// </summary>
        private void setUserTest(int score)
        {
            string sql = "select ID,Score from tbl_User_Test where testId=" + testId.ToString()+" and userId="+userId.ToString();
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                int Id = (int)ds.Tables[0].Rows[0]["ID"];
                int total = score + (int)ds.Tables[0].Rows[0]["Score"];
                sql = "update tbl_User_Test set Score = " + total + " where Id=" + Id.ToString();
                if (!db.ExeSQL(sql))
                {
                    MessageBox.Show("分数更新失败！");
                }
                else
                {
                    lblScore.Text = total.ToString()+"分";
                }
            }
            else
            {
                sql = "insert into tbl_User_Test(userId,testId,Score) values( "
                + "" + userId + ""
                + "," + testId + ""
                + "," + score + ")";
                if (!db.ExeSQL(sql))
                {
                    MessageBox.Show("分数更新失败！");
                }
                else
                {
                    lblScore.Text = score.ToString();
                }

            }

        }
        /// <summary>
        /// 读取分数
        /// </summary>
        private bool getAnswer()
        {
            string sql = "select * from tbl_User_test_detail where userId=" + userId.ToString()
                + " and testId="+testId.ToString()
                + " and questionId=" + questionId.ToString()
                ;
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 答题
        /// </summary>
        private bool setAnswer(string Answer,bool isRight)
        {
            string sql = "insert into [tbl_User_test_detail]([userId],[testId],[questionId],[QuestionType],[Answer]) values("
            + "" + userId + ""
            + "," + testId + ""
            + "," + questionId + ""
            + "," + questionType + ""
            + ",'" + Answer + "')";

            bool ret = db.ExeSQL(sql);
            if (ret)
            {
                if (isRight)
                {
                    int score = getScore();
                    setUserTest(score);
                }
            }
            else
            {
                MessageBox.Show("答题追加失败！");
            }
            return ret;
        }

        
    }
}
