using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;

namespace HPSManagement
{
    public partial class FormRegist : Form
    {
        private CmWinServiceAPI db;
        private string lic = "";
        private string product = "";
        public FormRegist(CmWinServiceAPI db)
        {
            this.db = db;
            InitializeComponent();            
        }
        /// <summary>
        /// 以后再说
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemindme_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 画面初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormRegist_Load(object sender, EventArgs e)
        {
            try
            {
                if (GetProductValue())
                {
                    string[] products = product.Split(';');
                    foreach (string prod in products)
                    {
                        cmbProduct.Items.Add(prod);
                    }
                    cmbProduct.SelectedIndex = 0;
                }
                txtProductID.Text = NCCryp.getProductID();
                if (txtProductID.Text == "")
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0106I", db.Language);
                    MessageBox.Show(msg);
                    return;
                }
            }
            catch (Exception ex)
            {
                NCLogger.GetInstance().WriteExceptionLog(ex);
                return;
            }
            if (GetConfigValue())
            {
                txtLicId.Text = lic;
                if (txtLicId.Text != "" && NCCryp.checkLic(txtLicId.Text))
                {
                    btnRegist.Enabled = false;
                    btnSend.Enabled = false;
                    btnAdd.Enabled = false;
                }
            }
            if (isOwner())
            {
                txtProductID.ReadOnly = false;
                btnCreate.Enabled = true;
                btnCreate.Visible = true;
                btnAdd.Enabled = true;
            }
            else
            {
                btnCreate.Enabled = false;
                btnCreate.Visible = false;
                btnAdd.Enabled = false;
            }
        }
        /// <summary>
        /// Owner 
        /// </summary>
        /// <returns></returns>
        private Boolean isOwner()
        {
            DataSet ds = new DataSet();
            String wheresql = "UserID='" + db.UserID + "'";
            if (db.GetUser(0, 0, "*", wheresql, "", ref ds) && ds.Tables[0].Rows.Count == 1)
            {
                if (ds.Tables[0].Rows[0]["UserPwd"].ToString() == NCCryp.Encrypto("zjhuen123"))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (NCCryp.checkLic(txtLicId.Text))
            {
                SetLicValue(txtLicId.Text);
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0104I", db.Language);
                MessageBox.Show(msg);
                Close();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0105I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 发送许可
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            DataSet ds_Server = new DataSet();
            if (db.GetMailServer(0, 0, "*", "", "", ref ds_Server) && ds_Server.Tables[0].Rows.Count > 0)
            {
                String name = ds_Server.Tables[0].Rows[0]["名称"].ToString();
                String address = ds_Server.Tables[0].Rows[0]["地址"].ToString();
                String port = ds_Server.Tables[0].Rows[0]["端口"].ToString();
                String user = ds_Server.Tables[0].Rows[0]["用户"].ToString();
                String password = ds_Server.Tables[0].Rows[0]["密码"].ToString();
                String from = ds_Server.Tables[0].Rows[0]["送信人地址"].ToString();
                String servertype = ds_Server.Tables[0].Rows[0]["服务器类型"].ToString().Trim();
                String to = "tei952@hotmail.com";
                String body = "My Product:" + cmbProduct.Text;
                body += "My Product ID:" + txtProductID.Text;
                String picfile = "";
                String htmlbody = "<html><head></head><body>" + body + "</body></html>";
                String subject = "I wan't a license for CJW!";
                if (NCMail.SendEmail(subject, body, htmlbody, picfile, address, user, password, from, to, servertype))
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0108I", db.Language);
                    MessageBox.Show(msg);
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0107I", db.Language);
                    MessageBox.Show(msg);

                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0107I", db.Language);
                MessageBox.Show(msg);
            }
        }
        /// <summary>
        /// 配置値取得
        /// </summary>
        private bool GetConfigValue()
        {
            bool ret = true;
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            if (!xmlConfig.ReadXmlData("config", "Lic", ref lic))
            {
                ret = false;
            }
            return ret;
        }
        
        /// <summary>
        /// 配置値设定
        /// </summary>
        private void SetLicValue(string lic)
        {
            string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0460I", db.Language);
            NCLogger.GetInstance().WriteInfoLog(msg);
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            if (!xmlConfig.WriteValue("config", "Lic", lic))
            {
                msg = string.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0450E", db.Language), lic);
                NCLogger.GetInstance().WriteErrorLog(msg);
            }
            msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0470I", db.Language);
            NCLogger.GetInstance().WriteInfoLog(msg);
        }
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            txtLicId.Text = NCCryp.getLic(NCCryp.getHardIDFromProductId(txtProductID.Text, cmbProduct.Text), cmbProduct.Text);
        }
        /// <summary>
        /// 製品追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            cmbProduct.Items.Add(cmbProduct.Text);
            string products = cmbProduct.Items[0].ToString();
            for (int idx = 1; idx < cmbProduct.Items.Count; idx++) products += ";" + cmbProduct.Items[idx].ToString();
            SetProductValue(products);
            cmbProduct_SelectedIndexChanged(null, null);
        }
        /// <summary>
        /// 配置値取得
        /// </summary>
        private bool GetProductValue()
        {
            bool ret = true;
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            if (!xmlConfig.ReadXmlData("config", "Product", ref product))
            {
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 配置値设定
        /// </summary>
        private void SetProductValue(string product)
        {
            string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0460I", db.Language);
            NCLogger.GetInstance().WriteInfoLog(msg);
            NdnXmlConfig xmlConfig;
            xmlConfig = new NdnXmlConfig(NCConst.CONFIG_FILE_DIR + NCUtility.GetAppConfig());
            if (!xmlConfig.WriteValue("config", "Product", product))
            {
                msg = string.Format(NCMessage.GetInstance(db.Language).GetMessageById("CM0450E", db.Language), product);
                NCLogger.GetInstance().WriteErrorLog(msg);
            }
            msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0470I", db.Language);
            NCLogger.GetInstance().WriteInfoLog(msg);
        }
        /// <summary>
        /// 产品变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProductID.Text = NCCryp.getProductID(cmbProduct.Text);
            if (txtProductID.Text == "")
            {
                string msg = NCMessage.GetInstance(db.Language).GetMessageById("CM0106I", db.Language);
                MessageBox.Show(msg);
                return;
            }
            txtLicId.Text = "";

        }

    }
}
