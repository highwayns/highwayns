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
                    if (url.StartsWith("/"))
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
                    if (url.StartsWith("/"))
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
                            + "," + linodes[2].InnerText.Trim()
                            + "," + linodes[3].InnerText.Trim()
                            + "," + linodes[4].InnerText.Trim()
                            ;
                    }
                }
            }
        }

    }
}
