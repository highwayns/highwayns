using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HPSConsultant
{
    public partial class FormConsultantEdit : Form
    {
        public string[] data=null;
        public FormConsultantEdit(string[] data)
        {
            this.data = data;

            InitializeComponent();
        }

        private void FormConsultantEdit_Load(object sender, EventArgs e)
        {
            //id,Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID
            string urls = "";
            if (data[11].IndexOf("***") > -1)
            {
                int idx_start = data[11].IndexOf("***") + 3;
                int idx_end = data[11].IndexOf("*", idx_start);
                if (idx_end > -1)
                {
                    urls = data[11].Substring(idx_start, idx_end - idx_start);
                }
                else
                {
                    urls = data[11].Substring(idx_start);
                }
                lblFlg.Text = "***";
            }
            else if (data[11].IndexOf("**")>-1)
            {
                int idx_start = data[11].IndexOf("**") + 2;
                int idx_end = data[11].IndexOf("*", idx_start);
                if (idx_end > -1)
                {
                    urls = data[11].Substring(idx_start, idx_end - idx_start);
                }
                else
                {
                    urls = data[11].Substring(idx_start);
                }
                lblFlg.Text = "**";

            }
            else if (data[11].IndexOf("*") > -1)
            {
                urls = data[11].Split('*')[1];
                lblFlg.Text = "*";
            }
            lblPath.Text = urls;
            try
            {
                if(data[13]!="")

                    webBrowser1.Navigate(new Uri(data[13]));
            }
            catch
            {
                //
            }
            //id,Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID
            lblConsultant.Text = data[1];
            txtDepartment.Text = data[14];
            txtManager.Text = data[2];
            txtAddress.Text = data[4];
            txtTel.Text = data[5];
            txtMail.Text = data[12];
            txtWeb.Text = data[13];
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //id,Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID
            data[1] = lblConsultant.Text;
            data[14] = txtDepartment.Text;
            data[2] = txtManager.Text;
            data[4] = txtAddress.Text;
            data[5] = txtTel.Text;
            data[12] = txtMail.Text;
            data[13] = txtWeb.Text;
            this.DialogResult = DialogResult.OK;
        }


    }
}
