using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HEDAS.Lib;

namespace HPSTest.User
{
    public partial class FormTestView : Form
    {
        private int userId = 0;
        private int testId =-1;
        private int num;//试题编号
        private string answer;//

        private static CmWinServiceAPI db = new CmWinServiceAPI();
        public FormTestView(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            getTestByUserId();
        }
        /// <summary>
        /// 读取会议编号
        /// </summary>
        private void getTestByUserId()
        {
            string sql = "select a.Id,a.TestName from tbl_Test a,tbl_Test_User b where b.UserId ="
                + userId.ToString() + " and a.Id=b.TestId";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cmbTest.Items.Add(ds.Tables[0].Rows[i]["ID"].ToString() + ":"
                        + ds.Tables[0].Rows[i]["TestName"].ToString());
                }
            }
        }
        /// <summary>
        /// 选择变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTest.Text != "")
            {
                testId = Convert.ToInt32(cmbTest.Text.ToString().Split(':')[0]);
                getFirst();
            }
            else
            {
                MessageBox.Show("请选择会议！");
            }

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
        /// 读取用户答案
        /// </summary>
        private void getUserAnswer()
        {
            string sql = "select * from tbl_User_Test_Detail where QuestionId="+num+" and testId="
                + testId.ToString() + " and userId ="+userId;
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                lblUserAnswer.Text = (string)ds.Tables[0].Rows[0]["Answer"];
            }
            else
            {
                lblUserAnswer.Text = "";
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
                setAnswer();
                int questionType = (int)ds.Tables[0].Rows[0]["QuestionType"];
                switch (questionType)
                {
                    case 1: lblQuestionType.Text = "必答题"; break;
                    case 2: lblQuestionType.Text = "抢答题"; break;
                }
                getUserAnswer();
            }
        }
        /// <summary>
        /// 读取最后一条记录
        /// </summary>
        private void getLast()
        {
            string sql = "select top 1 a.*,b.QuestionType from tbl_Question_Single a,tbl_Test_Detail b where a.ID = b.QuestionId and b.testId="
                + testId.ToString() + " order by a.ID desc";
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
                setAnswer();
                int questionType = (int)ds.Tables[0].Rows[0]["QuestionType"];
                switch (questionType)
                {
                    case 1: lblQuestionType.Text = "必答题"; break;
                    case 2: lblQuestionType.Text = "抢答题"; break;
                }
                getUserAnswer();

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
                setAnswer();
                int questionType = (int)ds.Tables[0].Rows[0]["QuestionType"];
                switch (questionType)
                {
                    case 1: lblQuestionType.Text = "必答题"; break;
                    case 2: lblQuestionType.Text = "抢答题"; break;
                }
                getUserAnswer();

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
                + testId.ToString() + " and a.ID<" + num.ToString() + " order by a.ID desc";
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
                setAnswer();
                int questionType = (int)ds.Tables[0].Rows[0]["QuestionType"];
                switch (questionType)
                {
                    case 1: lblQuestionType.Text = "必答题"; break;
                    case 2: lblQuestionType.Text = "抢答题"; break;
                }
                getUserAnswer();

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
        private void setAnswer()
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
    }

}
