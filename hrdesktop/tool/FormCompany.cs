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
using NC.HPS.Lib;
using System.Collections;

namespace highwayns
{
    public partial class FormCompany : Form
    {
        public FormCompany()
        {
            InitializeComponent();
        }
        /// <summary>
        /// use hashtable to exclude exists company
        /// </summary>
        Hashtable ht = new Hashtable(); 
        /// <summary>
        /// load initial data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadTxt_Click(object sender, EventArgs e)
        {
            string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "派遣会社一覧.csv");
            readData(fileName);
            fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "生産技能会社一覧.txt");
            readData2(fileName);
            fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "CompanyList_20130715.csv");
            readData3(fileName);
            fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "CompanyList_20151220.csv");
            readData3(fileName);

            MessageBox.Show("Load Over!\r\n there are " + ht.Keys.Count.ToString() + " record!");
        }
        /// <summary>
        /// save data to excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveExcel_Click(object sender, EventArgs e)
        {
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
                    string no  = row.Cells[0].Value.ToString();
                    string companyName = row.Cells[1].Value.ToString();
                    string departName = row.Cells[2].Value.ToString();
                    string manager = row.Cells[3].Value.ToString();
                    string address = row.Cells[4].Value.ToString();
                    string tel = row.Cells[5].Value.ToString();
                    string mail = row.Cells[6].Value.ToString();
                    string web = row.Cells[7].Value.ToString();
                    string comment = row.Cells[8].Value.ToString();

                    execel.setValue(1, idx, no);
                    execel.setValue(2, idx, companyName);
                    execel.setValue(3, idx, departName);
                    execel.setValue(4, idx, manager);
                    execel.setValue(5, idx, mail);
                    execel.setValue(6, idx, address);
                    execel.setValue(7, idx, tel);
                    execel.setValue(8, idx, web);
                    execel.setValue(9, idx, comment);
                    idx++;
                }
                execel.SaveAs(dlg.FileName);
                MessageBox.Show("Save Over!\r\n there are "+ht.Keys.Count.ToString()+" record!");
            }
        }
        /// <summary>
        /// read data have dispatch no
        /// </summary>
        /// <param name="filename"></param>
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
                            string[] rows = new string[9];
                            rows[0] = dgvData.Rows.Count.ToString();
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
                            rows[7] = "";//web
                            rows[8] = temp[0] + temp[1];//会社番号
                            if (ht[rows[1]] == null)
                            {
                                ht[rows[1]] = rows;
                                dgvData.Rows.Add(rows);
                            }
                        }
                    }
                    line = sr.ReadLine();
                }
            }
        }
        /// <summary>
        /// read data from text file which include produce company
        /// </summary>
        /// <param name="filename"></param>
        private void readData2(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split(' '); 
                    string[] rows = new string[9];
                    rows[0] = dgvData.Rows.Count.ToString();
                    rows[1] = temp[0];//会社名
                    rows[2] = temp[1];//部門または職位
                    rows[3] = line.Replace(temp[0],"").Replace(temp[1],"").Trim();//管理者名前                    
                    rows[4] = sr.ReadLine();//アドレス
                    rows[5] = sr.ReadLine();//電話・FAX
                    rows[6] = "";//メール
                    rows[7] = "";//web
                    rows[8] = "";//other
                    if (ht[rows[1]] == null)
                    {
                        ht[rows[1]] = rows;
                        dgvData.Rows.Add(rows);
                    }
                    line = sr.ReadLine();
                }
            }
        }
        /// <summary>
        /// read data which is on open markert
        /// </summary>
        /// <param name="filename"></param>
        private void readData3(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                line = sr.ReadLine();
                while (line != null)
                {
                    string[] temp = line.Split(',');
                    string[] rows = new string[9];
                    for (int i = 3; i < temp.Length; i++)
                    {
                        if (temp[i].StartsWith("http"))
                        {
                            rows[0] = dgvData.Rows.Count.ToString();
                            rows[1] = temp[i-1];//会社名
                            rows[2] = "";//部門または職位
                            rows[3] = "";//管理者名前                    
                            rows[4] = "";//アドレス
                            rows[5] = "";//電話・FAX
                            rows[6] = "";//メール
                            rows[7] = temp[i];//web
                            rows[8] = "";//other
                            if (ht[rows[1]] == null)
                            {
                                ht[rows[1]] = rows;
                                dgvData.Rows.Add(rows);
                            }
                            break;
                        }
                    }
                    line = sr.ReadLine();
                }
            }
        }

        /// <summary>
        /// search web
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchWeb_Click(object sender, EventArgs e)
        {
            Random rdm = new Random();
            int count = 0;
            for (int i = 0; i < dgvData.Rows.Count; i++)
            {
                Application.DoEvents();
                if (i % 5 == 0)
                {
                    System.Threading.Thread.Sleep(10000);
                }
                string tmp = rdm.Next(1,999).ToString("000")+"_"+rdm.Next(1,9).ToString("0");
                string companyName  = dgvData.Rows[i].Cells[1].Value.ToString();
                string[] infor = (string[])ht[companyName];
                if (infor[7] == "")
                {
                    companyName = System.Web.HttpUtility.UrlEncode(companyName);
                    string link = string.Format("https://www.bing.com/search?q={0}&form=PRJPJA&httpsmsn=1&refig=681a111219984b49bdcfa299a3dde555&pq=%E7%BE%8E%E7%91%9B%E9%80%9A%E9%81%8B%E6%A0%AA%E5%BC%8F%E4%BC%9A%E7%A4%BE&sc=1-8&sp=-1&qs=n&sk=", companyName, tmp);
                    Uri uri2 = new Uri(link);
                    string filename = dgvData.Rows[i].Cells[1].Value.ToString() + ".htm";

                    UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
                    string path = i.ToString();
                    path = @"C:\Temp\bing\" + uri.Host + "\\" + Regex.Replace(path, "/", "\\");

                    if (!path.EndsWith("\\"))
                        path += "\\";

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFileAsync(uri2, path + filename);
                        count++;
                        Application.DoEvents();
                    }
                }
            }
            MessageBox.Show("Sereach Over! there are "+count.ToString()+" record!");

        }
        /// <summary>
        /// load profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadProfile_Click(object sender, EventArgs e)
        {
            string dir = @"C:\Temp\Spider";
            getLine(dir);
            MessageBox.Show("Load html Over!\r\n there are " + ht.Keys.Count.ToString() + " record!");
        }
        /// <summary>
        /// get line after spider
        /// </summary>
        /// <param name="dir"></param>
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
        /// <summary>
        /// get company infor after spider
        /// </summary>
        /// <param name="fileName"></param>
        private void getCompanyInfor(string fileName)
        {
            if (fileName.IndexOf("株") > -1)
            {
                string[] rows = new string[9];
                rows[0] = dgvData.Rows.Count.ToString();
                rows[1] = Path.GetFileNameWithoutExtension(fileName);//会社名
                rows[2] = "";//部門または職位
                rows[3] = "";//管理者名前                    
                rows[4] = "";//アドレス
                rows[5] = "";//電話・FAX
                rows[6] = "";//メール
                rows[7] = "";//web
                rows[8] = "";//other
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
                if (ht[rows[1]] == null)
                {
                    ht[rows[1]] = rows;
                    dgvData.Rows.Add(rows);
                }
            }
        }
        /// <summary>
        /// get url after websearch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetUrl_Click(object sender, EventArgs e)
        {
            getLine2(@"C:\temp\bing\www.bing.com");
        }
        /// <summary>
        /// get line after web search
        /// </summary>
        /// <param name="dir"></param>
        private void getLine2(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*.htm*");
            foreach (string file in files)
            {
                getCompanyInfor2(file);
            }
            string[] subdirs = Directory.GetDirectories(dir);
            foreach (string subdir in subdirs)
            {
                getLine2(subdir);
            }
            //MessageBox.Show("Over!");
        }
        /// <summary>
        /// get company infor after web search
        /// </summary>
        /// <param name="fileName"></param>
        private void getCompanyInfor2(string fileName)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionAutoCloseOnEnd = false;  //最後に自動で閉じる（？）
            doc.OptionCheckSyntax = false;     //文法チェック。
            doc.OptionFixNestedTags = true;    //閉じタグが欠如している場合の処理
            FileStream fs = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            doc.Load(sr);
            fs.Close();
            sr.Close();
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a");
            if (nodes != null)
            {
                getCompanyInfor2(nodes, fileName);
            }

        }
        /// <summary>
        /// get company infor after web search
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="fileName"></param>
        private void getCompanyInfor2(HtmlAgilityPack.HtmlNodeCollection nodes, string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            List<string> urls = new List<string>();
            foreach (HtmlAgilityPack.HtmlNode node in nodes)
            {
                if (node.InnerHtml.IndexOf(name) > -1)
                {
                    string url = node.GetAttributeValue("href", "");
                    if(url.StartsWith("http"))
                    {
                        if (url.Split('/').Length < 5 && url.IndexOf("?") < 0)
                        {
                            urls.Add(url);
                        }
                    }
                }
            }
            if (urls.Count > 0)
            {
                string url = "";
                foreach (string url_ in urls)
                {
                    if (url_.IndexOf("profile") > -1)
                    {
                        url = url_;
                        break;
                    }
                }
                if (url == "")
                {
                    foreach (string url_ in urls)
                    {
                        if (url_.IndexOf("company") > -1)
                        {
                            url = url_;
                            break;
                        }
                    }
                }
                if (url == "")
                {
                    url = urls[0];
                }
                string[] data = (string[])ht[name];
                if (data != null)
                {
                    data[7] = url;
                    int idx = int.Parse(data[0]);
                    if (idx < dgvData.Rows.Count)
                    {
                        dgvData.Rows[idx].Cells[7].Value = url;
                    }
                    Application.DoEvents();
                }
            }

        }
        /// <summary>
        /// CSV ファイルに保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSavetoCsv_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(dlg.FileName,true, Encoding.UTF8))
                {
                    // add table list
                    foreach (DataGridViewRow row in dgvData.Rows)
                    {
                        string companyName = row.Cells[1].Value.ToString();
                        string[] data = (string[])ht[companyName];
                        sw.WriteLine(string.Join(",", data));
                    }
                }
                MessageBox.Show("Save Over!\r\n there are " + ht.Keys.Count.ToString() + " record!");
            }

        }
        /// <summary>
        /// CSVファイル読み取る
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadCsv_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(dlg.FileName, Encoding.UTF8))
                {
                    dgvData.Rows.Clear();
                    ht.Clear();
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        string[] data = line.Split(',');
                        dgvData.Rows.Add(data);
                        ht[data[1]] = data;
                        line = sr.ReadLine();
                    }
                }
                MessageBox.Show("Save Over!\r\n there are " + ht.Keys.Count.ToString() + " record!");
            }

        }
        /// <summary>
        /// Get Home Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetHomepage_Click(object sender, EventArgs e)
        {
            foreach (string key in ht.Keys)
            {
                Application.DoEvents();
                string[] data = (string[])ht[key];
                if(data[7]!="" && (data[3]=="" || data[5]==""))
                {
                    Uri uri2 = new Uri(data[7]);
                    string filename = data[1];

                    UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
                    string path = data[0];
                    path = @"C:\Temp\Spider\" + uri.Host + "\\" + Regex.Replace(path, "/", "\\");

                    if (!path.EndsWith("\\"))
                        path += "\\";

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFileAsync(uri2, path + filename);
                        Application.DoEvents();
                    }
                }
            }
            MessageBox.Show("Get Profile Over!");

        }
        /// <summary>
        /// get Profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetProfile_Click(object sender, EventArgs e)
        {

        }

    }
}
