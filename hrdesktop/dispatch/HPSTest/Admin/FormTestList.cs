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
    public partial class FormTestList : Form
    {
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        public FormTestList()
        {
            InitializeComponent();
            getAllTest();
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
        /// 读取用户信息
        /// </summary>
        private void getAllTest()
        {
            string sql = "select * from tbl_Test";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dgvUser.DataSource = ds.Tables[0];
            }
        }
        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormTestManager form = new FormTestManager();
            form.ShowDialog();
            getAllTest();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = dgvUser.SelectedRows[0].Cells[0].Value.ToString();
            string sql = "delete from tbl_test where id="+id;
            if (db.ExeSQL(sql))
            {
                sql = "delete from tbl_Test_Detail where testid="+id;
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
                MessageBox.Show("清除成功！");
                getAllTest();
            }
            else
            {
                MessageBox.Show("删除失败！");
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = (int)(dgvUser.SelectedRows[0].Cells[0].Value);
            FormTestManager form = new FormTestManager(id);
            form.ShowDialog();
            getAllTest();
        }

    }
}
