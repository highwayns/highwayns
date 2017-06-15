using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace highwayns
{
    public partial class FormCompanyEdit : Form
    {
        public string[] data=null;
        public FormCompanyEdit(string[] data)
        {
            this.data = data;

            InitializeComponent();
        }

        private void FormCompanyEdit_Load(object sender, EventArgs e)
        {

            string urls = "";
            if (data[8].IndexOf("***") > -1)
            {
                int idx_start = data[8].IndexOf("***") + 3;
                int idx_end = data[8].IndexOf("*", idx_start);
                if (idx_end > -1)
                {
                    urls = data[8].Substring(idx_start, idx_end - idx_start);
                }
                else
                {
                    urls = data[8].Substring(idx_start);
                }
                lblFlg.Text = "***";
            }
            else if (data[8].IndexOf("**")>-1)
            {
                int idx_start = data[8].IndexOf("**") + 2;
                int idx_end = data[8].IndexOf("*", idx_start);
                if (idx_end > -1)
                {
                    urls = data[8].Substring(idx_start, idx_end - idx_start);
                }
                else
                {
                    urls = data[8].Substring(idx_start);
                }
                lblFlg.Text = "**";

            }
            else if (data[8].IndexOf("*") > -1)
            {
                urls = data[8].Split('*')[1];
                lblFlg.Text = "*";
            }
            lblPath.Text = urls;
            try
            {
                webBrowser1.Navigate(new Uri(data[7]));
            }
            catch
            {
                //
            }
            lblCompany.Text = data[1];
            txtDepartment.Text = data[2];
            txtManager.Text = data[3];
            txtAddress.Text = data[4];
            txtTel.Text = data[5];
            txtMail.Text = data[6];
            txtWeb.Text = data[7];
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            data[1] = lblCompany.Text;
            data[1] = txtDepartment.Text;
            data[1] = txtManager.Text;
            data[1] = txtAddress.Text;
            data[1] = txtTel.Text;
            data[1] = txtMail.Text;
            data[1] = txtWeb.Text;
            this.DialogResult = DialogResult.OK;
        }


    }
}
