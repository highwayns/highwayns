using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using NC.HPS.Lib;
using System.Threading;
using System.Globalization;
//using Com.Seezt.Skins;

namespace HPSManagement
{
    public partial class frmLogin : Form
    {
        public static string M_str_name;        //记录登录用户名字
        public static string M_str_pwd;         //记录登录用户密码
        public static string M_str_right;       //记录登录用户的权限
        private CmWinServiceAPI db;
        private Boolean isInital= false;
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="db"></param>
        public frmLogin(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();
        }
        /// <summary>
        /// 画面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLogin_Load(object sender, EventArgs e)
        {
            init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            if (db.GetUser(0, 0, "*", "", "", ref ds))
            {
                cboxUName.DataSource = ds.Tables[0];
                cboxUName.DisplayMember = "UserName";
            }
            else
            {
                FormDataBase form = new FormDataBase(db);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (db.GetUser(0, 0, "*", "", "", ref ds))
                    {
                        cboxUName.DataSource = ds.Tables[0];
                        cboxUName.DisplayMember = "UserName";
                    }                    
                }
            }
            isInital = true;
            if (db.Language == "zh-CN")
            {
                cboxLanguage.Text = "中文";
            }
            else if (db.Language == "ja-JP")
            {
                cboxLanguage.Text = "日本語";
            }
            else
            {
                cboxLanguage.Text = "English";
            }
            isInital = false;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            String wheresql="UserName='"+cboxUName.Text+"' and UserPwd='"+NCCryp.Encrypto(txtPwd.Text)+"'";
            if (db.GetUser(0, 0, "*", wheresql, "", ref ds) && ds.Tables[0].Rows.Count == 1)
            {
                db.UserID = ds.Tables[0].Rows[0]["UserID"].ToString();
                db.UserRight = ds.Tables[0].Rows[0]["UserRight"].ToString();
                DialogResult = DialogResult.OK;
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0011I", db.Language);
                MessageBox.Show(msg, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPwd.Text = "";
                txtPwd.Focus();
            } 
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// 用户选择变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxUName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable tb = (DataTable)cboxUName.DataSource;
            String wheresql = "UserID='" + tb.Rows[cboxUName.SelectedIndex]["UserId"].ToString()+ "'";
            DataSet ds = new DataSet();
            if (db.GetUser(0, 0, "*", wheresql, "", ref ds) && ds.Tables[0].Rows.Count == 1)
            {
                labURight.Text = db.UserRightTable[ds.Tables[0].Rows[0]["UserRight"].ToString()].ToString();
            }
        }
        /// <summary>
        /// 用户密码回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnLogin.Focus();
        }
        /// <summary>
        /// 用户名回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxUName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txtPwd.Focus();
        }
        /// <summary>
        /// 语言变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInital) return;

            if (cboxLanguage.Text == "中文")
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-CN");
                SetLanguageValue("zh-CN");
            }
            else if (cboxLanguage.Text == "日本語")
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("ja-JP");
                SetLanguageValue("ja-JP");
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en");
                SetLanguageValue("en");
            }
            string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0012I", db.Language);
            if (MessageBox.Show(msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        /// <summary>
        /// 配置値设定
        /// </summary>
        private void SetLanguageValue(string language)
        {
            string msg = NCMessage.GetInstance(language).GetMessageById("CM0460I",language);
            NCLogger.GetInstance().WriteInfoLog(msg);
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            if (!xmlConfig.WriteValue("config", "language", language))
            {
                msg = string.Format(NCMessage.GetInstance(language).GetMessageById("CM0450E", language), language);
                NCLogger.GetInstance().WriteErrorLog(msg);
            }
            msg = NCMessage.GetInstance(language).GetMessageById("CM0470I", language);
            NCLogger.GetInstance().WriteInfoLog(msg);
        }

    }
}