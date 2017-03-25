using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using HPSWizard;
using NC.HPS.Lib;

namespace HPSManagement.PublishMagzine
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
        /// <summary>
        /// 初期化
        /// </summary>
        private void init()
        {
            string MagId = PageData["MagId"].ToString();
            DataSet ds = new DataSet();
            if (db.GetPublish(0, 0, "发行编号,发行期号", "期刊编号=" + MagId, "", ref ds))
            {
                cmbMagzineNo.DataSource = ds.Tables[0];
                cmbMagzineNo.DisplayMember = "发行期号";
                ButtonStateNext |= WizardButtonState.Enabled;
            }
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
            if (cmbMagzineNo.SelectedIndex > -1)
            {
                DataTable dt = (DataTable)cmbMagzineNo.DataSource;
                NextPages[0].PageData["MagId"] = PageData["MagId"].ToString();
                NextPages[0].PageData["SMagId"] = PageData["SMagId"].ToString();
                NextPages[0].PageData["MagNo"] = dt.Rows[cmbMagzineNo.SelectedIndex]["发行期号"].ToString();
                NextPages[0].PageData["PublishId"] = dt.Rows[cmbMagzineNo.SelectedIndex]["发行编号"].ToString();

                return NextPages[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 期号选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMagzineNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMagzineNo.SelectedIndex != 0)
            {
                ButtonStateNext |= WizardButtonState.Enabled;
                ParentWizardForm.UpdateWizardForm(this);
            }
            else
            {
                ButtonStateNext = WizardButtonState.Visible;
            }

        }
        /// <summary>
        /// WizardPage2_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WizardPage2_Load(object sender, EventArgs e)
        {
            init();
        }

	}
}
