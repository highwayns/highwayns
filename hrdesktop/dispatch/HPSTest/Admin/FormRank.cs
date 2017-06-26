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
    public partial class FormRank : Form
    {
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        public FormRank()
        {
            InitializeComponent();
            getAllTest();
            getAllUser();
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
        /// 读取所有用户
        /// </summary>
        private void getAllUser()
        {
            string sql = "select a.Id,a.UserName from tbl_User a";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cmbUser.Items.Add(ds.Tables[0].Rows[i]["ID"].ToString() + ":"
                        + ds.Tables[0].Rows[i]["UserName"].ToString());
                }
            }
        }
        /// <summary>
        /// 读取会议编号
        /// </summary>
        private void getAllTest()
        {
            string sql = "select a.Id,a.TestName from tbl_Test a";
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
        /// 读取排名信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTest.Text != "")
            {
                int testId = Convert.ToInt32(cmbTest.Text.ToString().Split(':')[0]);
                string sql = "select c.testName,b.userName,a.score,a.rank from tbl_User_Test a,tbl_User b,tbl_Test c where a.testId = " + testId
                    + " and a.userId=b.ID and a.testId = c.id and a.rank<4 order by a.score desc";
                DataSet ds = db.ReturnDataSet(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dgvRank.DataSource = ds.Tables[0];
                }
                else
                {
                    dgvRank.DataSource = null;
                }
                cmbUser.Text = "";
            }
            else
            {
                MessageBox.Show("请选择会议！");
            }

        }
        /// <summary>
        /// 用户选择变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUser.Text != "")
            {
                int userId = Convert.ToInt32(cmbUser.Text.ToString().Split(':')[0]);
                string sql = "select c.testName,b.userName,a.score,a.rank from tbl_User_Test a,tbl_User b,tbl_Test c where b.Id = " + userId
                    + " and a.userId=b.ID and c.id = a.testId and a.rank<4 order by a.score desc";
                DataSet ds = db.ReturnDataSet(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dgvRank.DataSource = ds.Tables[0];
                }
                else
                {
                    dgvRank.DataSource = null;
                }
                cmbTest.Text = "";
            }
            else
            {
                MessageBox.Show("请选择会议！");
            }
        }

    }
}
