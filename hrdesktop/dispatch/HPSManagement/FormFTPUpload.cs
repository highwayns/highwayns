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
    public partial class FormFTPUpload : Form
    {
        private CmWinServiceAPI db;
        public FormFTPUpload(CmWinServiceAPI db)
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
            DataSet ds_ftp = new DataSet();
            if (db.GetFTPUploadVW(0, 0, "*", "", "", ref ds_ftp))
            {
                dataGridView3.DataSource = ds_ftp.Tables[0];
                lblFileUploadN.Text = Convert.ToString(ds_ftp.Tables[0].Rows.Count);
            }
        }

        /// <summary>
        /// FTP上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView3.SelectedRows.Count; i++)
            {
                string id = dataGridView3.SelectedRows[i].Cells[0].Value.ToString();
                DataSet ds = new DataSet();
                String strWhere = "上传编号=" + id;
                if (db.GetFTPUploadVW(0, 0, "*", strWhere, "", ref ds))
                {
                    NCFTP ftp = new NCFTP();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        String filename = dr["上传名称"].ToString();
                        String url = dr["FTP文件"].ToString();
                        string uploadid = dr["上传编号"].ToString();
                        if (ftp.uploadFile(url, filename, dr["用户"].ToString(), dr["密码"].ToString()))
                        {
                            setUpload(uploadid, "上传完");
                        }
                    }
                }
                Application.DoEvents();
            }
            init();
        }
        /**
         *设置上传状态
        **/
        private void setUpload(String uploadid, String Status)
        {
            int id = 0;
            String wheresql = "上传编号=" + uploadid;
            String valuesql = "上传状态='" + Status + "'";
            db.SetFTPUpload(0, 1, "", wheresql, valuesql, out id);
        }

    }
}
