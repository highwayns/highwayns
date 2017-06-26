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
    public partial class FormRegister : Form
    {
        private static CmWinServiceAPI db = new CmWinServiceAPI();
        private int userId = -1;
        public FormRegister()
        {
            InitializeComponent();
        }
        public FormRegister(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            getUserById();
        }
        /// <summary>
        /// 取得用户信息
        /// </summary>
        private void getUserById()
        {
            string sql = "select * from tbl_User where id=" + userId;
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                txtUserName.Text = ds.Tables[0].Rows[0]["userName"].ToString();
                txtPassword.Text = ds.Tables[0].Rows[0]["password"].ToString();
                txtPassword2.Text = ds.Tables[0].Rows[0]["password"].ToString();
                //dtpBirthday.Value = (DateTime)ds.Tables[0].Rows[0]["Birthday"];
                txtMail.Text = ds.Tables[0].Rows[0]["mail"].ToString();
                txtTel.Text = ds.Tables[0].Rows[0]["tel"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
                btnRegist.Text = "更新";
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
        /// 用户注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (check())
            {
                regist();
            }
        }
        /// <summary>
        /// 输入检查
        /// </summary>
        private bool check()
        {
            bool ret = false;
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("请输入用户名！");
                txtUserName.Focus();
                return ret;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("请输入密码！");
                txtPassword.Focus();
                return ret;
            }
            if (string.IsNullOrWhiteSpace(txtPassword2.Text))
            {
                MessageBox.Show("请输入密码验证！");
                txtPassword2.Focus();
                return ret;
            }
            if (txtPassword.Text != txtPassword2.Text)
            {
                MessageBox.Show("密码和密码验证不一致！");
                txtPassword2.Focus();
                return ret;
            }
            if (string.IsNullOrWhiteSpace(txtMail.Text))
            {
                MessageBox.Show("请输入邮件！");
                txtMail.Focus();
                return ret;
            }
            if (isMailExist())
            {
                MessageBox.Show("输入邮件已经存在！");
                txtMail.Focus();
                return ret;
            }
            ret = true;
            return ret;
        }
        /// <summary>
        /// 邮件存在检查
        /// </summary>
        private bool isMailExist()
        {
            string sql = "select * from tbl_User where Mail='" + txtMail.Text + "'";
            if (userId > 0)
            {
                sql += " and id<>"+userId;
            }
            DataSet ds = db.ReturnDataSet(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        private void regist()
        {
            string userName = txtUserName.Text;
            string password = txtPassword.Text;
            string birthday = "2016/01/01";//dtpBirthday.Value.ToShortDateString();
            bool sex = true;
            //if (rdoWoman.Checked) sex = false;
            string mail = txtMail.Text;
            string tel = txtTel.Text;
            string address = txtAddress.Text;

            if (userId > 0)
            {
                string sql = "update [tbl_User] set "
                + "[UserName] ='" + userName + "'"
                + ",[Password]='" + password + "'"
                + ",[Birthday]=" + birthday + ""
                + ",[Sex]=" + sex + ""
                + ",[Mail]='" + mail + "'"
                + ",[Tel]='" + tel + "'"
                + ",[Address]='" + address + "'"
                + " where id="+userId;
                bool ret = db.ExeSQL(sql);
                if (ret)
                {
                    MessageBox.Show("更新成功!");
                    Close();
                }
                else
                {
                    MessageBox.Show("更新失败!");
                }
            }
            else
            {
                string sql = "insert into [tbl_User]([UserName],[Password],[Birthday],[Sex],[Mail],[Tel],[Address],[IsAdmin]) values("
                + "'" + userName + "'"
                + ",'" + password + "'"
                + "," + birthday + ""
                + "," + sex + ""
                + ",'" + mail + "'"
                + ",'" + tel + "'"
                + ",'" + address + "'"
                + ",False)";
                bool ret = db.ExeSQL(sql);
                if (ret)
                {
                    MessageBox.Show("注册成功!");
                    Close();
                }
                else
                {
                    MessageBox.Show("注册失败!");
                }
            }

        }
        /// <summary>
        /// 画面启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormRegister_Load(object sender, EventArgs e)
        {
            txtUserName.Focus();
        }
    }
}
