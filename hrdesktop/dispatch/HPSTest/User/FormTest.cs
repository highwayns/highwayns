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
    public partial class FormTest : Form
    {
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        private int num;//试题编号
        private string answer;//试题答案
        public FormTest()
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
            string sql = "select top 1 * from tbl_Question_Single  order by ID";
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
            }
        }
        /// <summary>
        /// 读取最后一条记录
        /// </summary>
        private void getLast()
        {
            string sql = "select top 1 * from tbl_Question_Single  order by ID desc";
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
            }
        }

        /// <summary>
        /// 读取下一条记录
        /// </summary>
        private void getNext()
        {
            string sql = "select top 1 * from tbl_Question_Single where  ID>" + num.ToString() + " order by ID";
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
            string sql = "select top 1 * from tbl_Question_Single where  ID<" + num.ToString() + " order by ID desc";
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

    }
}
