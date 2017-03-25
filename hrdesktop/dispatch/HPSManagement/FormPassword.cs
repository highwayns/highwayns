using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;

namespace HPSManagement
{
    public partial class FormPassword : Form
    {
        private CmWinServiceAPI db;
        public FormPassword(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }


        /// <summary>
        /// 画面启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDataBase_Load(object sender, EventArgs e)
        {
            txtUser.Text = db.UserID;
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            String wheresql="UserId='"+txtUser.Text+"' and UserPwd='"+NCCryp.Encrypto(txtOldPwd.Text)+"'";
            if (db.GetUser(0, 0, "*", wheresql, "", ref ds) && ds.Tables[0].Rows.Count == 1)
            {
                int id = 0;
                if (db.SetUser(0, 1, "", wheresql, "UserPwd='" + NCCryp.Encrypto(txtNewPwd.Text) + "'", out id) && id == 1)
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
