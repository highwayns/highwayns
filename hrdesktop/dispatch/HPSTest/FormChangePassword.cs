using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HEDAS.Lib;

namespace HPSTest
{
    public partial class FormChangePassword : Form
    {
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        public FormChangePassword(int userId)
        {
            InitializeComponent();
            getUserId(userId);
        }
        /// <summary>
        /// 取得用户ID
        /// </summary>
        /// <param name="userId"></param>
        private void getUserId(int userId)
        {
            string sql = "select Mail from tbl_User where id=" + userId ;
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtUserId.Text = ds.Tables[0].Rows[0][0].ToString();
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                newTxtPassword.Focus();
            }
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newpasswordConfirm_Click(object sender, EventArgs e)
        {
            string sql = "select * from tbl_User where Mail='" + txtUserId.Text + "' and password='" + txtPassword.Text + "'";
            DataSet ds = db.ReturnDataSet(sql);
            //判断四个文本框内是否有空项没填写
            if ( txtPassword.Text.Trim().Length == 0)
               {
                 MessageBox.Show("请输入旧密码！");
                 txtPassword.Focus();
                 return;
               }
            else if (newTxtPassword.Text.Trim().Length == 0) 
                {
                 MessageBox.Show("请输入新密码！");
                 newTxtPassword.Focus();
                 return;
                }
            else if (newPWordConfirmagain.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入确认密码！");
                newPWordConfirmagain.Focus();
                return;
            }

            //判断两次输入的新密码是否一致
            if (newTxtPassword.Text.Trim() != newPWordConfirmagain.Text.Trim())
            {
                MessageBox.Show("新密码和确认密码不一致！");
                return;
            }

            //判断新旧密码是否相同
            if (txtPassword.Text.Trim() == newPWordConfirmagain.Text.Trim())
            {
                MessageBox.Show("旧密码与新密码相同！");
                return;
            }


            //写入新密码
            if (ds.Tables[0].Rows.Count == 1)
            {
                sql = "update tbl_User set [password] = '" + newTxtPassword.Text + "' where Mail='" + txtUserId.Text + "';";
                if (db.ExeSQL(sql))
                {
                    MessageBox.Show("密码更新成功！");
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("密码更新失败！");
                }
            }
            else
            {
                sql = "select * from tbl_User where Mail='" + txtUserId.Text + "'";
                ds = db.ReturnDataSet(sql);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    MessageBox.Show("旧密码不正确！");
                }
                else
                {
                    MessageBox.Show("用户不存在！");
                }
            }
        }
        /// <summary>
        /// 新密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newTxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                newTxtPassword.Focus();
            }
        }
        /// <summary>
        /// 新密码确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newPWordConfirmagain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                newpasswordConfirm_Click(null, null);
            }
        }
        /// <summary>
        /// Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
   }
}
