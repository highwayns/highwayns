using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;

namespace HPSManagement
{
    public partial class FormMag : Form
    {
        private CmWinServiceAPI db;

        public FormMag(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }
        /// <summary>
        /// 画面初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormWeekly_Load(object sender, EventArgs e)
        {
            initBig();
            initMiddle();
            initSmall();
            init("","","","");
        }
        /// <summary>
        /// 初期化大分类
        /// </summary>
        private void initBig()
        {
            DataSet ds = new DataSet();
            if (db.GetMag(0, 0, "distinct 大分类", "", "", ref ds))
            {
                cmbBig4s.DataSource = ds.Tables[0];
                cmbBig4s.DisplayMember = "大分类";
            }
            DataSet ds2 = new DataSet();
            if (db.GetMag(0, 0, "distinct 大分类", "", "", ref ds2))
            {
                cmbBig.DataSource = ds2.Tables[0];
                cmbBig.DisplayMember = "大分类";
            }
            cmbBig4s.Text = "";
        }
        /// <summary>
        /// 初期化中分类
        /// </summary>
        private void initMiddle()
        {
            DataSet ds = new DataSet();
            if (db.GetMag(0, 0, "distinct 中分类", "", "", ref ds))
            {
                cmbMiddle4s.DataSource = ds.Tables[0];
                cmbMiddle4s.DisplayMember = "中分类";
            }
            DataSet ds2 = new DataSet();
            if (db.GetMag(0, 0, "distinct 中分类", "", "", ref ds2))
            {
                cmbMiddle.DataSource = ds2.Tables[0];
                cmbMiddle.DisplayMember = "中分类";
            }
            cmbMiddle4s.Text = "";
        }
        /// <summary>
        /// 初期化小分类
        /// </summary>
        private void initSmall()
        {
            DataSet ds = new DataSet();
            if (db.GetMag(0, 0, "distinct 小分类", "", "", ref ds))
            {
                cmbSmall4s.DataSource = ds.Tables[0];
                cmbSmall4s.DisplayMember = "小分类";
            }
            DataSet ds2 = new DataSet();
            if (db.GetMag(0, 0, "distinct 小分类", "", "", ref ds2))
            {
                cmbSmall.DataSource = ds2.Tables[0];
                cmbSmall.DisplayMember = "小分类";
            }
            cmbSmall4s.Text = "";
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void init(string big,string middle,string small,string islocal)
        {
            DataSet ds = new DataSet();
            string wheresql = "1=1";
            if (!string.IsNullOrEmpty(big))
            {
                wheresql += " and 大分类='"+big+"'";
            }
            if (!string.IsNullOrEmpty(middle))
            {
                wheresql += " and 中分类='" + middle + "'";
            }
            if (!string.IsNullOrEmpty(small))
            {
                wheresql += " and 小分类='" + small + "'";
            }
            if (!string.IsNullOrEmpty(islocal))
            {
                wheresql += " and 是否本机='" + islocal + "'";
            }
            if (db.GetMag(0, 0, "*", wheresql, "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
            }
        }
        /// <summary>
        /// 增加期刊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = 0;
            String SID = (Guid.NewGuid()).ToString();
            String fieldlist = "名称,主编,发行周期,发行地点,大分类,中分类,小分类,是否加密,密码,是否收费,每期金额,货币类型,主编信箱,是否本机,UserID,S期刊编号";
            String valuelist = "'" + txtName.Text + "','" + txtEditor.Text + "','"
                + txtCycle.Text + "','" + txtAddress.Text + "','" + cmbBig.Text + "','" + cmbMiddle.Text + "','" + cmbSmall.Text + "','" 
                + cmbIsEncrpt.Text + "','" + txtPassword.Text +
                "','" + cmbIsIncharge.Text + "'," + txtAmount.Text + ",'" + cmbCurrency.Text +
                "','" + txtEditorMail.Text +
                "','Y','" + db.UserID + "','"+SID+"'";
            if (db.SetMag(0, 0, fieldlist,
                                 "", valuelist, out id))
            {
                fieldlist = "S期刊编号,编号,名称,主编,发行周期,发行地点,大分类,中分类,小分类,是否加密,密码,是否收费,每期金额,货币类型,主编信箱,是否本机,UserID";
                valuelist = "'" + SID + "'," + id.ToString() + ",'" + txtName.Text + "','" + txtEditor.Text + "','"
                    + txtCycle.Text + "','" + txtAddress.Text + "','" + cmbBig.Text + "','" + cmbMiddle.Text + "','" + cmbSmall.Text + "','"
                    + cmbIsEncrpt.Text + "','" + txtPassword.Text +
                    "','" + cmbIsIncharge.Text + "'," + txtAmount.Text + ",'" + cmbCurrency.Text +
                    "','" + txtEditorMail.Text +
                    "','N','" + db.UserID + "'";
                if (db.SetSMag(0, 0, fieldlist,
                                     "", valuelist, out id))
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0030I", db.Language);
                    MessageBox.Show(msg);
                    string isLocal = "";
                    if (chbIslocal.Checked) { isLocal = "Y"; }
                    init(cmbBig4s.Text, cmbMiddle4s.Text, cmbSmall4s.Text, isLocal);
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0031I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0031I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 更新期刊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "编号="+txtId.Text;
                String valuesql = "名称='" + txtName.Text 
                    + "',主编='" + txtEditor.Text 
                    + "',发行周期='" + txtCycle.Text 
                    + "',大分类='" + cmbBig.Text 
                    + "',中分类='" + cmbMiddle.Text 
                    + "',小分类='" + cmbSmall.Text 
                    + "',是否加密='" + cmbIsEncrpt.Text 
                    + "',密码='" + txtPassword.Text 
                    + "',是否收费='" + cmbIsIncharge.Text 
                    + "',每期金额=" + txtAmount.Text 
                    + ",货币类型='" + cmbCurrency.Text 
                    + "',发行地点='" + txtAddress.Text 
                    + "',主编信箱='" + txtEditorMail.Text 
                    + "'";
                if (db.SetMag(0, 1, "", wheresql, valuesql, out id) && id == 1)
                {
                    wheresql = "S期刊编号='" + lblSID.Text+"'";
                    valuesql = "名称='" + txtName.Text
                        + "',主编='" + txtEditor.Text
                        + "',发行周期='" + txtCycle.Text
                        + "',大分类='" + cmbBig.Text
                        + "',中分类='" + cmbMiddle.Text
                        + "',小分类='" + cmbSmall.Text
                        + "',是否加密='" + cmbIsEncrpt.Text
                        + "',密码='" + txtPassword.Text
                        + "',是否收费='" + cmbIsIncharge.Text
                        + "',每期金额=" + txtAmount.Text
                        + ",货币类型='" + cmbCurrency.Text
                        + "',发行地点='" + txtAddress.Text
                        + "',主编信箱='" + txtEditorMail.Text
                        + "'";
                    if (db.SetSMag(0, 1, "", wheresql, valuesql, out id) && id == 1)
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0032I", db.Language);
                        MessageBox.Show(msg);
                        string isLocal = "";
                        if (chbIslocal.Checked) { isLocal = "Y"; }
                        init(cmbBig4s.Text, cmbMiddle4s.Text, cmbSmall4s.Text, isLocal);
                    }
                    else
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0033I", db.Language);
                        MessageBox.Show(msg);
                    }
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0033I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0029I", db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 选择行变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                txtId.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtEditor.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtCycle.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtAddress.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                cmbBig.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                cmbMiddle.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                cmbSmall.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                cmbIsEncrpt.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
                txtPassword.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
                cmbIsIncharge.Text = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
                txtAmount.Text = dataGridView1.SelectedRows[0].Cells[11].Value.ToString();
                cmbCurrency.Text = dataGridView1.SelectedRows[0].Cells[12].Value.ToString();
                txtEditorMail.Text = dataGridView1.SelectedRows[0].Cells[13].Value.ToString();
                txtIsLocal.Text = dataGridView1.SelectedRows[0].Cells[14].Value.ToString();
                lblSID.Text = dataGridView1.SelectedRows[0].Cells[16].Value.ToString();
            }
        }
        /// <summary>
        /// 期刊发行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPublish_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                String MagId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                String MagName = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                String SMagId = dataGridView1.SelectedRows[0].Cells[16].Value.ToString();
                FormPublish form = new FormPublish(db, MagId, MagName,SMagId);
                form.ShowDialog();
            }
        }
        /// <summary>
        /// 删除期刊和相关的发行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "期刊编号=" + txtId.Text;
                DataSet ds = new DataSet();
                if (db.GetPublish(0, 0, "*", wheresql, "", ref ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        String PublishId = dr[0].ToString();
                        deletePublish(PublishId);
                    }
                }
                wheresql = "编号=" + txtId.Text;
                if (db.SetMag(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    wheresql = "S期刊编号=" + lblSID.Text;
                    if (db.SetSMag(0, 2, "", wheresql, "", out id) && id == 2)
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0034I", db.Language);
                        MessageBox.Show(msg);
                        string isLocal = "";
                        if (chbIslocal.Checked) { isLocal = "Y"; }
                        init(cmbBig4s.Text, cmbMiddle4s.Text, cmbSmall4s.Text, isLocal);
                    }
                    else
                    {
                        string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0035I", db.Language);
                        MessageBox.Show(msg);
                    }
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0035I", db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0029I", db.Language);
                MessageBox.Show(msg);
            }
        }

        /// <summary>
        /// 删除期刊发行数据
        /// </summary>
        /// <param name="publishId"></param>
        private void deletePublish(String publishId)
        {
            int id = 0;
            String wheresql = "发行编号=" + publishId;
            ///期刊送信删除
            if (db.SetSend(0, 2, "", wheresql, "", out id) && id == 2)
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0036I", db.Language);
                MessageBox.Show(msg);
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0037I", db.Language);
                MessageBox.Show(msg);
            }
            ///期刊送信历史删除
            if (db.SetHistory(0, 2, "", wheresql, "", out id) && id == 2)
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0038I", db.Language);
                MessageBox.Show(msg);
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0039I", db.Language);
                MessageBox.Show(msg);
            }
            ///期刊FTP上传删除
            if (db.SetFTPUpload(0, 2, "", wheresql, "", out id) && id == 2)
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0136I", db.Language);
                MessageBox.Show(msg);
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0137I", db.Language);
                MessageBox.Show(msg);
            }
            ///期刊FTP上出历史删除
            if (db.SetFTPHistory(0, 2, "", wheresql, "", out id) && id == 2)
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0138I", db.Language);
                MessageBox.Show(msg);
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0139I", db.Language);
                MessageBox.Show(msg);
            }
            ///媒体发布删除
            if (db.SetMediaPublish(0, 2, "", wheresql, "", out id) && id == 2)
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0146I", db.Language);
                MessageBox.Show(msg);
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0147I", db.Language);
                MessageBox.Show(msg);
            }
            ///媒体发布历史删除
            if (db.SetMediaHistory(0, 2, "", wheresql, "", out id) && id == 2)
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0148I", db.Language);
                MessageBox.Show(msg);
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0149I", db.Language);
                MessageBox.Show(msg);
            }
            ///期刊发行删除
            if (db.SetPublish(0, 2, "", wheresql, "", out id) && id == 2)
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0040I", db.Language);
                MessageBox.Show(msg);
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0041I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string isLocal = "";
            if (chbIslocal.Checked) { isLocal = "Y"; }
            init(cmbBig4s.Text, cmbMiddle4s.Text,cmbSmall4s.Text, isLocal);
        }
        /// <summary>
        /// 订阅信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubscript_Click(object sender, EventArgs e)
        {


        }
    }
}
