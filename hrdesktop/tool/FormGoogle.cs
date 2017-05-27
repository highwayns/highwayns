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
using System.Web;
using System.Collections;

namespace highwayns
{
    public partial class FormGoogle : Form
    {
        public FormGoogle()
        {
            InitializeComponent();
        }
        // ポストデータの作成
        private byte[] MakePostDate(string input, Encoding enc)
        {
            string param = "";
            Hashtable ht = new Hashtable();
            ht["ie"] = "UTF-8";
            ht["hl"] = "ja";
            ht["oe"] = "UTF-8";
            ht["text"] = HttpUtility.UrlEncode(input, enc);
            ht["langpair"] = "ja|en";
            ht["gtrans"] = "";
            foreach (string k in ht.Keys)
            {
                param += String.Format("{0}={1}&", k, ht[k]);
            }
            byte[] data = Encoding.ASCII.GetBytes(param);
            return data;
        }
        //リクエストの作成
        private void MakeRequest(byte[] data, HttpWebRequest req)
        {
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
        }

        // ポストデータの書き込み
        private void WritePostData(byte[] data, HttpWebRequest req)
        {
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
        }

        // レスポンスの取得と読み込み
        private string GetResponse(Encoding enc, HttpWebRequest req)
        {
            WebResponse res = req.GetResponse();
            Stream resStream = res.GetResponseStream();
            StreamReader sr = new StreamReader(resStream, enc);
            string html = sr.ReadToEnd();
            sr.Close();
            resStream.Close();
            return html;
        }

        //　取得したHTMLから翻訳部分を抽出
        private string GetTranslate(string html)
        {
            string startStr = "<div id=result_box dir=\"ltr\">";
            string endStr = "</div>";
            int trimStartIdx = html.IndexOf(startStr) + startStr.Length;
            int trimEndIdx = html.IndexOf(endStr, trimStartIdx);
            string result = html.Substring(trimStartIdx, trimEndIdx - trimStartIdx);
            result = result.Replace("<br>", "\n");
            return result;
        }
        private string TranslateMethod(string authToken, string translating)
        {
            Encoding enc = Encoding.UTF8;
            string url = "http://translate.google.com/translate_t";	//翻訳サイトのURL
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            string source = translating;
            string html;
            byte[] data;

            data = MakePostDate(source, enc);	// ポストデータの作成
            MakeRequest(data, req);	//リクエストの作成
            WritePostData(data, req);	//ポストデータの書き込み
            html = GetResponse(enc, req);	// レスポンスの取得と読み込み
            string result = GetTranslate(html);		//　取得したHTMLから翻訳部分を抽出
            return result;

        }
        private void btnTranslate_Click(object sender, EventArgs e)
        {
            txtDestination.Text = TranslateMethod("",txtSource.Text);
        }

    }
}
