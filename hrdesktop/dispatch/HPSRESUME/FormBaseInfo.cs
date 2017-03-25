using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;

namespace HPSRESUME
{
    public partial class FormBaseInfo : Form
    {
        private DB db;
        public FormBaseInfo(DB db)
        {
            this.db = db;
            InitializeComponent();
        }
        /// <summary>
        /// 履歴書管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormBaseInfo_Load(object sender, EventArgs e)
        {
            init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void init()
        {
            DataSet ds = new DataSet();
            string fieldlist = "";
	        fieldlist+="[EmpId],";
	        fieldlist+="[Name],";
	        fieldlist+="[Country],";
	        fieldlist+="[NameKana],";
	        fieldlist+="[NamePinyin],";
	        fieldlist+="[Sex],";
	        fieldlist+="[Marriaged],";
	        fieldlist+="[Birthday],";
	        fieldlist+="[BirthAddress],";
	        fieldlist+="[LocalAddress],";
	        fieldlist+="[CurrentAddress],";
	        fieldlist+="[Occupation],";
	        fieldlist+="[TEL],";
	        fieldlist+="[PassportNo],";
	        fieldlist+="[ExpireDate],";
	        fieldlist+="[BizaNo],";
	        fieldlist+="[BizaType],";
	        fieldlist+="[BizaRange],";
	        fieldlist+="[BizaExpireDate],";
	        fieldlist+="[ExpBizaType],";
	        fieldlist+="[ExpBizaRange],";
	        fieldlist+="[ExpReason],";
	        fieldlist+="[EnterDay],";
	        fieldlist+="[NearStation]";
            if (db.GetEmp(0, 0, fieldlist, "", "", ref ds))
            {
                dataGridView1.DataSource = ds.Tables[0];
            }

        }

        /// <summary>
        /// 履歴書追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = 0;
            string fieldlist = "";
            fieldlist += "[Name],";
            fieldlist += "[Country],";
            fieldlist += "[NameKana],";
            fieldlist += "[NamePinyin],";
            fieldlist += "[Sex],";
            fieldlist += "[Marriaged],";
            fieldlist += "[Birthday],";
            fieldlist += "[BirthAddress],";
            fieldlist += "[LocalAddress],";
            fieldlist += "[CurrentAddress],";
            fieldlist += "[Occupation],";
            fieldlist += "[TEL],";
            fieldlist += "[PassportNo],";
            fieldlist += "[ExpireDate],";
            fieldlist += "[BizaNo],";
            fieldlist += "[BizaType],";
            fieldlist += "[BizaRange],";
            fieldlist += "[BizaExpireDate],";
            fieldlist += "[ExpBizaType],";
            fieldlist += "[ExpBizaRange],";
            fieldlist += "[ExpReason],";
            fieldlist += "[EnterDay],";
            fieldlist += "[NearStation],";
            fieldlist += "[Skill],";
            fieldlist += "[Skill1],";
            fieldlist += "[Skill2],";
            fieldlist += "[Skill3],";
            fieldlist += "[SkillManager],";
            fieldlist += "[SkillRange],";
            fieldlist += "[SkillJapanese],";
            fieldlist += "[SkillEnglish],";
            fieldlist += "[Comment1],";
            fieldlist += "[Comment2],";
            fieldlist += "[SelfComment1],";
            fieldlist += "[SelfComment2],";
            fieldlist += "[SelfComment3],";
            fieldlist += "[UserId]";
            String valuelist = "'"
                + txtName.Text + "','"
                + txtCountry.Text + "','" 
                + txtNameKana.Text + "','" 
                + txtNamePinyin.Text + "','" 
                + txtSex.Text + "','" 
                + txtMarriaged.Text + "','" 
                + txtBirthday.Text + "','"
                + txtBirthAddress.Text + "','"
                + txtLocalAddress.Text + "','"
                + txtCurrentAddress.Text + "','"
                + txtOccupation.Text + "','"
                + txtTEL.Text + "','"
                + txtPassportNo.Text + "','"
                + txtExpireDate.Text + "','"
                + txtBizaNo.Text + "','"
                + txtBizaType.Text + "','"
                + txtBizaRange.Text + "','"
                + txtBizaExpireDate.Text + "','"
                + txtExpBizaType.Text + "','"
                + txtExpBizaRange.Text + "','"
                + txtExpReason.Text + "','"
                + txtEnterDay.Text + "','"
                + txtNearStation.Text + "','"
                + txtSkill.Text + "','"
                + txtSkill1.Text + "','"
                + txtSkill2.Text + "','"
                + txtSkill3.Text + "','"
                + txtSkillManager.Text + "','"
                + txtSkillRange.Text + "','"
                + txtSkillJapanese.Text + "','"
                + txtSkillEnglish.Text + "','"
                + txtComment1.Text + "','"
                + txtComment2.Text + "','"
                + txtSelfComment1.Text + "','"
                + txtSelfComment2.Text + "','"
                + txtSelfComment3.Text + "','"
                + db.db.UserID + "'" ;
            if (db.SetEmp(0, 0, fieldlist,
                                 "", valuelist, out id))
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0170I", db.db.Language);
                MessageBox.Show(msg);
                init();
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0171I", db.db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 行选择变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0
                && dataGridView1.SelectedRows[0].Cells[0].Value!=null)
            {
                txtEmpId.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtCountry.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtNameKana.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtNamePinyin.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtSex.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                txtMarriaged.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                txtBirthday.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString().Trim();
                txtBirthAddress.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString().Trim();
                txtLocalAddress.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString().Trim();
                txtCurrentAddress.Text = dataGridView1.SelectedRows[0].Cells[10].Value.ToString().Trim();
                txtOccupation.Text = dataGridView1.SelectedRows[0].Cells[11].Value.ToString().Trim();
                txtTEL.Text = dataGridView1.SelectedRows[0].Cells[12].Value.ToString().Trim();
                txtPassportNo.Text = dataGridView1.SelectedRows[0].Cells[13].Value.ToString().Trim();
                txtExpireDate.Text = dataGridView1.SelectedRows[0].Cells[14].Value.ToString().Trim();
                txtBizaNo.Text = dataGridView1.SelectedRows[0].Cells[15].Value.ToString().Trim();
                txtBizaType.Text = dataGridView1.SelectedRows[0].Cells[16].Value.ToString().Trim();
                txtBizaRange.Text = dataGridView1.SelectedRows[0].Cells[17].Value.ToString().Trim();
                txtBizaExpireDate.Text = dataGridView1.SelectedRows[0].Cells[18].Value.ToString().Trim();
                txtExpBizaType.Text = dataGridView1.SelectedRows[0].Cells[19].Value.ToString().Trim();
                txtExpBizaRange.Text = dataGridView1.SelectedRows[0].Cells[20].Value.ToString().Trim();
                txtExpReason.Text = dataGridView1.SelectedRows[0].Cells[21].Value.ToString().Trim();
                txtEnterDay.Text = dataGridView1.SelectedRows[0].Cells[22].Value.ToString().Trim();
                txtNearStation.Text = dataGridView1.SelectedRows[0].Cells[23].Value.ToString().Trim();
                DataSet ds = new DataSet();
                String wheresql = "EmpId=" + txtEmpId.Text;
                string fieldlist = "";
	            fieldlist += "[Skill],";
	            fieldlist += "[Skill1],";
	            fieldlist += "[Skill2],";
	            fieldlist += "[Skill3],";
	            fieldlist += "[SkillManager],";
	            fieldlist += "[SkillRange],";
	            fieldlist += "[SkillJapanese],";
	            fieldlist += "[SkillEnglish],";
	            fieldlist += "[Comment1],";
	            fieldlist += "[Comment2],";
	            fieldlist += "[SelfComment1],";
	            fieldlist += "[SelfComment2],";
                fieldlist += "[SelfComment3],";
                if (db.GetEmp(0, 0, fieldlist, wheresql, "", ref ds) && ds.Tables[0].Rows.Count==1)
                {
                    txtSkill.Text = ds.Tables[0].Rows[0][0].ToString().Trim();
                    txtSkill1.Text = ds.Tables[0].Rows[0][1].ToString().Trim();
                    txtSkill2.Text = ds.Tables[0].Rows[0][2].ToString().Trim();
                    txtSkill3.Text = ds.Tables[0].Rows[0][3].ToString().Trim();
                    txtSkillManager.Text = ds.Tables[0].Rows[0][4].ToString().Trim();
                    txtSkillRange.Text = ds.Tables[0].Rows[0][5].ToString().Trim();
                    txtSkillJapanese.Text = ds.Tables[0].Rows[0][6].ToString().Trim();
                    txtSkillEnglish.Text = ds.Tables[0].Rows[0][7].ToString().Trim();
                    txtComment1.Text = ds.Tables[0].Rows[0][8].ToString().Trim();
                    txtComment2.Text = ds.Tables[0].Rows[0][9].ToString().Trim();
                    txtSelfComment1.Text = ds.Tables[0].Rows[0][10].ToString().Trim();
                    txtSelfComment2.Text = ds.Tables[0].Rows[0][11].ToString().Trim();
                    txtSelfComment3.Text = ds.Tables[0].Rows[0][12].ToString().Trim();
                }
            }

        }
        /// <summary>
        /// 更新简历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "EmpId=" + txtEmpId.Text;
                String valuesql = "Name='" + txtName.Text
                    + "',Country='" + txtCountry.Text
                    + "',NameKana='" + txtNameKana.Text
                    + "',NamePinyin='" + txtNamePinyin.Text
                    + "',Sex='" + txtSex.Text
                    + "',Marriaged='" + txtMarriaged.Text
                    + "',Birthday='" + txtBirthday.Text
                    + "',BirthAddress='" + txtBirthAddress.Text
                    + "',LocalAddress='" + txtLocalAddress.Text
                    + "',CurrentAddress='" + txtCurrentAddress.Text
                    + "',Occupation='" + txtOccupation.Text
                    + "',TEL='" + txtTEL.Text
                    + "',PassportNo='" + txtPassportNo.Text
                    + "',ExpireDate='" + txtExpireDate.Text
                    + "',BizaNo='" + txtBizaNo.Text
                    + "',BizaType='" + txtBizaType.Text
                    + "',BizaRange='" + txtBizaRange.Text
                    + "',BizaExpireDate='" + txtBizaExpireDate.Text
                    + "',ExpBizaType='" + txtExpBizaType.Text
                    + "',ExpBizaRange='" + txtExpBizaRange.Text
                    + "',ExpReason='" + txtExpReason.Text
                    + "',EnterDay='" + txtEnterDay.Text
                    + "',NearStation='" + txtNearStation.Text
                    + "',Skill='" + txtSkill.Text
                    + "',Skill1='" + txtSkill1.Text
                    + "',Skill2='" + txtSkill2.Text
                    + "',Skill3='" + txtSkill3.Text
                    + "',SkillManager='" + txtSkillManager.Text
                    + "',SkillRange='" + txtSkillRange.Text
                    + "',SkillJapanese='" + txtSkillJapanese.Text
                    + "',SkillEnglish='" + txtSkillEnglish.Text
                    + "',Comment1='" + txtComment1.Text
                    + "',Comment2='" + txtComment2.Text
                    + "',SelfComment1='" + txtSelfComment1.Text
                    + "',SelfComment2='" + txtSelfComment2.Text
                    + "',SelfComment3='" + txtSelfComment3.Text
                    + "'"
                    ;
                if (db.SetEmp(0, 1, "", wheresql, valuesql, out id) && id == 1)
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0172I", db.db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0173I", db.db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0169I", db.db.Language);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = 0;
                String wheresql = "EmpId=" + txtEmpId.Text;
                if (db.SetEmp(0, 2, "", wheresql, "", out id) && id == 2)
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0174I", db.db.Language);
                    MessageBox.Show(msg);
                    init();
                }
                else
                {
                    string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0175I", db.db.Language);
                    MessageBox.Show(msg);
                }
            }
            else
            {
                string msg = NCMessage.GetInstance(db.db.Language).GetMessageById("CM0169I", db.db.Language);
                MessageBox.Show(msg);
            }

        }
    }
}
