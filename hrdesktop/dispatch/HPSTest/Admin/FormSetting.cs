using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HPSTest.Admin
{
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company
        {
            get { return txtCompanyName.Text; }
            set { txtCompanyName.Text = value; }
        }
        /// <summary>
        /// 网站
        /// </summary>
        public string Home
        {
            get { return txtUrl.Text; }
            set { txtUrl.Text = value; }
        }
        /// <summary>
        /// 网站
        /// </summary>
        public string Home2
        {
            get { return txtUrl2.Text; }
            set { txtUrl2.Text = value; }
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
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK; 
        }
        /// <summary>
        /// 焦点设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSetting_Activated(object sender, EventArgs e)
        {
            txtCompanyName.Focus();
        }
        /// <summary>
        /// 选择图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtUrl.Text = dlg.FileName;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtUrl2.Text = dlg.FileName;
            }
        }

    }
}
