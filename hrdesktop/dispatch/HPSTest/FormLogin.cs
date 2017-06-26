using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HEDAS.Lib;
using System.IO;

namespace HPSTest
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        public int userId=0;
        public bool isAdmin = false;
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 画面启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            txtUserId.Focus();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string sql = "select * from tbl_User where Mail='" + txtUserId.Text + "' and password='" + txtPassword.Text + "'";
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                if ((bool)ds.Tables[0].Rows[0]["isadmin"] == true)
                {
                    userId = (int)ds.Tables[0].Rows[0]["ID"];
                    isAdmin = true;

                }
                else
                {
                    userId = (int)ds.Tables[0].Rows[0]["ID"];
                    isAdmin = false;
                }
                DialogResult = DialogResult.OK;
            }
            else
            {
                sql = "select * from tbl_User where Mail='" + txtUserId.Text + "'";
                ds = db.ReturnDataSet(sql);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    MessageBox.Show("密码不正确！");
                }
                else
                {
                    MessageBox.Show("用户不存在！");
                }
            }
        }
        /// <summary>
        /// 回车键处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

    }
}
