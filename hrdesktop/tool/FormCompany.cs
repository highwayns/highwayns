using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
//using NC.HPS.Lib;

namespace highwayns
{
    public partial class FormCompany : Form
    {
        public FormCompany()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "派遣会社一覧.csv");
            readData(fileName);
            fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "生産技能会社一覧.txt");
            readData2(fileName);
            fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "CompanyList_20130715.csv");
            readData3(fileName);
            fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "CompanyList_20151220.csv");
            readData3(fileName);

            MessageBox.Show("Load Over!");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {/*
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "お客様テンプレート.xlsx");
                NCExcel execel = new NCExcel();
                execel.OpenExcelFile(fileName);
                execel.SelectSheet(1);
                int idx = 3;
                // add table list
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    string comment  = row.Cells[0].Value.ToString();
                    string companyName = row.Cells[1].Value.ToString();
                    string departName = row.Cells[2].Value.ToString();
                    string manager = row.Cells[3].Value.ToString();
                    string address = row.Cells[4].Value.ToString();
                    string tel = row.Cells[5].Value.ToString();
                    string mail = row.Cells[6].Value.ToString();

                    execel.setValue(2, idx, companyName);
                    execel.setValue(3, idx, departName);
                    execel.setValue(4, idx, manager);
                    execel.setValue(5, idx, mail);
                    execel.setValue(6, idx, address);
                    execel.setValue(7, idx, tel);
                    execel.setValue(8, idx, comment);
                    idx++;
                }
                execel.SaveAs(dlg.FileName);
                MessageBox.Show("Save Over!");
            }*/
        }

        private void readData(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.Length > 0 && line[0].ToString() == "派")
                    {
                        string[] temp = line.Split(' ');
                        if (temp.Length >= 4)
                        {
                            string[] rows = new string[7];
                            rows[0] = temp[0] + temp[1];//会社番号
                            rows[1] = temp[2];//会社名称
                            if (temp.Length == 4)
                            {
                                rows[2] = "";//部門または職位
                                rows[4] = temp[3];//アドレス
                            }
                            else
                            {
                                rows[2] = temp[3];//部門または職位
                                rows[4] = temp[4];//アドレス

                            }
                            rows[3] = "";//管理者名前
                            rows[5] = "";//電話・FAX
                            rows[6] = "";//メール
                            dgvData.Rows.Add(rows);
                        }
                    }
                    line = sr.ReadLine();
                }
            }
        }

        private void readData2(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split(' '); 
                    string[] rows = new string[7];
                    rows[0] = "";//会社番号
                    rows[1] = temp[0];//会社名
                    rows[2] = temp[1];//部門または職位
                    rows[3] = line.Replace(temp[0],"").Replace(temp[1],"").Trim();//管理者名前                    
                    rows[4] = sr.ReadLine();//アドレス
                    rows[5] = sr.ReadLine();//電話・FAX
                    rows[6] = "";//メール
                    dgvData.Rows.Add(rows);
                    line = sr.ReadLine();
                }
            }
        }

        private void readData3(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                line = sr.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split(',');
                    string[] rows = new string[7];
                    for (int i = 3; i < temp.Length; i++)
                    {
                        if (temp[i].StartsWith("http"))
                        {
                            rows[0] = temp[i];//会社番号
                            rows[1] = temp[i-1];//会社名
                            rows[2] = "";//部門または職位
                            rows[3] = "";//管理者名前                    
                            rows[4] = "";//アドレス
                            rows[5] = "";//電話・FAX
                            rows[6] = "";//メール
                            dgvData.Rows.Add(rows);
                            break;
                        }
                    }
                    line = sr.ReadLine();
                }
            }
        }

        private void btnLoadHtml_Click(object sender, EventArgs e)
        {
            string dir = @"C:\Temp\Spider";
            getLine(dir);
        }
        private void getLine(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*.htm*");
            foreach (string file in files)
            {
                getCompanyInfor(file);
            }
            string[] subdirs = Directory.GetDirectories(dir);
            foreach (string subdir in subdirs)
            {
                getLine(subdir);
            }
        }

        private void getCompanyInfor(string fileName)
        {
            if (fileName.IndexOf("株") > -1)
            {
                string[] rows = new string[7];
                rows[0] = "";//会社番号
                rows[1] = Path.GetFileNameWithoutExtension(fileName);//会社名
                rows[2] = "";//部門または職位
                rows[3] = "";//管理者名前                    
                rows[4] = "";//アドレス
                rows[5] = "";//電話・FAX
                rows[6] = "";//メール
                using (StreamReader sr = new StreamReader(fileName, Encoding.UTF8))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        if (line.IndexOf(">電話番号<") > -1)
                        {
                            line = sr.ReadLine();
                            rows[5] = line.Replace("<td>", "").Replace("</td></tr>", "");
                        }
                        if (line.IndexOf(">郵便番号<") > -1)
                        {
                            line = sr.ReadLine();
                            rows[4] = line.Replace("<td>", "").Replace("</td></tr>", "");
                        }
                        if (line.IndexOf(">住所<") > -1)
                        {
                            line = sr.ReadLine();
                            rows[4] += line.Replace("<td>", "").Replace("</td></tr>", "");
                        }
                        line = sr.ReadLine();
                    }
                }
                dgvData.Rows.Add(rows);
            }
        }

        private void btnGetHP_Click(object sender, EventArgs e)
        {
            Random rdm = new Random();
            
            for (int i = 0; i < dgvData.Rows.Count; i++)
            {
                Application.DoEvents();
                if (i % 5 == 0)
                {
                    System.Threading.Thread.Sleep(10000);
                }
                string tmp = rdm.Next(1,999).ToString("000")+"_"+rdm.Next(1,9).ToString("0");
                string companyName  = dgvData.Rows[i].Cells[1].Value.ToString();
                companyName = System.Web.HttpUtility.UrlEncode(companyName);
                string link = string.Format("https://search.yahoo.co.jp/search?p={0}&ei=UTF-8", companyName,tmp);
                Uri uri2 = new Uri(link);
                string filename = dgvData.Rows[i].Cells[1].Value.ToString()+".htm";

                UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
                string path = i.ToString();
                path = @"C:\Temp\yahoo\" + uri.Host + "\\" + Regex.Replace(path, "/", "\\");

                if (!path.EndsWith("\\"))
                    path += "\\";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                using (WebClient client = new WebClient())
                {
                    client.DownloadFileAsync(uri2, path + filename);
                    //Log.DownloadedFile(uri2.AbsoluteUri);
                    Application.DoEvents();
                }
            }
            MessageBox.Show("Get Over!");

        }

    }
}
