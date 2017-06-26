using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HEDAS.Lib;

namespace HPSTest.Admin
{
    public partial class FormQuestionSingle : Form
    {
        private int num;//试题编号
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        public FormQuestionSingle()
        {
            InitializeComponent();
            getFirst();
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
        /// 读取第一条记录
        /// </summary>
        private void getFirst()
        {
            string sql = "select top 1 * from tbl_Question_Single";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                num = (int)ds.Tables[0].Rows[0]["ID"];
                lblNo.Text = num.ToString();
                txtContent.Text = (string)ds.Tables[0].Rows[0]["Content"];
                txtA.Text = (string)ds.Tables[0].Rows[0]["SelectionA"];
                txtB.Text = (string)ds.Tables[0].Rows[0]["SelectionB"];
                txtC.Text = (string)ds.Tables[0].Rows[0]["SelectionC"];
                txtD.Text = (string)ds.Tables[0].Rows[0]["SelectionD"];
                nudScore.Value = (int)ds.Tables[0].Rows[0]["Score"]; 
                string answer = (string)ds.Tables[0].Rows[0]["Answer"];
                switch (answer)
                {
                    case "A": rdoA.Checked = true; break;
                    case "B": rdoB.Checked = true; break;
                    case "C": rdoC.Checked = true; break;
                    case "D": rdoD.Checked = true; break;
                }
            }
        }
        /// <summary>
        /// 读取最后一条记录
        /// </summary>
        private void getLast()
        {
            string sql = "select top 1 * from tbl_Question_Single order by ID desc";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                num = (int)ds.Tables[0].Rows[0]["ID"];
                lblNo.Text = num.ToString();
                txtContent.Text = (string)ds.Tables[0].Rows[0]["Content"];
                txtA.Text = (string)ds.Tables[0].Rows[0]["SelectionA"];
                txtB.Text = (string)ds.Tables[0].Rows[0]["SelectionB"];
                txtC.Text = (string)ds.Tables[0].Rows[0]["SelectionC"];
                txtD.Text = (string)ds.Tables[0].Rows[0]["SelectionD"];
                nudScore.Value = (int)ds.Tables[0].Rows[0]["Score"]; 
                string answer = (string)ds.Tables[0].Rows[0]["Answer"];
                switch (answer)
                {
                    case "A": rdoA.Checked = true; break;
                    case "B": rdoB.Checked = true; break;
                    case "C": rdoC.Checked = true; break;
                    case "D": rdoD.Checked = true; break;
                }
            }
        }

        /// <summary>
        /// 读取下一条记录
        /// </summary>
        private void getNext()
        {
            string sql = "select top 1 * from tbl_Question_Single where ID>"+num.ToString();
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                num = (int)ds.Tables[0].Rows[0]["ID"];
                lblNo.Text = num.ToString();
                txtContent.Text = (string)ds.Tables[0].Rows[0]["Content"];
                txtA.Text = (string)ds.Tables[0].Rows[0]["SelectionA"];
                txtB.Text = (string)ds.Tables[0].Rows[0]["SelectionB"];
                txtC.Text = (string)ds.Tables[0].Rows[0]["SelectionC"];
                txtD.Text = (string)ds.Tables[0].Rows[0]["SelectionD"];
                nudScore.Value = (int)ds.Tables[0].Rows[0]["Score"]; 
                string answer = (string)ds.Tables[0].Rows[0]["Answer"];
                switch (answer)
                {
                    case "A": rdoA.Checked = true; break;
                    case "B": rdoB.Checked = true; break;
                    case "C": rdoC.Checked = true; break;
                    case "D": rdoD.Checked = true; break;
                }
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
            string sql = "select top 1 * from tbl_Question_Single where ID>=" + (num-1).ToString();
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                num = (int)ds.Tables[0].Rows[0]["ID"];
                lblNo.Text = num.ToString();
                txtContent.Text = (string)ds.Tables[0].Rows[0]["Content"];
                txtA.Text = (string)ds.Tables[0].Rows[0]["SelectionA"];
                txtB.Text = (string)ds.Tables[0].Rows[0]["SelectionB"];
                txtC.Text = (string)ds.Tables[0].Rows[0]["SelectionC"];
                txtD.Text = (string)ds.Tables[0].Rows[0]["SelectionD"];
                nudScore.Value = (int)ds.Tables[0].Rows[0]["Score"]; 
                string answer = (string)ds.Tables[0].Rows[0]["Answer"];
                switch (answer)
                {
                    case "A": rdoA.Checked = true; break;
                    case "B": rdoB.Checked = true; break;
                    case "C": rdoC.Checked = true; break;
                    case "D": rdoD.Checked = true; break;
                }
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
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            update();
        }
        /// <summary>
        /// 更新当前记录
        /// </summary>
        private void update()
        {
            int id = Convert.ToInt32(lblNo.Text);
            string content = txtContent.Text;
            string answerA = txtA.Text;
            string answerB = txtB.Text;
            string answerC = txtC.Text;
            string answerD = txtD.Text;
            int score = (int)nudScore.Value;
            string answer = "A";
            if (rdoB.Checked) answer = "B";
            else if (rdoC.Checked) answer = "C";
            else if (rdoD.Checked) answer = "D";

            string sql = "update tbl_Question_Single set "
            + "Content='" + content+"'"
            + ",SelectionA='" + answerA + "'"
            + ",SelectionB='" + answerB + "'"
            + ",SelectionC='" + answerC + "'"
            + ",SelectionD='" + answerD + "'"
            + ",Score=" + score + ""
            + ",Answer='" + answer + "'"
            + " where ID=" + id.ToString();
            bool ret = db.ExeSQL(sql);
            if (ret)
            {
                MessageBox.Show("更新成功!");
            }
            else
            {
                MessageBox.Show("更新失败!");
            }
        }
        /// <summary>
        /// 焦点设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormQuestionSingle_Activated(object sender, EventArgs e)
        {
            txtContent.Focus();
        }

    }
}
