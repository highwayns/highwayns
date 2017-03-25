using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using HPSWizard;
using NC.HPS.Lib;

namespace HPSManagement.CreateMagzine
{
	public partial class WizardPage2 : HPSWizard.WizardPage
	{
        private CmWinServiceAPI db;
        //--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor that assumes the page type is "intermediate"
		/// </summary>
		/// <param name="parent">The parent WizardFormBase-derived form</param>
        public WizardPage2(WizardFormBase parent, CmWinServiceAPI db) 
					: base(parent)
		{
            this.db = db;
			InitPage();
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor that allows the programmer to specify the page type. In 
		/// the case of the sample app, we use this constructor for this oject, 
		/// and specify it as the start page.
		/// </summary>
		/// <param name="parent">The parent WizardFormBase-derived form</param>
		/// <param name="pageType">The type of page this object represents (start, intermediate, or stop)</param>
        public WizardPage2(WizardFormBase parent, WizardPageType pageType, CmWinServiceAPI db) 
					: base(parent, pageType)
		{
            this.db = db;
			InitPage();
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// This method serves as a common constructor initialization location, 
		/// and serves mainly to set the desired size of the container panel in 
		/// the wizard form (see WizardFormBase for more info).  I didn't want 
		/// to do this here but it was the only way I could get the form to 
		/// resize itself appropriately - it needed to size itself according 
		/// to the size of the largest wizard page.
		/// </summary>
		public void InitPage()
		{
			InitializeComponent();
			base.Size = this.Size;
			this.ParentWizardForm.DiscoverPagePanelSize(this.Size);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Overriden method that allows this wizard page to save page-specific data.
		/// </summary>
		/// <returns>True if the data was saved successfully</returns>
		public override bool SaveData()
		{
			return true;
		}
        //--------------------------------------------------------------------------------
        /// <summary>
        /// This is an overriden method that performs special processing when 
        /// it's time to select the next page to display. In the case of our 
        /// Page 1, a different "next" page is displayed depending on which 
        /// radio button has been selected.
        /// </summary>
        /// <returns>The new current page that we will show</returns>
        public override WizardPage GetNextPage()
        {
            if (string.IsNullOrEmpty(txtMagzineNo.Text))
            {
                MessageBox.Show("请输入期刊号！");
                return null;
            }
            if (!checkMagNo())
            {
                MessageBox.Show("本期期刊已经存在！");
                return null;
            }
            else
            {
                NextPages[0].PageData["MagId"] = PageData["MagId"].ToString();
                NextPages[0].PageData["SMagId"] = PageData["SMagId"].ToString();
                NextPages[0].PageData["MagNo"] = txtMagzineNo.Text;
            }
            return NextPages[0];
        }

        /// <summary>
        /// 期刊存在检查
        /// </summary>
        /// <returns></returns>
        private Boolean checkMagNo()
        {
            string MagId =PageData["MagId"].ToString();
            DataSet ds = new DataSet();
            if (db.GetPublish(0, 0, "count(*)", "期刊编号=" + MagId + " AND 发行期号='"+txtMagzineNo.Text+"'", "", ref ds))
            {
                int count = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                if (count > 0) return false;
            }
            return true;
        }
        /// <summary>
        /// 期号输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMagzineNo_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMagzineNo.Text))
            {
                ButtonStateNext |= WizardButtonState.Enabled;
                ParentWizardForm.UpdateWizardForm(this);
            }
            else
            {
                ButtonStateNext = WizardButtonState.Visible;
            }

        }

	}
}
