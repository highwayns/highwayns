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
    public partial class FormMailServer : Form
    {
        private CmWinServiceAPI db;
        public FormMailServer(CmWinServiceAPI db)
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
            if (db.GetMailServer(0, 0, "编号,名称,地址,端口,用户,密码,送信人地址,服务器类型,添付文件,HTML", "", "", ref ds))
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
                txtPort.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtUser.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtPassword.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                txtSender.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                cmbType.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString().Trim();
                cmbAttachment.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString().Trim();
                cmbISHtml.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString().Trim();
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
            String fieldlist = "名称,地址,端口,用户,密码,送信人地址,服务器类型,UserID,添付文件,HTML";
            String valuelist = "'" + txtName.Text + "','" + txtAddress.Text + "',"
                + port.ToString() + ",'" + txtUser.Text + "','" + txtPassword.Text 
                + "','" + txtSender.Text + "','" + cmbType.Text + "','"+db.UserID+"','"
                +cmbAttachment.Text+"','"+cmbISHtml.Text+"'";
            if (db.SetMailServer(0, 0, fieldlist,
                                 "", valuelist, out id))
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0060I", db.Language);
                MessageBox.Show(msg);
                init();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0061I", db.Language);
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
                    + "',端口=" + txtPort.Text 
                    + ",用户='" + txtUser.Text
                    + "',密码='" + txtPassword.Text
                    + "',送信人地址='" + txtSender.Text
                    + "',服务器类型='" + cmbType.Text
                    + "',添付文件='" + cmbAttachment.Text
                    + "',HTML='" + cmbISHtml.Text + "'"
                    ;
                if (db.SetMailServer(0, 1, "", wheresql, valuesql, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0062I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0063I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0059I", db.Language);
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
                if (db.SetMailServer(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0064I", db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0065I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0059I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 邮件服务器测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                String name = txtName.Text;
                String address = txtAddress.Text;
                String port = txtPort.Text;
                String user = txtUser.Text;
                String password = txtPassword.Text;
                String from = txtSender.Text;
                String servertype = cmbType.Text.Trim();
                String attachement = cmbAttachment.Text.Trim();
                String isHtml = cmbISHtml.Text.Trim();
                String to = txtTestTo.Text;
                NdnPublicFunction function = new NdnPublicFunction();
                if (!function.IsMail(to, false))
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0066I", db.Language);
                    MessageBox.Show(msg);
                    return;
                }
                String subject = "这是测试"+" by " + name;
                String pdffile = "http://www.chojogakuin.com/weekly/20130907.pdf";
                String body = "测试内容\r\n收到邮件说明测试成功\r\nURL="+pdffile;
                String htmlbody = "<html><head></head><body>测试内容<br>收到邮件说明测试成功</body></html>";
                String picfile = @"c:\test.jpg";
                if (isHtml != "Y")
                {
                    htmlbody = null;
                }
                if (attachement != "Y")
                {
                    picfile = null;
                }
                if (NCMail.SendEmail(subject, body, htmlbody, picfile, address, user, password, from, to, servertype))
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0067I", db.Language);
                    MessageBox.Show(msg);
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0068I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0059I", db.Language);
                MessageBox.Show(msg);
            }
        }
    }
}
