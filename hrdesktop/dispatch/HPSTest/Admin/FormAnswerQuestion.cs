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
    public partial class FormAnswerQuestion : Form
    {
        private int testId = 0;
        private int num;//试题编号
        private string answer;//
        UserInfor[] userInfors;
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        private const int Max_User_Count = 10;
        private int index = 0;
        public FormAnswerQuestion(int testId)
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
                userInfors = new UserInfor[userCount];
                for (int i = 0; i < userCount; i++)
                {
                    userInfors[i] = new UserInfor(db); 
                    string userName = (string)ds.Tables[0].Rows[i]["userName"];
                    userInfors[i].setUserInfor(userName,0,0);
                    int userId = (int)ds.Tables[0].Rows[i]["userId"];
                    userInfors[i].setUserId(userId);
                    userInfors[i].Parent = this;
                    userInfors[i].Top = 300 + i * (userInfors[i].Height + 2);
                    userInfors[i].Left = 44;
                    userInfors[i].OnClick += new UserInfor.ClickHandler(btn_OnClick);
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
            for (int i = 0; i < userInfors.Length; i++)
            {
                userInfors[i].setTestInfo(testId, questionId, questionType, real_answer);
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
                if (questionType == 1)
                {
                    index = 0;
                    userInfors[index].Select();
                }
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
                if (questionType == 1)
                {
                    index = 0;
                    userInfors[index].Select();
                }
            }
            else
            {
                MessageBox.Show("竞赛结束!");
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
            clear();
            switch (answer)
            {
                case "A": txtA.BackColor= Color.Blue; break;
                case "B": txtB.BackColor = Color.Blue; break;
                case "C": txtC.BackColor = Color.Blue; break;
                case "D": txtD.BackColor = Color.Blue; break;
            }

        }
        /// <summary>
        /// 清除答案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clear()
        {
            txtA.BackColor = Color.Gray;
            txtB.BackColor = Color.Gray; 
            txtC.BackColor = Color.Gray; 
            txtD.BackColor = Color.Gray;
        }
        /// <summary>
        /// 清除其他选中用户和选择答案
        /// </summary>
        public void btn_OnClick(object sender, UserEventArgs e)
        {
            clear();
            for (int j = 0; j < userInfors.Length; j++)
            {
                if (userInfors[j].getUserId() != e.userid)
                {
                    userInfors[j].Clear();
                }
                else
                {
                    switch (userInfors[j].temp_answer)
                    {
                        case "A": txtA.BackColor = Color.Blue; break;
                        case "B": txtB.BackColor = Color.Blue; break;
                        case "C": txtC.BackColor = Color.Blue; break;
                        case "D": txtD.BackColor = Color.Blue; break;
                    }
                }
            }
        }

        /// <summary>
        /// 清除答题标志
        /// </summary>
        /// <param name="Questiontype"></param>
        private void setBtnEnableByQuestionType(int Questiontype)
        {
            for (int j = 0; j < userInfors.Length; j++)
            {
                userInfors[j].Clear();
                userInfors[j].temp_answer = "";
            }
            if (Questiontype == 1)
            {
                btnSetScore.Enabled = true;
            }
            else
            {
                btnSetScore.Enabled = false;
            }
        }
        /// <summary>
        /// 更新排名
        /// </summary>
        private void updateRank()
        {
            string sql = "select ID,userId,Score from tbl_User_Test where testId=" + testId.ToString() + " order by Score desc";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int prevScore = -1;
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
                    for (int j = 0; j < userInfors.Length; j++)
                    {
                        if (userInfors[j].getUserId() == userId)
                        {
                            userInfors[j].setRank(rank, prevScore);
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// A选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtA_Click(object sender, EventArgs e)
        {
            clear();
            txtA.BackColor = Color.Blue;
            for (int j = 0; j < userInfors.Length; j++)
            {
                if (userInfors[j].isSelected)
                {
                    if (lblQuestionType.Text == "必答题")
                    {
                        userInfors[j].temp_answer="A";
                    }
                    else
                    {
                        userInfors[j].setScore("A");
                        updateRank();
                    }
                    break;
                }
            }
            setScore();
        }
        /// <summary>
        /// B选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtB_Click(object sender, EventArgs e)
        {
            clear();
            txtB.BackColor = Color.Blue;
            for (int j = 0; j < userInfors.Length; j++)
            {
                if (userInfors[j].isSelected)
                {
                    if (lblQuestionType.Text == "必答题")
                    {
                        userInfors[j].temp_answer = "B";
                    }
                    else
                    {
                        userInfors[j].setScore("B");
                        updateRank();
                    }
                    break;
                }
            }
            setScore();
        }
        /// <summary>
        /// C选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtC_Click(object sender, EventArgs e)
        {
            clear();
            txtC.BackColor = Color.Blue;
            for (int j = 0; j < userInfors.Length; j++)
            {
                if (userInfors[j].isSelected)
                {
                    if (lblQuestionType.Text == "必答题")
                    {
                        userInfors[j].temp_answer = "C";
                    }
                    else
                    {
                        userInfors[j].setScore("C");
                        updateRank();
                    }
                    break;
                }
            }
            setScore();
        }
        /// <summary>
        /// D选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtD_Click(object sender, EventArgs e)
        {
            clear();
            txtD.BackColor = Color.Blue;
            for (int j = 0; j < userInfors.Length; j++)
            {
                if (userInfors[j].isSelected)
                {
                    if (lblQuestionType.Text == "必答题")
                    {
                        userInfors[j].temp_answer = "D";
                    }
                    else
                    {
                        userInfors[j].setScore("D");
                        updateRank();
                    }
                    break;
                }
            }
            setScore();
        }
        /// <summary>
        /// 评分
        /// </summary>
        private void setScore()
        {
            Application.DoEvents();
            System.Threading.Thread.Sleep(1000);

            if (lblQuestionType.Text == "必答题")
            {
                if (userInfors[index].isSelected)
                {
                    index++;
                    if (index < userInfors.Length)
                    {
                        clear();
                        userInfors[index - 1].Clear();
                        userInfors[index].Select();
                    }
                    else
                    {
                        for (int j = 0; j < userInfors.Length; j++)
                        {
                            userInfors[j].setScore(userInfors[j].temp_answer);
                        }
                        updateRank();
                        getNext();
                    }
                }
            }
            else
            {
                bool isSelected = false;
                for (int j = 0; j < userInfors.Length; j++)
                {
                    if (userInfors[j].isSelected)
                    {
                        isSelected = true;
                        break;
                    }
                }
                if (isSelected)
                {
                    getNext();
                }
                else
                {
                    MessageBox.Show("请选择参赛者！");
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
            for (int j = 0; j < userInfors.Length; j++)
            {
                userInfors[j].setScore(userInfors[j].temp_answer);
            }
            updateRank();
        }

    }
}
