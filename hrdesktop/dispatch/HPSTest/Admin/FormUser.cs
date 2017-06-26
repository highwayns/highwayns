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
    public partial class FormUser : Form
    {
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        public FormUser()
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
            string sql = "select ID,userName,Birthday,iif( Sex, '男' , '女') as sex1 ,Mail,Tel,Address from tbl_User where isAdmin=false";
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
            FormRegister form = new FormRegister();
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
            string sql = "delete from tbl_User where id="+id;
            if (db.ExeSQL(sql))
            {
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
            FormRegister form = new FormRegister(id);
            form.ShowDialog();
            getAllTest();
        }

    }
}
