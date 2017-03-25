using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace weblogin_test
{
    public partial class weblogin : Form
    {
        public weblogin()
        {
            InitializeComponent();
        }
        private bool submited;
        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute( IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);
        private void weblogin_Load(object sender, EventArgs e)
        {

            
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElement weibopub = webBrowser1.Document.GetElementById("weiboPublisher");
            HtmlElement plcmain = webBrowser1.Document.GetElementById("plc_main");
            HtmlElement timesec = webBrowser1.Document.GetElementById("timeSec");
            if (weibopub==null&&plcmain==null&&timesec==null)
            {
                Load_delay.Enabled = true;
            }
            else if (plcmain!=null)
            {
                //转到一键分享
                if (submited == false)
                {
                    Redirect_delay.Enabled = Enabled;
                }
            }
            else if (weibopub != null)
            {
                //填写分享内容
                weibopub.InnerText = textBox_test.Text;
                Pub_delay.Enabled = true;
            }
            else if (timesec!=null)
            {
                label_pub_status.Text = "发布完成";
                submited = true;
                label_pub_status.Text = "status";
                webBrowser1.Navigate("http://weibo.com/login");
            }
            
        }

        private void Load_delay_Tick(object sender, EventArgs e)
        {
            HtmlElementCollection inputboxes = webBrowser1.Document.GetElementsByTagName("input");
            //HtmlElement savestate = webBrowser1.Document.GetElementById("login_form_savestate");
            // savestate.InvokeMember("click");

            // 填写用户名和密码。更新cookies
            label_pub_status.Text = "登录中...";
            for (int i = 0; i < inputboxes.Count; i++)
            {
                if (inputboxes[i].GetAttribute("name") == "username")
                {
                    inputboxes[i].SetAttribute("value", "1662098461@qq.com");
                }
                if (inputboxes[i].GetAttribute("name") == "password")
                {
                    inputboxes[i].SetAttribute("value", "chojo123456");
                }

            }
            Click_delay.Enabled = true;
            Load_delay.Enabled = false;
        }

        private void Click_delay_Tick(object sender, EventArgs e)
        {
            HtmlElementCollection links = webBrowser1.Document.GetElementsByTagName("a");
            for (int j = 0; j < links.Count; j++)
            {
                if (links[j].GetAttribute("action-type") == "btn_submit" && submited == false)
                {
                    links[j].InvokeMember("click");
                }
            }
            Click_delay.Enabled = false;
        }

        private void Redirect_delay_Tick(object sender, EventArgs e)
        {
            //转到一键分享
            webBrowser1.Navigate("http://service.weibo.com/share/share.php?url=%url%&appkey=&title=&pic=&ralateUid=&language=");
            label_pub_status.Text = "发布中...";
            Redirect_delay.Enabled = false;
        }

        private void Pub_delay_Tick(object sender, EventArgs e)
        {
            HtmlElement sharebtn = webBrowser1.Document.GetElementById("shareIt");
            sharebtn.InvokeMember("click");
            Pub_delay.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            submited = false;
            //清空cookies
            ShellExecute(IntPtr.Zero, "open", "rundll32.exe", " InetCpl.cpl,ClearMyTracksByProcess 2", "", 0);
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
            webBrowser1.Navigate("http://weibo.com/login");
            //一键分享
            //webBrowser1.Navigate("http://service.weibo.com/share/share.php?url=%url%&appkey=&title=&pic=&ralateUid=&language=");
        }

    }
}
