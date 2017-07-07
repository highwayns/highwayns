using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Collections;

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
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormConsultantEdit_Load(object sender, EventArgs e)
        {
            //id,Cname,name,postcode,address,tel,fax,kind,format,scale,CYMD,other,mail,web,jCNAME,createtime,subscripted,UserID

            lblPath.Text = data[13];
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
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 数据取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (txtWeb.Text.Trim() == "")
            {
                MessageBox.Show("Webを取得してください！");
                return;
            }
            string companyName = lblConsultant.Text;
            string link = txtWeb.Text;
            Uri uri2 = new Uri(link);
            string filename = companyName + ".htm";

            UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
            string path = @"C:\Temp\Spider\" + uri.Host + "\\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(uri2, path + filename);
                Application.DoEvents();
                getCompanyInfor3(path + filename);
            }
        }
        /// <summary>
        /// get company infor after web search
        /// </summary>
        /// <param name="fileName"></param>
        private void getCompanyInfor3(string fileName)
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
                getCompanyInfor3(nodes, fileName);
            }

        }
        /// <summary>
        /// get company infor after web search
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="fileName"></param>
        private void getCompanyInfor3(HtmlAgilityPack.HtmlNodeCollection nodes, string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            Uri uri1 = new Uri(txtWeb.Text);
            Hashtable urls = new Hashtable();
            int count = 0;
            foreach (HtmlAgilityPack.HtmlNode node in nodes)
            {
                if (node.InnerHtml.IndexOf("企業") > -1 ||
                    node.InnerHtml.IndexOf("会社") > -1 ||
                    node.InnerHtml.IndexOf("概要") > -1 ||
                    node.InnerHtml.IndexOf("案内") > -1 ||
                    node.InnerHtml.IndexOf("COMPANY") > -1 ||
                    node.InnerHtml.IndexOf("ABOUTUS") > -1)
                {
                    string url = System.Web.HttpUtility.HtmlDecode(node.GetAttributeValue("href", ""));
                    if (url.StartsWith("http"))
                    {
                        if (url.Split('/').Length < 6 && url.IndexOf("?") < 0)
                        {
                            Uri uri2 = new Uri(url);
                            if (url != txtWeb.Text && uri1.Host == uri2.Host && urls[url] == null)
                            {
                                urls[url] = url;
                                count++;
                                if (count == 20)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (url.StartsWith("/"))
                            url = "http://" + uri1.Host + url;
                        else
                            url = "http://" + uri1.Host + "/" + url;
                        if (url.Split('/').Length < 6 && url.IndexOf("?") < 0)
                        {
                            Uri uri2 = new Uri(url);
                            if (url != txtWeb.Text && urls[url] == null)
                            {
                                urls[url] = url;
                                count++;
                                if (count == 20)
                                {
                                    break;
                                }
                            }
                        }

                    }

                }
            }
            if (count == 0)
            {
                foreach (HtmlAgilityPack.HtmlNode node in nodes)
                {
                    string url = System.Web.HttpUtility.HtmlDecode(node.GetAttributeValue("href", ""));
                    if (url.IndexOf("corporate") > -1 ||
                        url.IndexOf("about") > -1 ||
                        url.IndexOf("infor") > -1 ||
                        url.IndexOf("profile") > -1 ||
                        url.IndexOf("company") > -1)
                    {

                        if (url.StartsWith("http"))
                        {
                            if (url.Split('/').Length < 6 && url.IndexOf("?") < 0)
                            {
                                Uri uri2 = new Uri(url);
                                if (url != txtWeb.Text && uri1.Host == uri2.Host && urls[url] == null)
                                {
                                    urls[url] = url;
                                    count++;
                                    if (count == 20)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (url.StartsWith("/"))
                                url = "http://" + uri1.Host + url;
                            else
                                url = "http://" + uri1.Host + "/" + url;
                            if (url.Split('/').Length < 6 && url.IndexOf("?") < 0)
                            {
                                Uri uri2 = new Uri(url);
                                if (url != txtWeb.Text && urls[url] == null)
                                {
                                    urls[url] = url;
                                    count++;
                                    if (count == 20)
                                    {
                                        break;
                                    }
                                }
                            }

                        }
                    }

                }
            }
            if (urls.Keys.Count > 0)
            {
                string[] urldata = new string[urls.Keys.Count];
                urls.Keys.CopyTo(urldata, 0);
                for (int i = 0; i < urldata.Length; i++)
                {
                    Uri uri3 = new Uri(urldata[i]);
                    UriBuilder uri = new UriBuilder(uri3.AbsoluteUri);
                    string path = fileName.Split('\\')[4];
                    path = @"C:\Temp\Spider\" + uri.Host + "\\";
                    string filename = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + "_" + i.ToString() + ".htm");
                    if (!path.EndsWith("\\"))
                        path += "\\";

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    if (!File.Exists(filename))
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(uri3, filename);
                            System.Threading.Thread.Sleep(1000);
                            Application.DoEvents();
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 网址取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetWeb_Click(object sender, EventArgs e)
        {
            string companyName = lblConsultant.Text;
            string companyNameE = System.Web.HttpUtility.UrlEncode(companyName);
            string link = string.Format("https://www.bing.com/search?q={0}&form=PRJPJA&httpsmsn=1&refig=681a111219984b49bdcfa299a3dde555&pq=%E7%BE%8E%E7%91%9B%E9%80%9A%E9%81%8B%E6%A0%AA%E5%BC%8F%E4%BC%9A%E7%A4%BE&sc=1-8&sp=-1&qs=n&sk=", companyNameE);
            Uri uri2 = new Uri(link);
            string filename = companyName + ".htm";

            UriBuilder uri = new UriBuilder(uri2.AbsoluteUri);
            string path = @"C:\Temp\bing\" + uri.Host + "\\" ;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(uri2, path + filename);
                Application.DoEvents();
                getCompanyInfor2(path + filename);
            }

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
                    if (url.StartsWith("http"))
                    {
                        if (url.Split('/').Length < 6 && url.IndexOf("?") < 0)
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
                txtWeb.Text = url;
                webBrowser1.Navigate(new Uri(txtWeb.Text));
            }

        }


    }
}
