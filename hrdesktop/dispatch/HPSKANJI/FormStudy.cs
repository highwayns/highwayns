using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;

namespace HPSKANJI
{
    public partial class FormStudy : Form
    {
        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        private static extern int mciSendString(String command,
           StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        private string aliasName = "MediaFile";
        private string lessonid;
        private DB db;
        private int CurrentIndex = 0;
        DataSet ds = new DataSet();
        public FormStudy(DB db, string lessonid, string lesson, string className)
        {
            this.db = db;
            this.lessonid = lessonid;
            InitializeComponent();
            this.lblLesson.Text = className + " " + lesson;
        }
        /// <summary>
        /// 現在内容を表示
        /// </summary>
        private void showCurrentContent()
        {
            txtContents.Text = ds.Tables[0].Rows[CurrentIndex - 1][2].ToString();
            txtAudioFile.Text = ds.Tables[0].Rows[CurrentIndex - 1][3].ToString();
            txtBlock1.Text = ds.Tables[0].Rows[CurrentIndex - 1][4].ToString();
            txtBlock2_1.Text = ds.Tables[0].Rows[CurrentIndex - 1][5].ToString();
            txtBlock2_2.Text = ds.Tables[0].Rows[CurrentIndex - 1][6].ToString();
            txtBlock2_3.Text = ds.Tables[0].Rows[CurrentIndex - 1][7].ToString();
            txtBlock2_4.Text = ds.Tables[0].Rows[CurrentIndex - 1][8].ToString();

            txtBlock3_1.Text = ds.Tables[0].Rows[CurrentIndex - 1][9].ToString();
            txtBlock3_2.Text = ds.Tables[0].Rows[CurrentIndex - 1][10].ToString();
            txtBlock3_3.Text = ds.Tables[0].Rows[CurrentIndex - 1][11].ToString();
            txtBlock3_4.Text = ds.Tables[0].Rows[CurrentIndex - 1][12].ToString();

            txtBlock4_1.Text = ds.Tables[0].Rows[CurrentIndex - 1][13].ToString();
            txtBlock4_2.Text = ds.Tables[0].Rows[CurrentIndex - 1][14].ToString();
            txtBlock4_3.Text = ds.Tables[0].Rows[CurrentIndex - 1][15].ToString();
            txtBlock4_4.Text = ds.Tables[0].Rows[CurrentIndex - 1][16].ToString();

            txtBlock5_1.Text = ds.Tables[0].Rows[CurrentIndex - 1][17].ToString();
            txtBlock5_2.Text = ds.Tables[0].Rows[CurrentIndex - 1][18].ToString();
            txtBlock5_3.Text = ds.Tables[0].Rows[CurrentIndex - 1][19].ToString();
            txtBlock5_4.Text = ds.Tables[0].Rows[CurrentIndex - 1][20].ToString();

            txtBlock6_1.Text = ds.Tables[0].Rows[CurrentIndex - 1][21].ToString();
            txtBlock6_2.Text = ds.Tables[0].Rows[CurrentIndex - 1][22].ToString();
            txtBlock6_3.Text = ds.Tables[0].Rows[CurrentIndex - 1][23].ToString();
            txtBlock6_4.Text = ds.Tables[0].Rows[CurrentIndex - 1][24].ToString();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            if (db.GetContent(0, 0, @"[ContentId],[ContentType],[Contents],[AudioFile],
                [block1],
                [block2_1],[block2_2],[block2_3],[block2_4],
                [block3_1],[block3_2],[block3_3],[block3_4],
                [block4_1],[block4_2],[block4_3],[block4_4],
                [block5_1],[block5_2],[block5_3],[block5_4],
                [block6_1],[block6_2],[block6_3],[block6_4]
                ", "LessonId=" + lessonid, "", ref ds) && ds.Tables[0].Rows.Count > 0)
            {
                CurrentIndex = 1;
                showCurrentContent();
                btnPrev.Enabled = false;
                if (CurrentIndex < ds.Tables[0].Rows.Count)
                {
                    btnNext.Enabled = true;
                }
                else
                {
                    btnNext.Enabled = false;
                }

            }
            else
            {
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
            }
            btnPlay.Tag = "play";
        }
        /// <summary>
        /// 前へ移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click(object sender, EventArgs e)
        {
            CurrentIndex--;
            showCurrentContent();
            if (CurrentIndex <= 1)
            {
                btnPrev.Enabled = false;
            }
            if (CurrentIndex < ds.Tables[0].Rows.Count)
            {
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }
            btnPlay.Tag = "play";
        }
        /// <summary>
        /// 次へ移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            CurrentIndex++;
            showCurrentContent();
            btnPrev.Enabled = true;
            if (CurrentIndex < ds.Tables[0].Rows.Count)
            {
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }

        }
        /// <summary>
        /// 音声放送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlay_Click(object sender, EventArgs e)
        {
            string fileName = txtAudioFile.Text;
            if (!string.IsNullOrEmpty(fileName))
            {
                if (btnPlay.Tag.ToString() != "stop")
                {
                    btnPlay.Tag = "stop";

                    string cmd;
                    //ファイルを開く
                    cmd = "open \"" + fileName + "\" type mpegvideo alias " + aliasName;
                    if (mciSendString(cmd, null, 0, IntPtr.Zero) != 0)
                        return;
                    //再生する
                    cmd = "play " + aliasName;
                    mciSendString(cmd, null, 0, IntPtr.Zero);
                }
                else
                {
                    btnPlay.Tag = "play";
                    string cmd;
                    //再生しているWAVEを停止する
                    cmd = "stop " + aliasName;
                    mciSendString(cmd, null, 0, IntPtr.Zero);
                    //閉じる
                    cmd = "close " + aliasName;
                    mciSendString(cmd, null, 0, IntPtr.Zero);

                }
            }
        }
        /// <summary>
        /// 画面起動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormStudy_Load(object sender, EventArgs e)
        {
            init();
        }
    }
}
