using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace HPSWizard
{
	public partial class WizardPage : UserControl
	{
		#region Data members
		private WizardFormBase		m_parentWizardForm;
		private List<WizardPage>	m_nextPages				= new List<WizardPage>();
		private WizardPageType		m_pageType				= WizardPageType.Intermediate;
		private WizardStepType		m_stepType				= WizardStepType.None;
		private string				m_title					= "";
		private string				m_subtitle				= "";

		// added the following line to support the "<< Start" button - jms - 2/19/2009
		private WizardButtonState	m_buttonStateStart		= WizardButtonState.Enabled | WizardButtonState.Visible;

		private WizardButtonState	m_buttonStateBack		= WizardButtonState.Enabled | WizardButtonState.Visible;
		private WizardButtonState	m_buttonStateNext		= WizardButtonState.Enabled | WizardButtonState.Visible;
		private WizardButtonState	m_buttonStateCancel		= WizardButtonState.Enabled | WizardButtonState.Visible;
		private WizardButtonState	m_buttonStateHelp		= WizardButtonState.Enabled | WizardButtonState.Visible;

        /// <summary>
        /// used to save page data
        /// </summary>
        private Hashtable pageData = new Hashtable();
        #endregion Data members

		#region Events
		public event WizardPageActivateHandler WizardPageActivated;
		#endregion Events

		#region Properties
        /// <summary>
        /// PageData
        /// </summary>
        public Hashtable PageData
        {
            get { return pageData; }
            set { this.pageData = value; }
        }
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the title text displayed in the graphic panel
		/// </summary>
		public string Title
		{
			get { return m_title; }
			set { m_title = value;}
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the sub-title text displayed in the graphic panel
		/// </summary>
		public string Subtitle
		{
			get { return m_subtitle; }
			set { m_subtitle = value;}
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the title page type (start, intermediate, or stop)
		/// </summary>
		public WizardPageType WizardPageType
		{
			get { return m_pageType; }
			set { m_pageType = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the parent wizard form
		/// </summary>
		public WizardFormBase ParentWizardForm
		{
			get { return m_parentWizardForm; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		///  The button state (visible/enabled) for the Back button
		/// </summary>
		/// <remarks>Added to support the new "Start" button - jms - 2/19/20-09</remarks>
		public WizardButtonState ButtonStateStart
		{
			get { return m_buttonStateStart; }
			set { m_buttonStateStart = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		///  The button state (visible/enabled) for the Back button
		/// </summary>
		public WizardButtonState ButtonStateBack
		{
			get { return m_buttonStateBack; }
			set { m_buttonStateBack = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		///  The button state (visible/enabled) for the Next button
		/// </summary>
		public WizardButtonState ButtonStateNext
		{
			get { return m_buttonStateNext; }
			set { m_buttonStateNext = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		///  The button state (visible/enabled) for the Cancel button
		/// </summary>
		public WizardButtonState ButtonStateCancel
		{
			get { return m_buttonStateCancel; }
			set { m_buttonStateCancel = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		///  The button state (visible/enabled) for the Help button
		/// </summary>
		public WizardButtonState ButtonStateHelp
		{
			get { return m_buttonStateHelp; }
			set { m_buttonStateHelp = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get the list of "next" pages
		/// </summary>
		public List<WizardPage> NextPages
		{
			get { return m_nextPages; }
		}
		#endregion Properties


	
		//--------------------------------------------------------------------------------
		public WizardPage()
		{
			InitializeComponent();
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Default constructor - creates an Intermediate wizard page. To create a 
		/// start or stop page, use the overloaded constructor
		/// </summary>
		public WizardPage(WizardFormBase parent)
		{
			Init(parent, WizardPageType.Intermediate);
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Creates a wizard page of the specified type. You can optionally call the 
		/// other constructor if you're adding anintermediate page.
		/// </summary>
		/// <param name="parent">The parent wizard form</param>
		/// <param name="pageType">The type of page being created (see WizardPageType enum)</param>
		public WizardPage(WizardFormBase parent, WizardPageType pageType)
		{
			Init(parent, pageType);
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// This is a common initialization function called by all of the constructors.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="pageType"></param>
		private void Init(WizardFormBase parent, WizardPageType pageType)
		{
			InitializeComponent();

			//this.BorderStyle = BorderStyle.FixedSingle;

			m_parentWizardForm	= parent;
			this.Visible		= false;
			this.Dock			= DockStyle.Fill;
			m_pageType			= pageType;

			// if this is the start page, disable the Back and Start buttons
			if (WizardPageType == WizardPageType.Start)
			{
				// added the following line to support the "<< Start" button - jms 2/19/2009
				ButtonStateStart &= ~WizardButtonState.Enabled;

				ButtonStateBack &= ~WizardButtonState.Enabled;
			}

			m_parentWizardForm.PageCreated(this);
			m_parentWizardForm.WizardPageChangeEvent += new WizardPageChangeHandler(parentForm_WizardPageChange);
		}


		//--------------------------------------------------------------------------------
		/// <summary>
		/// Adds a "next page" item to the list of possible next pages. The derived 
		/// Wizard page can then decide on its own which page is next based on the 
		/// values of one/more controls in the derived page.
		/// </summary>
		/// <param name="nextPage">The page to add as a possible "next" page</param>
		public void AddNextPage(WizardPage nextPage)
		{
			m_nextPages.Add(nextPage);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Allows the derived Wizard form to raise the WizardPageActivated event.
		/// </summary>
		/// <param name="e"></param>
		protected void Raise_WizardPageActivated(WizardPageActivateArgs e)
		{
			WizardPageActivated(this, e);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Base method used to save data for all visited wizard pages. This copy of 
		/// the method always returns true.
		/// </summary>
		/// <returns>True if the data was succesfully saved</returns>
		public virtual bool SaveData()
		{
			return true;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get the next page to be shown. This is virtual so that you can override it 
		/// in order to provide a programmatically determined "next" page.
		/// </summary>
		/// <returns>The page that will be displayed next</returns>
		public virtual WizardPage GetNextPage()
		{
			// sanity check to make sure we have a page to return
			if (m_nextPages.Count == 0)
			{
				throw new WizardFormException("No pages have been specified as a \"next\" page.");
			}
			// return the first page in the list of "next" pages
			return m_nextPages[0];
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Allows the base class to handle a page change event. Right now, there's 
		/// nothing to do, but you could add some apppropriate functionalty that 
		/// suits your application.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void parentForm_WizardPageChange(object sender, WizardPageChangeArgs e)
		{
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when a page is made visible.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WizardPage_VisibleChanged(object sender, EventArgs e)
		{
			// The designer will crash the IDE if this code is executed *in the designer.  
			// To avoid this pain in the *ass* issue, we have to check to see if the 
			// designer is active before executing the code, and no - there is no built-in 
			// method in the UseControl class to provide this status, so I wrote a small 
			// function that can be called from within this class whenever necessary. The 
			// only problem is that ANY derived control (or form) may need this method.
			if (!WizardUtility.IsDesignTime())
			{
				if (this.Visible)
				{
					WizardPageActivated(this, new WizardPageActivateArgs(this, m_stepType));
				}
			}
		}

	}
}



