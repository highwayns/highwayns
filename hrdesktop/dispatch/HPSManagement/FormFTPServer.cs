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
    public partial class FormFTPServer : Form
    {
        private CmWinServiceAPI db;
        public FormFTPServer(CmWinServiceAPI db)
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
            if (db.GetFTPServer(0, 0, "*", "", "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
            }

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtId.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtAddress.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtFolder.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtPort.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtUser.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                txtPassword.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            }

        }
        /// <summary>
        /// 增加服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = 0;
            int port =0;
            try
            {
                port = Convert.ToInt32(txtPort.Text);
            }
            catch{}
            String fieldlist = "名称,地址,文件夹,端口,用户,密码,UserID";
            String valuelist = "'" + txtName.Text + "','" + txtAddress.Text + "','" + txtFolder.Text + "',"
                + port.ToString() + ",'" + txtUser.Text + "','" + txtPassword.Text 
                + "','" +db.UserID+"'";
            if (db.SetFTPServer(0, 0, fieldlist,
                                 "", valuelist, out id))
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0020I", db.Language);
                MessageBox.Show(msg);
                init();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0021I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 更新服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "编号=" + txtId.Text;
                String valuesql = "名称='" + txtName.Text 
                    + "',地址='" + txtAddress.Text
                    + "',文件夹='" + txtFolder.Text
                    + "',端口=" + txtPort.Text 
                    + ",用户='" + txtUser.Text
                    + "',密码='" + txtPassword.Text + "'";
                if (db.SetFTPServer(0, 1, "", wheresql, valuesql, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0022I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0023I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0019I", db.Language);
                MessageBox.Show(msg);
            }


        }
        /// <summary>
        /// 删除服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "编号=" + txtId.Text;
                if (db.SetFTPServer(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0024I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0025I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0019I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// FTP服务器测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (txtTestTo.Text == "")
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0019I", db.Language);
                    MessageBox.Show(msg);
                    return;
                }
                NCFTP ftp = new NCFTP();
                String filename = txtTestTo.Text;
                String url ="ftp://"+txtAddress.Text+"/"+txtFolder.Text+"/"+System.IO.Path.GetFileName(filename);

                if (ftp.uploadFile(url, filename, txtUser.Text, txtPassword.Text))
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0027I", db.Language);
                    MessageBox.Show(msg);
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0028I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0019I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 选择测试文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtTestTo.Text = dialog.FileName;
            }
        }
    }
}
