using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;
using HPSWizard;
using HPSManagement.PublishMagzine;
//using Com.Seezt.Skins;

namespace HPSManagement
{
    public partial class FormPublishMagzineWizard : WizardFormBase
    {
        private CmWinServiceAPI db;
        private WizardPage1 page1 = null;
        private WizardPage2 page2 = null;
        private WizardPage3 page3 = null;
        private WizardPage40 page40 = null;
        private WizardPage41 page41 = null;
        private WizardPage42 page42 = null;
        private WizardPage43 page43 = null;
        private WizardPage5 page5 = null;
        public FormPublishMagzineWizard()
        {
            InitializeComponent();
        }
        public FormPublishMagzineWizard(CmWinServiceAPI db)
            : base(db)
        {
            this.db = db;
            InitializeComponent();
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Fired when the form is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WizardExample_Load(object sender, EventArgs e)
        {
            // set our image panel metrics
            this.GraphicPanelImagePosition = WizardImagePosition.Right;
            this.GraphicPanelImageResource = "HPSManagement.Resources.udplogo";
            this.GraphicPanelGradientColor = Color.DarkSlateBlue;

            // if you don't need a given button, you can hide it here
            //this.ButtonHelpHide = true;
            //this.ButtonStartHide = true;

            // add handlers for the buttons
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);

            // create the wizard pages we need
            page1 = new WizardPage1(this, WizardPageType.Start,db);
            page2 = new WizardPage2(this,db);
            page3 = new WizardPage3(this,db);
            page40 = new WizardPage40(this, db);
            page41 = new WizardPage41(this, db);
            page42 = new WizardPage42(this, db);
            page43 = new WizardPage43(this, db);
            page5 = new WizardPage5(this, WizardPageType.Stop, db);

            // add a handler that lets us know when a page has been activated
            page1.WizardPageActivated += new WizardPageActivateHandler(WizardPageActivated);
            page2.WizardPageActivated += new WizardPageActivateHandler(WizardPageActivated);
            page3.WizardPageActivated += new WizardPageActivateHandler(WizardPageActivated);
            page40.WizardPageActivated += new WizardPageActivateHandler(WizardPageActivated);
            page41.WizardPageActivated += new WizardPageActivateHandler(WizardPageActivated);
            page42.WizardPageActivated += new WizardPageActivateHandler(WizardPageActivated);
            page43.WizardPageActivated += new WizardPageActivateHandler(WizardPageActivated);
            page5.WizardPageActivated += new WizardPageActivateHandler(WizardPageActivated);

            // make sure all of the necessary pages have a "next" page
            page1.AddNextPage(page2);
            page2.AddNextPage(page3);
            page3.AddNextPage(page40);
            page3.AddNextPage(page41);
            page3.AddNextPage(page42);
            page3.AddNextPage(page43);
            page40.AddNextPage(page5);
            page41.AddNextPage(page5);
            page42.AddNextPage(page5);
            page43.AddNextPage(page5);

            // start the wizard
            StartWizard();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Fired when a wizard page is activated (made visible)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WizardPageActivated(object sender, WizardPageActivateArgs e)
        {
            PaintTitle();
            this.buttonBack.Enabled = (e.ActivatedPage.WizardPageType != WizardPageType.Start);
            if (e.ActivatedPage.WizardPageType == WizardPageType.Stop)
            {
                if (db.Language == "zh-CN")
                {
                    this.buttonNext.Text = "完成";
                }
                else if (db.Language == "ja-JP")
                {
                    this.buttonNext.Text = "完了";
                }
                else
                {
                    this.buttonNext.Text = "Finished";
                }
            }
            else
            {
                if (db.Language == "zh-CN")
                {
                    this.buttonNext.Text = "下一步";
                }
                else if (db.Language == "ja-JP")
                {
                    this.buttonNext.Text = "次へ";
                }
                else
                {
                    this.buttonNext.Text = "Next >";
                }
            }
        }
        
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Fired when the back button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBack_Click(object sender, EventArgs e)
        {
            // tell the page chain to go to the previous page
            WizardPage currentPage = PageChain.GoBack();
            // raise the page change event (this currently does nothing but lets the 
            // base class know when the active page has changed
            Raise_WizardPageChangeEvent(new WizardPageChangeArgs(currentPage, WizardStepType.Previous));
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Fired when the Next button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNext_Click(object sender, EventArgs e)
        {
            // if the current page (before changing) is the last page in the wizard, 
            // take steps to close the wizard
            if (PageChain.GetCurrentPage().WizardPageType == WizardPageType.Stop)
            {
                // call the central SaveData method (which calls the SaveData 
                // method in each page in the chain
                if (PageChain.SaveData() == null)
                {
                    // and if everything is okay, close the wizard form
                    this.Close();
                }
            }
            // otherwise, move to the next page in the chain, and let the base class know
            else if(PageChain!=null)
            {
                WizardPage nextpage = PageChain.GetCurrentPage().GetNextPage();
                if (nextpage != null)
                {
                    WizardPage currentPage = PageChain.GoNext(nextpage);
                    Raise_WizardPageChangeEvent(new WizardPageChangeArgs(currentPage, WizardStepType.Next));
                    
                }
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Fired when the user clicks the Cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Fired when the user clicks the Help button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet.");
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Fired when the user clicks the Start button (to return to the first wizard 
        /// page).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStart_Click(object sender, EventArgs e)
        {
            WizardPage currentPage = PageChain.GoFirst();
            // raise the page change event - this currently does nothing but lets the 
            // base class know when the active page has changed
            Raise_WizardPageChangeEvent(new WizardPageChangeArgs(currentPage, WizardStepType.Previous));
        }
    }
}
