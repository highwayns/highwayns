using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using QWeiboSDK;
using NC.HPS.Lib;
using System.IO;

namespace HPSManagement
{
    public partial class FormQWeiboPublish : Form
    {
        private string appKey = null;
        private string appSecret = null;
        private string accessKey = null;
        private string accessSecret = null;
        private DataSet ds_media = null;
        private int CurrentIndex = 0;

        //用于线程间通讯
        private int myhWnd = 0;

        //线程通讯数据队列
        private Dictionary<int, string> dicData = new Dictionary<int, string>();

        public void SetAppKey(string appKey)
        {
            this.appKey = appKey;
        }
        public void SetAppSecret(string appSecret)
        {
            this.appSecret = appSecret;
        }
        public void SetAccessKey(string accessKey)
        {
            this.accessKey = accessKey;
        }
        public void SetAccessSecret(string accessSecret)
        {
            this.accessSecret = accessSecret;
        }
        private CmWinServiceAPI db;
        /// <summary>
        /// QQ微博信息发布
        /// </summary>
        /// <param name="db"></param>
        public FormQWeiboPublish(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
            myhWnd = this.Handle.ToInt32();
            getPublishData();
        }
        /// <summary>
        /// exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 发布数据取得
        /// </summary>
        private void getPublishData()
        {
            if (db.GetMediaPublishVW(0, 0, "*", "MediaType='TENCENT' AND 发行状态='未发行'", "", ref ds_media)
                && ds_media.Tables[0].Rows.Count > 0)
            {
                CurrentIndex = 1;
                lblNum.Text = Convert.ToString(CurrentIndex) + "/" + Convert.ToString(ds_media.Tables[0].Rows.Count);
                txtTitle.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["名称"].ToString()
                    + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行期号"].ToString()
                    + "[" + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行日期"].ToString() + "]";
                txtMessage.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["文本内容"].ToString();
                txtPicturePath.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["本地图片"].ToString();
                if (File.Exists(txtPicturePath.Text))
                {
                    pbCover.Image = new Bitmap(txtPicturePath.Text);
                }
                else
                {
                    if (pbCover.Image != null) pbCover.Image.Dispose();
                }
                btnPrev.Enabled = false;
                if (CurrentIndex < ds_media.Tables[0].Rows.Count)
                {
                    btnNext.Enabled = true;
                }
                else
                {
                    btnNext.Enabled = false;
                }
                btnPost.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
                btnPost.Enabled = false;
            }
        }
        /// <summary>
        /// next Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            CurrentIndex++;
            lblNum.Text = Convert.ToString(CurrentIndex) + "/" + Convert.ToString(ds_media.Tables[0].Rows.Count);
            txtTitle.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["名称"].ToString()
                + ds_media.Tables[0].Rows[CurrentIndex - 1]["期号"].ToString()
                + "[" + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行日期"].ToString() + "]";
            txtMessage.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["文本内容"].ToString();
            txtPicturePath.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["本地图片"].ToString();
            if (File.Exists(txtPicturePath.Text))
            {
                pbCover.Image = new Bitmap(txtPicturePath.Text);
            }
            else
            {
                if (pbCover.Image != null) pbCover.Image.Dispose();
            }
            btnPrev.Enabled = true;
            if (CurrentIndex < ds_media.Tables[0].Rows.Count)
            {
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }

        }
        /// <summary>
        /// Prev Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click(object sender, EventArgs e)
        {
            CurrentIndex--;
            lblNum.Text = Convert.ToString(CurrentIndex) + "/" + Convert.ToString(ds_media.Tables[0].Rows.Count);
            txtTitle.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["名称"].ToString()
                + ds_media.Tables[0].Rows[CurrentIndex - 1]["期号"].ToString()
                + "[" + ds_media.Tables[0].Rows[CurrentIndex - 1]["发行日期"].ToString() + "]";
            txtMessage.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["文本内容"].ToString();
            txtPicturePath.Text = ds_media.Tables[0].Rows[CurrentIndex - 1]["本地图片"].ToString();
            if (File.Exists(txtPicturePath.Text))
            {
                pbCover.Image = new Bitmap(txtPicturePath.Text);
            }
            else
            {
                if (pbCover.Image != null) pbCover.Image.Dispose();
            }
            if (CurrentIndex <= 1)
            {
                btnPrev.Enabled = false;
            }
            if (CurrentIndex < ds_media.Tables[0].Rows.Count)
            {
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }

        }
        /**
         *设置发布状态
        **/
        private void setMediaPublishStatus(String mediaPublishId, String Status)
        {
            int id = 0;
            String wheresql = "发布编号=" + mediaPublishId;
            String valuesql = "发行状态='" + Status + "'";
            db.SetMediaPublish(0, 1, "", wheresql, valuesql, out id);
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntPost_Click(object sender, EventArgs args)
        {
            pgbPost.Value = 0;
            pgbPost.Maximum = ds_media.Tables[0].Rows.Count;
            for (int idx = 0; idx < ds_media.Tables[0].Rows.Count; idx++)
            {
                String mediaPublishId = ds_media.Tables[0].Rows[idx]["发布编号"].ToString();
                setMediaPublishStatus(mediaPublishId, "发行中");

                string content = ds_media.Tables[0].Rows[idx]["名称"].ToString()
                    + ds_media.Tables[0].Rows[idx]["发行期号"].ToString()
                    + "[" + ds_media.Tables[0].Rows[idx]["发行日期"].ToString() + "]\r\n"
                    + ds_media.Tables[0].Rows[idx]["文本内容"].ToString();
                string picpath = ds_media.Tables[0].Rows[idx]["本地图片"].ToString();

                try
                {
                    OauthKey oauthKey = new OauthKey();
                    oauthKey.customKey = appKey;
                    oauthKey.customSecret = appSecret;
                    oauthKey.tokenKey = accessKey;
                    oauthKey.tokenSecret = accessSecret;

                    ///发送带图片微博
                    t twit = new t(oauthKey, "json");
                    UTF8Encoding utf8 = new UTF8Encoding();

                    string ret = twit.add_pic(utf8.GetString(utf8.GetBytes(content)),
                                  utf8.GetString(utf8.GetBytes("127.0.0.1")),
                                  utf8.GetString(utf8.GetBytes("")),
                                  utf8.GetString(utf8.GetBytes("")),
                                  utf8.GetString(utf8.GetBytes(picpath))
                                 );
                    setMediaPublishStatus(mediaPublishId, "发行完");
                }
                catch (Exception ex)
                {
                    setMediaPublishStatus(mediaPublishId, "未发行");
                    NCLogger.GetInstance().WriteExceptionLog(ex);
                }
            }
        }

        /// <summary>
        /// Form_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Load(object sender, EventArgs e)
        {
            getPublishData();
        }
        /// <summary>
        /// 回调函数准备
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("User32.dll", EntryPoint = "PostMessage")]
        private static extern int PostMessage(
        int hWnd, //目标窗口的handle
        int Msg, // 消息
        int wParam, // 第一个消息参数
        int lParam // 第二个消息参数
        );
        const int WM_HTTPNOTIFY = 8000;

        //该函数用于异步http的场景，由http线程调用，通知主线程显示http结果
        protected void RequestCallback(int key, string content)
        {
            //转换线程调用
            lock (dicData)
            {
                Encoding utf8 = Encoding.GetEncoding(65001);
                Encoding defaultChars = Encoding.Default;
                byte[] temp = utf8.GetBytes(content);
                byte[] temp1 = Encoding.Convert(utf8, defaultChars, temp);
                string result = defaultChars.GetString(temp1);
                dicData.Add(key, result);
            }

            PostMessage(myhWnd, WM_HTTPNOTIFY, 0, 0);
        }
        /// <summary>
        /// 主线程处理
        /// </summary>
        /// <param name="m"></param>
        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case WM_HTTPNOTIFY:

                    //把数据取出来缓存，以减少加锁的时间
                    Dictionary<int, string> dicText = new Dictionary<int, string>();
                    lock (dicData)
                    {
                        foreach (KeyValuePair<int, string> a in dicData)
                        {
                            dicText.Add(a.Key, a.Value);
                        }
                        dicData.Clear();
                    }
                    
                    break;
                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }
    }
}