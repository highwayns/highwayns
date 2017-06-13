using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace highwayns
{
    public partial class FormPersonal : Form
    {
        public FormPersonal()
        {
            InitializeComponent();
        }

        private void btnSearchWeb_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            path = Path.Combine(path, "personal");
            getLine(path);
            MessageBox.Show("Search Web Over!");
        }
        private void getLine(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*.htm");
            foreach (string file in files)
            {
                getPersonalInfor(file);
            }
            string[] subdirs = Directory.GetDirectories(dir);
            foreach (string subdir in subdirs)
            {
                getLine(subdir);
            }
            //MessageBox.Show("Over!");
        }
        private void getPersonalInfor(string fileName)
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
                getPersonalInfor(nodes, fileName);
            }

        }
        private void getPersonalInfor(HtmlAgilityPack.HtmlNodeCollection nodes,string fileName)
        {
            string Idx = Path.GetFileNameWithoutExtension(fileName);
            if (Idx == "1") Idx = "";
            foreach (HtmlAgilityPack.HtmlNode node in nodes)
            {
                string url = node.GetAttributeValue("href", "");
                if (url.StartsWith("mailto"))
                {
                    if (node.InnerHtml.IndexOf("@") > -1 && node.InnerHtml.IndexOf("admin") < 0)
                    {
                        string[] data = new string[9];
                        data[0] = dgvData.Rows.Count.ToString();
                        data[6] = node.InnerHtml;
                        HtmlAgilityPack.HtmlNode node2 = nodes[nodes[node] -1];
                        data[1] = node2.GetAttributeValue("name","");
                        string link = string.Format("http://www.minami-osaka.net/jobs{1}.htm#{0}", data[1],Idx);
                        foreach (HtmlAgilityPack.HtmlNode subnode in nodes)
                        {
                            string url2 = subnode.GetAttributeValue("href", "");
                            if (url2 == link)
                            {
                                HtmlAgilityPack.HtmlNode tdnode = subnode.ParentNode;
                                if (tdnode.Name.ToUpper() != "TD") tdnode = tdnode.ParentNode;
                                HtmlAgilityPack.HtmlNode trnode = tdnode.ParentNode;
                                if (tdnode.Name.ToUpper() == "TR")
                                {
                                    data[2] = tdnode.ChildNodes[5].InnerText;
                                    data[3] = tdnode.ChildNodes[3].InnerText;
                                    data[4] = tdnode.ChildNodes[7].InnerText;
                                    data[8] = tdnode.ChildNodes[9].InnerText + " " + tdnode.ChildNodes[11].InnerText;
                                }
                                else
                                {
                                    data[2] = trnode.ChildNodes[5].InnerText;
                                    data[3] = trnode.ChildNodes[3].InnerText;
                                    data[4] = trnode.ChildNodes[7].InnerText;
                                    data[8] = trnode.ChildNodes[9].InnerText + " " + trnode.ChildNodes[11].InnerText;
                                }
                                break;
                            }
                        }
                        dgvData.Rows.Add(data);
                    }
                    
                }
                Application.DoEvents();                
            }
        }

        private void btnLoadTxt_Click(object sender, EventArgs e)
        {

        }
    }
}
