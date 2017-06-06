using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HtmlAgilityPack;
using Weaver;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;

namespace highwayns
{
    public partial class FormWeb : Form
    {
        public FormWeb()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            getLine(textBox1.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void getLine(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*.htm*");
            foreach(string file in files)
            {
                getCompanyInfor(file);
            }
            string[] subdirs = Directory.GetDirectories(dir);
            foreach(string subdir in subdirs)
            {
                getLine(subdir);
            }
            //MessageBox.Show("Over!");
        }

        private void getCompanyInfor(string fileName)
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
                getCompanyInfor(nodes);
            }
            
        }
        private void getCompanyInfor(HtmlAgilityPack.HtmlNodeCollection nodes)
        {
            foreach (HtmlAgilityPack.HtmlNode node in nodes)               
            {
                if (node.InnerHtml.IndexOf("株") > -1
                    || node.InnerHtml.IndexOf("@") > -1
                    || node.InnerHtml.IndexOf("資本金") > -1
                    || node.InnerHtml.IndexOf("電話") > -1
                    || node.InnerHtml.IndexOf("有限") > -1
                    )
                {
                    string url = node.GetAttributeValue("href", "");
                    listBox1.Items.Add(url);
                    string temp = node.InnerHtml;
                    if (temp.IndexOf("の") > -1)
                        temp = temp.Substring(0, temp.IndexOf("の"));
                    if (temp.IndexOf("&") > -1)
                        temp = temp.Substring(0, temp.IndexOf("&"));
                    listBox1.Items.Add(temp);
                    Application.DoEvents();
                }
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i += 2)
            {
                Application.DoEvents();
                Uri uri2 = new Uri(listBox1.Items[i].ToString());
                string filename = Path.GetFileName(uri2.LocalPath);

                UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
                string path = i.ToString();
                path = @"C:\Temp\Spider\" + uri.Host + "\\" + Regex.Replace(path, "/", "\\");

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
