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
    public partial class FormEditor : Form
    {
        private string lessonid;
        private DB db;
        public FormEditor(DB db, string lessonid, string lesson, string className)
        {
            this.db = db;
            this.lessonid = lessonid;
            InitializeComponent();
            this.lblLesson.Text = className + " " + lesson;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (db.GetContent(0, 0, @"[ContentId],[ContentType],[Contents],[AudioFile],
                [block1],
                [block2_1],[block2_2],[block2_3],[block2_4],
                [block3_1],[block3_2],[block3_3],[block3_4],
                [block4_1],[block4_2],[block4_3],[block4_4],
                [block5_1],[block5_2],[block5_3],[block5_4],
                [block6_1],[block6_2],[block6_3],[block6_4]
                ", "LessonId=" + lessonid, "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
            }

        }

        /// <summary>
        /// 内容追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = 0;
            String fieldlist = @"[LessonId],[ContentType],[Contents],[AudioFile],
                [block1],
                [block2_1],[block2_2],[block2_3],[block2_4],
                [block3_1],[block3_2],[block3_3],[block3_4],
                [block4_1],[block4_2],[block4_3],[block4_4],
                [block5_1],[block5_2],[block5_3],[block5_4],
                [block6_1],[block6_2],[block6_3],[block6_4],
                UserId";
            String valuelist = "" + lessonid + ",'"
                + txtContentType.Text + "','" + txtContents.Text + "','" + txtAudioFile.Text
                + "','" + txtBlock1.Text
                + "','" + txtBlock2_1.Text + "','" + txtBlock2_2.Text + "','" + txtBlock2_3.Text + "','" + txtBlock2_4.Text + "','"
                + "','" + txtBlock3_1.Text + "','" + txtBlock3_2.Text + "','" + txtBlock3_3.Text + "','" + txtBlock3_4.Text + "','"
                + "','" + txtBlock4_1.Text + "','" + txtBlock4_2.Text + "','" + txtBlock4_3.Text + "','" + txtBlock4_4.Text + "','"
                + "','" + txtBlock5_1.Text + "','" + txtBlock5_2.Text + "','" + txtBlock5_3.Text + "','" + txtBlock5_4.Text + "','"
                + "','" + txtBlock6_1.Text + "','" + txtBlock6_2.Text + "','" + txtBlock6_3.Text + "','" + txtBlock6_4.Text + "','"
                + db.db.UserID + "'";
            if (db.SetContent(0, 0, fieldlist,
                                 "", valuelist, out id))
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0180I", db.db.Language);
                MessageBox.Show(msg);
                init();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0181I", db.db.Language);
                MessageBox.Show(msg);
            }


        }
        /// <summary>
        /// 画面起動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormWork_Load(object sender, EventArgs e)
        {
            init();
        }
        /// <summary>
        /// 内容更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "ContentId=" + txtContentId.Text;
                String valuesql = "ContentType='" + txtContentType.Text
                    + "',Contents='" + txtContents.Text
                    + "',AudioFile='" + txtAudioFile.Text
                    + "',block1='" + txtBlock1.Text
                    + "',block2_1='" + txtBlock2_1.Text
                    + "',block2_2='" + txtBlock2_2.Text
                    + "',block2_3='" + txtBlock2_3.Text
                    + "',block2_4='" + txtBlock2_4.Text

                    + "',block3_1='" + txtBlock3_1.Text
                    + "',block3_2='" + txtBlock3_2.Text
                    + "',block3_3='" + txtBlock3_3.Text
                    + "',block3_4='" + txtBlock3_4.Text

                    + "',block4_1='" + txtBlock4_1.Text
                    + "',block4_2='" + txtBlock4_2.Text
                    + "',block4_3='" + txtBlock4_3.Text
                    + "',block4_4='" + txtBlock4_4.Text

                    + "',block5_1='" + txtBlock5_1.Text
                    + "',block5_2='" + txtBlock5_2.Text
                    + "',block5_3='" + txtBlock5_3.Text
                    + "',block5_4='" + txtBlock5_4.Text

                    + "',block6_1='" + txtBlock6_1.Text
                    + "',block6_2='" + txtBlock6_2.Text
                    + "',block6_3='" + txtBlock6_3.Text
                    + "',block6_4='" + txtBlock6_4.Text
                    + "'"
                    ;
                if (db.SetContent(0, 1, "", wheresql, valuesql, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0182I", db.db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0183I", db.db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0179I", db.db.Language);
                MessageBox.Show(msg);
            }


        }
        /// <summary>
        /// 内容削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "ContentId=" + txtContentId.Text;
                if (db.SetContent(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0184I", db.db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0185I", db.db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0179I", db.db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 行選択変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtContentId.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtContentType.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtContents.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtAudioFile.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtBlock1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtBlock2_1.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                txtBlock2_2.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                txtBlock2_3.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                txtBlock2_4.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();

                txtBlock3_1.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
                txtBlock3_2.Text = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
                txtBlock3_3.Text = dataGridView1.SelectedRows[0].Cells[11].Value.ToString();
                txtBlock3_4.Text = dataGridView1.SelectedRows[0].Cells[12].Value.ToString();

                txtBlock4_1.Text = dataGridView1.SelectedRows[0].Cells[13].Value.ToString();
                txtBlock4_2.Text = dataGridView1.SelectedRows[0].Cells[14].Value.ToString();
                txtBlock4_3.Text = dataGridView1.SelectedRows[0].Cells[15].Value.ToString();
                txtBlock4_4.Text = dataGridView1.SelectedRows[0].Cells[16].Value.ToString();

                txtBlock5_1.Text = dataGridView1.SelectedRows[0].Cells[17].Value.ToString();
                txtBlock5_2.Text = dataGridView1.SelectedRows[0].Cells[18].Value.ToString();
                txtBlock5_3.Text = dataGridView1.SelectedRows[0].Cells[19].Value.ToString();
                txtBlock5_4.Text = dataGridView1.SelectedRows[0].Cells[20].Value.ToString();

                txtBlock6_1.Text = dataGridView1.SelectedRows[0].Cells[21].Value.ToString();
                txtBlock6_2.Text = dataGridView1.SelectedRows[0].Cells[22].Value.ToString();
                txtBlock6_3.Text = dataGridView1.SelectedRows[0].Cells[23].Value.ToString();
                txtBlock6_4.Text = dataGridView1.SelectedRows[0].Cells[24].Value.ToString();
            }

        }
    }
}
