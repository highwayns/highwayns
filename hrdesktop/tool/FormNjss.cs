using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using NC.HPS.Lib;

namespace highwayns
{
    public partial class FormNjss : Form
    {
        public FormNjss()
        {
            InitializeComponent();
        }
        /// <summary>
        /// get index.htm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchWeb_Click(object sender, EventArgs e)
        {
            string link = "https://www.njss.info/";
            Uri uri2 = new Uri(link);
            string filename = "index.htm";

            UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
            string path = @"C:\Temp\njss\" + uri.Host + "\\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (WebClient client = new WebClient())
            {
                client.DownloadFileAsync(uri2, path + filename);
            }
            MessageBox.Show("Search　Web　Over！");
        }
        /// <summary>
        /// get Url From index.htm
        /// </summary>
        /// <param name="fileName"></param>
        private Hashtable getUrl(string fileName)
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
                return getUrl(nodes);
            }
            return null;
        }
        /// <summary>
        /// get company infor after web search
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="fileName"></param>
        private Hashtable getUrl(HtmlAgilityPack.HtmlNodeCollection nodes)
        {
            Hashtable urls = new Hashtable();
            foreach (HtmlAgilityPack.HtmlNode node in nodes)
            {
                string url = node.GetAttributeValue("href", "");
                if (!url.StartsWith("http"))
                {
                    string link = "https://www.njss.info";
                    if (!url.StartsWith("/"))
                        url = link + "/" + url;
                    else
                        url = link + url;
                }
                if (url.IndexOf("searchgovernmentbid") > -1 || url.IndexOf("organization_introductions") > -1)
                {
                    urls[url] = node.InnerText.Replace(",","_");
                }
            }
            return urls;
        }
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetUrl_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if(dlg.ShowDialog()==DialogResult.OK)
            {
                string link = "https://www.njss.info/";
                Uri uri2 = new Uri(link);
                string filename = "index.htm";

                UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
                string path = @"C:\Temp\njss\" + uri.Host + "\\"+filename;
                Hashtable ht = getUrl(path);
                using (StreamWriter sw = new StreamWriter(dlg.FileName, false, Encoding.UTF8))
                {
                    foreach (string key in ht.Keys)
                    {
                        sw.WriteLine(Convert.ToString( ht[key])+","+key);
                    }
                }
                MessageBox.Show("Get Url Over!\r\n there are " + ht.Keys.Count.ToString() + " record!");

            }
        }
        /// <summary>
        /// get Home Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetHomePage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Hashtable ht = new Hashtable();
                using (StreamReader sr = new StreamReader(dlg.FileName, Encoding.UTF8))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        string[] data = line.Split(',');
                        ht[data[1]] = data[0];
                        line = sr.ReadLine();
                    }
                }
                foreach(string key in ht.Keys)
                {
                    Uri uri2 = new Uri(key);
                    string filename = ht[key].ToString() + ".htm";

                    UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
                    string path = @"C:\Temp\njss\" + uri.Host + "\\" + filename;

                    if (!File.Exists(path))
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFileAsync(uri2, path);
                            System.Threading.Thread.Sleep(1000);
                            Application.DoEvents();
                        }
                    }

                }
                MessageBox.Show("get Home Page Over!\r\n there are " + ht.Keys.Count.ToString() + " record!");
            }

        }
        /// <summary>
        /// Get Bid Infor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetBid_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Hashtable ht = new Hashtable();
                using (StreamReader sr = new StreamReader(dlg.FileName, Encoding.UTF8))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        string[] data = line.Split(',');
                        ht[data[6]] = data;
                        line = sr.ReadLine();
                    }
                }
                foreach (string key in ht.Keys)
                {
                    Uri uri2 = new Uri(key);
                    string[] data = (string[])ht[key];
                    string filename = data[0] + ".htm";
                    UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
                    string path = @"C:\Temp\njss\" + uri.Host + "\\" + Regex.Replace(uri.Path, "/", "\\"); ;
                    if (!path.EndsWith("\\"))
                        path += "\\";

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    path = path + filename;
                    if (!File.Exists(path))
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFileAsync(uri2, path);
                            System.Threading.Thread.Sleep(1000);
                            Application.DoEvents();
                        }
                    }

                }
                MessageBox.Show("get Bid Infor!\r\n there are " + ht.Keys.Count.ToString() + " record!");
            }
        }
        /// <summary>
        /// ダウンロードBidインフォ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string link = "https://www.njss.info/";
                Uri uri2 = new Uri(link);
                string filename = "index.htm";

                UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
                string path = @"C:\Temp\njss\" + uri.Host + "\\";
                string[] files = Directory.GetFiles(path);
                Hashtable urls = new Hashtable();
                foreach (string file in files)
                {
                    if(Path.GetFileName(file)!=filename)
                    {
                        getBidUrl(file, urls);
                    }
                }
                using (StreamWriter sw = new StreamWriter(dlg.FileName, false, Encoding.UTF8))
                {
                    foreach (string key in urls.Keys)
                    {
                        sw.WriteLine(Convert.ToString(urls[key]) + "," + key);
                    }
                }
                MessageBox.Show("Get Url Over!\r\n there are " + urls.Keys.Count.ToString() + " record!");

            }

        }
        /// <summary>
        /// get Bid Url 
        /// </summary>
        /// <param name="fileName"></param>
        private void getBidUrl(string fileName, Hashtable urls)
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
                getBidUrl(nodes, urls);
            }
        }
        /// <summary>
        /// get Bid Url
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="fileName"></param>
        private void getBidUrl(HtmlAgilityPack.HtmlNodeCollection nodes, Hashtable urls)
        {
            foreach (HtmlAgilityPack.HtmlNode node in nodes)
            {
                string url = node.GetAttributeValue("href", "");
                if (!url.StartsWith("http"))
                {
                    string link = "https://www.njss.info";
                    if (!url.StartsWith("/"))
                        url = link + "/" + url;
                    else
                        url = link + url;
                }
                if (url.IndexOf("organizations/proc") > -1 && node.InnerText.Trim()!="")
                {
                    HtmlAgilityPack.HtmlNodeCollection linodes = node.ParentNode.ParentNode.SelectNodes(".//li");
                    if (linodes != null && linodes.Count == 5)
                    {
                        urls[url] = node.InnerText.Replace(",", "_").Trim()
                            + "," + nodes[nodes[node] + 1].InnerText.Trim()
                            + "," + nodes[nodes[node] + 2].InnerText.Trim()
                            + "," + linodes[2].InnerText.Replace(",", "").Trim()
                            + "," + linodes[3].InnerText.Replace(",", "").Trim()
                            + "," + linodes[4].InnerText.Replace(",", "").Trim()
                            ;
                    }
                }
            }
        }

        private void btnDownloadBidDetail_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string link = "https://www.njss.info/";
                Uri uri2 = new Uri(link);

                UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
                string path = @"C:\Temp\njss\" + uri.Host + "\\organizations\\proc";
                string[] dirs = Directory.GetDirectories(path);
                Hashtable urls = new Hashtable();
                foreach (string dir in dirs)
                {
                    string[] files = Directory.GetFiles(dir);
                    if (files!=null && files.Length >0)
                    {
                        getBidDetailUrl(files[0], urls);
                    }
                }
                using (StreamWriter sw = new StreamWriter(dlg.FileName, false, Encoding.UTF8))
                {
                    foreach (string key in urls.Keys)
                    {
                        sw.WriteLine(Convert.ToString(urls[key]) + "," + key);
                    }
                }
                MessageBox.Show("Get BidDetail Url Over!\r\n there are " + urls.Keys.Count.ToString() + " record!");

            }
        }
        /// <summary>
        /// get Bid Url 
        /// </summary>
        /// <param name="fileName"></param>
        private void getBidDetailUrl(string fileName, Hashtable urls)
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
                getBidDetailUrl(nodes, urls, fileName);
            }
        }
        /// <summary>
        /// get Bid Detail Url
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="fileName"></param>
        private void getBidDetailUrl(HtmlAgilityPack.HtmlNodeCollection nodes, Hashtable urls,string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string address = "";
            HtmlAgilityPack.HtmlNodeCollection linodes = nodes[0].SelectNodes("//li");
            foreach (HtmlAgilityPack.HtmlNode addressNode in linodes)
            {
                if (addressNode.InnerText.IndexOf("〒") > -1)
                {
                    address = addressNode.InnerText.Replace(" ", "").Replace("\n","");
                    break;
                }
            }
            foreach (HtmlAgilityPack.HtmlNode node in nodes)
            {
                string url = node.GetAttributeValue("href", "");
                if (!url.StartsWith("http"))
                {
                    string link = "https://www.njss.info";
                    if (!url.StartsWith("/"))
                        url = link + "/" + url;
                    else
                        url = link + url;
                }
                if (url.IndexOf("offers/view") > -1 && node.InnerText.Trim() != "")
                {
                    linodes = node.ParentNode.ParentNode.SelectNodes(".//li");
                    if (linodes != null && linodes.Count >2)
                    {
                        urls[url] = name
                            + "," + node.InnerText.Replace(",", "_").Trim()
                            + "," + linodes[0].InnerText.Replace(" ", "").Replace("\n", "").Trim()
                            + "," + linodes[1].InnerText.Replace(" ", "").Replace("\n", "").Trim()
                            + "," + linodes[2].InnerText.Replace(" ", "").Replace("\n", "").Trim()
                            + "," + address 
                            ;
                    }
                    //Uri uri2 = new Uri(url);
                    //string filename = node.InnerText.Replace(",", "_").Trim() + ".htm";
                    //UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
                    //string path = @"C:\Temp\njss\" + uri.Host + "\\" + Regex.Replace(uri.Path, "/", "\\"); ;
                    //if (!path.EndsWith("\\"))
                    //    path += "\\";

                    //if (!Directory.Exists(path))
                    //    Directory.CreateDirectory(path);
                    //path = path + filename;
                    //if (!File.Exists(path))
                    //{
                    //    using (WebClient client = new WebClient())
                    //    {
                    //        client.DownloadFileAsync(uri2, path);
                    //        System.Threading.Thread.Sleep(1000);
                    //        Application.DoEvents();
                    //    }
                    //}
                }
            }
        }
        Hashtable bid = new Hashtable();
        /// <summary>
        /// Load Csv for bid project
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
                    bid.Clear();
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        string[] temp = line.Split(',');
                        string[] rows = new string[9];
                        rows[0] = dgvData.Rows.Count.ToString();
                        rows[1] = temp[0];//名称
                        rows[2] = temp[1];//分類
                        rows[3] = temp[2];//区域                    
                        rows[4] = temp[3].Replace("入札可能案件","");//入札可能案件
                        rows[5] = temp[4].Replace("案件登録数", "");//案件登録数
                        rows[6] = temp[5].Replace("入札結果数", "");//入札結果数
                        rows[7] = temp[6];//web
                        rows[8] = "";//other
                        dgvData.Rows.Add(rows);
                        bid[rows[1]] = rows;
                        line = sr.ReadLine();
                    }
                }
                MessageBox.Show("Load Csv Over!\r\n there are " + bid.Keys.Count.ToString() + " record!");
            }

        }
        /// <summary>
        /// Save to Csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSavetoCsv_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(dlg.FileName, false, Encoding.UTF8))
                {
                    // add table list
                    foreach (DataGridViewRow row in dgvData.Rows)
                    {
                        string companyName = row.Cells[1].Value.ToString();
                        string[] data = (string[])bid[companyName];
                        sw.WriteLine(string.Join(",", data,1,data.Length-1));
                    }
                }
                MessageBox.Show("Save Csv Over!\r\n there are " + bid.Keys.Count.ToString() + " record!");
            }
        }
        /// <summary>
        /// Show Detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvData_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                string name = dgvData.Rows[e.RowIndex].Cells[1].Value.ToString();
                (new FormNjssDetail(name)).ShowDialog();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "入札テンプレート.xlsx");
                NCExcel execel = new NCExcel();
                execel.OpenExcelFile(fileName);
                execel.SelectSheet(1);
                int idx = 3;
                // add table list
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    string no = row.Cells[0].Value.ToString();
                    string Name = row.Cells[1].Value.ToString();
                    string bunrui = row.Cells[2].Value.ToString();
                    string area = row.Cells[3].Value.ToString();
                    string num1 = row.Cells[4].Value.ToString();
                    string num2 = row.Cells[5].Value.ToString();
                    string num3 = row.Cells[6].Value.ToString();
                    string web = row.Cells[7].Value.ToString();
                    string comment = row.Cells[8].Value.ToString();

                    execel.setValue(1, idx, no);
                    execel.setValue(2, idx, Name);
                    execel.setValue(3, idx, bunrui);
                    execel.setValue(4, idx, area);
                    execel.setValue(5, idx, num1);
                    execel.setValue(6, idx, num2);
                    execel.setValue(7, idx, num3);
                    execel.setValue(8, idx, web);
                    execel.setValue(9, idx, comment);
                    idx++;
                }
                execel.SaveAs(dlg.FileName);
                MessageBox.Show("Save ExcelOver!\r\n there are " + dgvData.Rows.Count.ToString() + " record!");
            }
        }

    }
}
