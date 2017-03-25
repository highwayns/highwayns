using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;

namespace HPSManagement
{
    public partial class FormUser : Form
    {
        private CmWinServiceAPI db;
        public FormUser(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }
        /// <summary>
        /// 画面初始化
        /// </summary>
        /// <param e="sender"></param>
        /// <param name="e"></param>
        private void FormServer_Load(object sender, EventArgs e)
        {
            init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (db.GetUser(0, 0, "*", "", "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[2].Visible = false;
            }

        }
        /// <summary>
        /// 行选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtUser.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtPassword.Text = NCCryp.Decrypto(dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
                cmbType.Text = db.UserRightTable[dataGridView1.SelectedRows[0].Cells[3].Value.ToString()].ToString();
            }

        }
        /// <summary>
        /// 增加用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string usertype = getUserType(cmbType.Text);
            if (usertype == null)
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0049I", db.Language);
                MessageBox.Show(msg);
                return;
            }
            int id = 0;
            String fieldlist = "UserID,UserName,UserPwd,UserRight,UpperUserID";
            String valuelist = "'" + txtUser.Text + "','" + txtName.Text + "','"
                + NCCryp.Encrypto(txtPassword.Text) + "','"
                + usertype + "','"+db.UserID+"'";
            if (db.SetUser(0, 0, fieldlist,
                                 "", valuelist, out id))
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0043I", db.Language);
                MessageBox.Show(msg);
                init();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0044I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string usertype = getUserType(cmbType.Text);
                if (usertype == null)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0049I", db.Language);
                    MessageBox.Show(msg);
                    return;
                }
                int id = 0;
                String wheresql = "UserId='" + txtUser.Text + "' and UpperUserID='" + db.UserID + "'";
                String valuesql = "UserName='" + txtName.Text 
                    + "',UserPwd='" + NCCryp.Encrypto(txtPassword.Text)
                    + "',UserRight='" + usertype+ "'";
                if (db.SetUser(0, 1, "", wheresql, valuesql, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0045I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0046I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0042I", db.Language);
                MessageBox.Show(msg);
            }


        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "UserId='" + txtUser.Text+"' and UpperUserID='"+db.UserID+"'";
                if (db.SetUser(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0047I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0048I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0042I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 本地用户类型转换为中文用户类型
        /// </summary>
        /// <param name="userTypeLocal"></param>
        /// <returns></returns>
        private string getUserType(String userTypeLocal)
        {
            foreach (string key in db.UserRightTable.Keys)
            {
                if (db.UserRightTable[key].ToString() == userTypeLocal)
                {
                    return key;
                }
            }
            return null;
        }
    }
}
