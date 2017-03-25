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
	public partial class WizardPage3 : HPSWizard.WizardPage
	{
        private CmWinServiceAPI db;
        //--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor that assumes the page type is "intermediate"
		/// </summary>
		/// <param name="parent">The parent WizardFormBase-derived form</param>
        public WizardPage3(WizardFormBase parent, CmWinServiceAPI db) 
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
        public WizardPage3(WizardFormBase parent, WizardPageType pageType, CmWinServiceAPI db) 
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
            if (isSelected())
            {
                if (chkFaceBook.Checked)
                {
                    NextPages[0].PageData["MagId"] = PageData["MagId"].ToString();
                    NextPages[0].PageData["SMagId"] = PageData["SMagId"].ToString();
                    NextPages[0].PageData["MagNo"] = PageData["MagNo"].ToString();
                    NextPages[0].PageData["MediaType"] = "FACEBOOK";
                    return NextPages[0];
                }
                else if (chkTwitter.Checked)
                {
                    NextPages[1].PageData["MagId"] = PageData["MagId"].ToString();
                    NextPages[1].PageData["SMagId"] = PageData["SMagId"].ToString();
                    NextPages[1].PageData["MagNo"] = PageData["MagNo"].ToString();
                    NextPages[1].PageData["MediaType"] = "TWITTER";
                    return NextPages[1];
                }
                else if (chkQQWeibo.Checked)
                {
                    NextPages[2].PageData["MagId"] = PageData["MagId"].ToString();
                    NextPages[2].PageData["SMagId"] = PageData["SMagId"].ToString();
                    NextPages[2].PageData["MagNo"] = PageData["MagNo"].ToString();
                    NextPages[2].PageData["MediaType"] = "TENCENT";
                    return NextPages[2];
                }
                else
                {
                    NextPages[3].PageData["MagId"] = PageData["MagId"].ToString();
                    NextPages[3].PageData["SMagId"] = PageData["SMagId"].ToString();
                    NextPages[3].PageData["MagNo"] = PageData["MagNo"].ToString();
                    NextPages[3].PageData["MediaType"] = "WORDPRESS";
                    return NextPages[3];
                }
            }
            else
            {
                return null;
            }
		}
        /// <summary>
        /// 是否有选择
        /// </summary>
        /// <returns></returns>
        private bool isSelected()
        {
            return chkFaceBook.Checked || chkTwitter.Checked || chkQQWeibo.Checked || chkWordpress.Checked;
        }
        /// <summary>
        /// 选择变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkFaceBook_CheckedChanged(object sender, EventArgs e)
        {
            if (isSelected())
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
