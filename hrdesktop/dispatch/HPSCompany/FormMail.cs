using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using NC.HPS.Lib;
using System.IO;

namespace HPSCompany
{
    public partial class FormMail : Form
    {
        public string[] data=null;
        /// <summary>
        /// 数据库
        /// </summary>
        private DB db;
        public FormMail(string[] data, DB db)
        {
            this.data = data;
            this.db = db;
            InitializeComponent();
        }
        /// <summary>
        /// 送信用構造体
        /// </summary>
        struct MailPara
        {
            public string sendid;
            public string name;
            public string subject;
            public string body;
            public string htmlbody;
            public string picfile;
            public string address;
            public string user;
            public string password;
            public string from;
            public string to;
            public string servertype;
            public string attachement;
            public string isHtml;
        }

        /// <summary>
        /// メールサーバ情報取得
        /// </summary>
        private Hashtable GetMailServer()
        {
            DataSet ds = new DataSet();
            if (db.db.GetMailServer(0, 0, "编号,名称,地址,端口,用户,密码,送信人地址,服务器类型,添付文件,HTML", "", "", ref ds))
            {
                Hashtable ht = new Hashtable();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MailPara mp = new MailPara();
                    mp.name = dr["名称"].ToString();
                    mp.address = dr["地址"].ToString();
                    //mp.port = dr["端口"].ToString();
                    mp.user = dr["用户"].ToString();
                    mp.password = dr["密码"].ToString();
                    mp.from = dr["送信人地址"].ToString();
                    mp.servertype = dr["服务器类型"].ToString().Trim();
                    mp.attachement = dr["添付文件"].ToString();
                    mp.isHtml = dr["HTML"].ToString();
                    ht[mp.servertype] = mp;
                }
                return ht;
            }
            return null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormMail_Load(object sender, EventArgs e)
        {
            lblConsultant.Text = data[1];
            txtDepartment.Text = data[14];
            txtManager.Text = data[2];
            txtMail.Text = data[12];
        }
        /// <summary>
        /// 添付ファイル選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text File (*.jpg)|*.jpg|All File (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (txtAttach.Text != "")
                {
                    txtAttach.Text += ";" + dlg.FileName;
                }
                else
                {
                    txtAttach.Text = dlg.FileName;
                }
            }

        }
        /// <summary>
        /// テンプレート開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadTemplate_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text File (*.txt)|*.txt|All File (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                importTemplate(dlg.FileName);
            }

        }
        /// <summary>
        /// メールテンプレート
        /// </summary>
        /// <param name="csvFile"></param>
        private void importTemplate(String fileName)
        {
            NdnPublicFunction func = new NdnPublicFunction();
            using (StreamReader reader =
                new StreamReader(fileName, Encoding.GetEncoding("UTF-8")))
            {
                String line = reader.ReadLine();
                txtTitle.Text = line;
                txtContent.Text = "";
                while (line != null)
                {
                    line = reader.ReadLine();
                    txtContent.Text += line+"\r\n";
                }
            }
        }

        /// <summary>
        /// テンプレート保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = dlg.FileName;
                using (StreamWriter writer =
                    new StreamWriter(fileName,false, Encoding.GetEncoding("UTF-8")))
                {
                    writer.WriteLine(txtTitle.Text);
                    writer.WriteLine(txtContent.Text);
                }
                MessageBox.Show("Save template complete!");
            }
        }
        /// <summary>
        /// メール送信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Hashtable ht = GetMailServer();
            MailPara mp = (MailPara)ht["OUTLOOK"];
            mp.subject = txtTitle.Text;
            mp.body = string.Format(txtContent.Text,lblConsultant.Text,txtDepartment.Text,txtManager.Text);
            mp.picfile = txtAttach.Text;
            mp.to = txtMail.Text;
            if (mp.isHtml != "Y")
            {
                mp.htmlbody = null;
            }
            if (mp.attachement != "Y")
            {
                mp.picfile = null;
            }
            if (NCMail.SendEmail(mp.subject, mp.body, mp.htmlbody, mp.picfile, mp.address, mp.user, mp.password, mp.from, mp.to, mp.servertype))
            {
                MessageBox.Show("Mail Send Complete!");
                Close();
            }
            else
            {
                MessageBox.Show("Mail Send Fail!");
            }
        }
    }
}
