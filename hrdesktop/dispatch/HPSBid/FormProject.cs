using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Diagnostics;
using NC.HPS.Lib;

namespace HPSBid
{
    public partial class FormProject : Form
    {
        private string CompanyName="";
        /// <summary>
        /// 数据库
        /// </summary>
        private DB db;

        public FormProject(DB db,string CompanyName)
        {
            this.db = db;
            this.CompanyName = CompanyName;
            InitializeComponent();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        private void init(string kind, string format)
        {
            DataSet ds = new DataSet();
            string wheresql = "1=1";
            if (kind != "") wheresql += " and kind='" + kind + "'";
            if (format != "") wheresql += " and format='" + format + "'";
            wheresql += " and jCNAME='" + CompanyName + "'";
            if (db.GetProject(0, 0, "*", wheresql, "", ref ds))
            {
                dgvData.DataSource = ds.Tables[0];
                lblRecordNum.Text = "(" + ds.Tables[0].Rows.Count.ToString() + ")";
            }
        }
        /// <summary>
        /// 初期化行业
        /// </summary>
        private void initKind()
        {
            DataSet ds = new DataSet();
            if (db.GetProject(0, 0, "distinct kind", "", "", ref ds))
            {
                cmbKinds.DataSource = ds.Tables[0];
                cmbKinds.DisplayMember = "kind";
            }
            cmbKinds.Text = "";
        }
        /// <summary>
        /// 初期化分类
        /// </summary>
        private void initFormat()
        {
            DataSet ds2 = new DataSet();
            if (db.GetProject(0, 0, "distinct format", "", "", ref ds2))
            {
                cmbFormats.DataSource = ds2.Tables[0];
                cmbFormats.DisplayMember = "format";
            }
            cmbFormats.Text = "";
        }

        Hashtable bid = new Hashtable();
        /// <summary>
        /// プロジェクト导入
        /// </summary>
        /// <param name="csvFile"></param>
        private void importProjectFile(String fileName)
        {
            NdnPublicFunction func = new NdnPublicFunction();
            using (StreamReader reader =
                new StreamReader(fileName, Encoding.GetEncoding("UTF-8")))
            {
                String line = reader.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split(',');
                    string name = temp[0];//名称
                    string projectName = temp[1];//プロジェクト
                    string area = temp[2].Replace("都道府県", "");//区域                    
                    string bunrui = temp[3].Replace("入札形式", "");//入札形式
                    string pubdate = temp[4].Replace("公示日", "");//公示日
                    string adress = temp[5].Substring(2);//住所
                    string url = temp[6];//web

                    int id = 0;
                    String valueList = "'"
                        + projectName
                        + "','"
                        + ""//管理者
                        + "','"
                        + ""//postcode
                        + "','"
                        + adress//address
                        + "','','','"//tel,fax
                        + bunrui + "','"//kind
                        + area//format
                        + "','"
                        + ""//scale
                        + "','2017/06/15','"//CYMD
                        + ""//other
                        + "','"
                        + "" //mail
                        + "','"
                        + url//web
                        + "','"
                        + name//jCName 
                        + "','"
                        + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                        + "','Y','1','" + bunrui + "','" + pubdate + "',''";
                    db.SetProject(0, 0, "Cname,name,postcode,[address],tel,fax,kind,[format],scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID,入札形式,公示日,入札内容",
                                            "", valueList, out id);
                    line = reader.ReadLine();
                }
            }

        }


        /// <summary>
        /// load csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormProject_Load(object sender, EventArgs e)
        {
            //btnLoadCsv_Click(null, null);
        }
        /// <summary>
        /// DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvData_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                string url = dgvData.Rows[e.RowIndex].Cells[13].Value.ToString();
                ExecuteCommand(@"C:\Program Files\Internet Explorer\iexplore.exe", url, "", false);
            }

        }
        /// <summary>
        /// 应用程序启动
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="localworkpath"></param>
        /// <returns></returns>
        private static int ExecuteCommand(string cmd, string para, string localworkpath, bool userShell)
        {
            int exitCode = -1;

            try
            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = userShell;
                process.StartInfo.FileName = cmd;
                process.StartInfo.WorkingDirectory = localworkpath;
                process.StartInfo.Arguments = para;
                process.Start();
                //process.WaitForExit();
                exitCode = process.ExitCode;
                process.Close();
            }
            catch (Exception er)
            {
                //MessageBox.Show("Exception=" + er.ToString());
                exitCode = -1;
            }
            return exitCode;
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                importProjectFile(dlg.FileName);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            init(cmbKinds.Text, cmbFormats.Text);
        }
    }
}
