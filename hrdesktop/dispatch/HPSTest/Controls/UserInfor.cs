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
    public class UserEventArgs : EventArgs
    {
        public int userid;
        public UserEventArgs(int userid)
        {
            this.userid = userid;
        }
    }
    public partial class UserInfor : UserControl
    {
        private int userId = 0;
        private int testId = 0;
        private int questionId = 0;
        private int questionType = 0;
        public string real_answer = "";
        public string temp_answer = "";
        private CmWinServiceAPI db;

        public delegate void ClickHandler(object sender, UserEventArgs e);
        public event ClickHandler OnClick;
        public bool isSelected = false;
        private string userName = "";
        private int score = 0;
        private int rank = 0;

        public UserInfor(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }
        public UserInfor()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 设定用户信息
        /// </summary>
        public void setUserInfor(string userName,int score,int rank)
        {
            this.userName = userName;
            this.score = score;
            this.rank = rank;
            updateText();
        }
        /// <summary>
        /// 文本更新
        /// </summary>
        private void updateText()
        {
            btnConfirm.Text = userName + ":"
                + string.Format("{0}分", score) + " "
                + string.Format("第{0}名", rank);
            if (rank == 1)
            {
                btnConfirm.ForeColor = Color.Red;
            }
            else if (rank == 2)
            {
                btnConfirm.ForeColor = Color.Yellow;
            }
            else
            {
                btnConfirm.ForeColor = Color.Black;
            }
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
        /// 设定排名
        /// </summary>
        public void setRank(int rank,int score)
        {
            this.rank = rank;
            this.score = score;
            updateText();
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
        /// 必答评分
        /// </summary>
        public void setScore(string answer)
        {
            if (!getAnswer())
            {
                if (setAnswer(answer, answer == real_answer))
                {
                }
            }
        }
        /// <summary>
        /// 清除选中标志
        /// </summary>
        public void Clear()
        {
            btnConfirm.BackColor = Color.Gray;
            isSelected = false;
        }
        /// <summary>
        /// 设定选中标志
        /// </summary>
        public void Select()
        {
            btnConfirm.BackColor = Color.LightBlue;
            isSelected = true;
        }
        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (questionType == 2)
            {
                btnConfirm.BackColor = Color.LightBlue;
                isSelected = true;
                OnClick(sender, new UserEventArgs(userId));
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
                    this.score = total;
                    updateText();
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
                    this.score = score;
                    updateText();
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
