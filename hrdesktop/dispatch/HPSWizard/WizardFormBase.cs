using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using NC.HPS.Lib;
//using Com.Seezt.Skins;

[assembly:CLSCompliant(true)]

namespace HPSWizard
{
    public partial class WizardFormBase : Form
	{
		#region Data members
		// a management object for maintaining the list of visited pages
		private WizardPageChain		m_pageChain;
		private WizardPage			m_startPage;
		private WizardPage			m_stopPage;
		private Color				m_graphicPanelBackgroundColor		= Color.White;
		private Color				m_graphicPanelGradientColor			= Color.White;
		private string				m_graphicPanelImageResource			= "";
		private WizardImagePosition	m_graphicPanelImagePosition			= WizardImagePosition.Right;
		private bool				m_graphicPanelImageIsTransparent	= false;
		private Font				m_graphicPanelTitleFont				= new Font("Arial", 9.25f, FontStyle.Bold);
		private Font				m_graphicPanelSubtitleFont			= new Font("Arial", 8.25f, FontStyle.Italic);
		private Color				m_graphicPanelTitleColor			= Color.White;
		private Color				m_graphicPanelSubtitleColor			= Color.White;
		private int					m_pageCount							= 0;
		private Size				m_desiredPagePanelSize				= new Size(0,0);
		private bool				m_buttonStartHide					= false;
		private bool				m_buttonBackHide					= false;
		private bool				m_buttonNextHide					= false;
		private bool				m_buttonCancelHide					= false;
		private bool				m_buttonHelpHide					= false;
		#endregion Data members

		#region Events
		public event WizardPageChangeHandler WizardPageChangeEvent;
		public event WizardPageCreatedHandler WizardPageCreatedEvent;
		public event WizardFormStartedHandler WizardFormStartedEvent;
		#endregion Events

		#region Properties
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the graphic panel background color
		/// </summary>
		public Color GraphicPanelBackgroundColor
		{
			get { return m_graphicPanelBackgroundColor; }
			set { m_graphicPanelBackgroundColor = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the graphic panel gradient color
		/// </summary>
		public Color GraphicPanelGradientColor
		{
			get { return m_graphicPanelGradientColor; }
			set { m_graphicPanelGradientColor = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the image used on the graphic panel
		/// </summary>
		public string GraphicPanelImageResource
		{
			get { return m_graphicPanelImageResource; }
			set { m_graphicPanelImageResource = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Set the position of the image. If middle or center, title and subtitle text will be ignored.
		/// </summary>
		public WizardImagePosition GraphicPanelImagePosition
		{
			get { return m_graphicPanelImagePosition; }
			set { m_graphicPanelImagePosition = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Image has a transparent background - affects the way the gradient is painted
		/// </summary>
		public bool GraphicPanelImageIsTransparent
		{
			get { return m_graphicPanelImageIsTransparent; }
			set {m_graphicPanelImageIsTransparent = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the font used for the title string in the graphic panel
		/// </summary>
		public Font GraphicPanelTitleFont
		{
			get { return m_graphicPanelTitleFont; }
			set { m_graphicPanelTitleFont = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the font used for the subtitle string in the graphic panel
		/// </summary>
		public Font GraphicPanelSubtitleFont
		{
			get { return m_graphicPanelSubtitleFont; }
			set { m_graphicPanelSubtitleFont = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set title color
		/// </summary>
		public Color GraphicPanelTitleColor
		{
			get { return m_graphicPanelTitleColor; }
			set { m_graphicPanelTitleColor = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set subtitle color
		/// </summary>
		public Color GraphicPanelSubtitleColor
		{
			get { return m_graphicPanelSubtitleColor; }
			set { m_graphicPanelSubtitleColor = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get the start page for this wizard form
		/// </summary>
		public WizardPage StartPage
		{
			get { return m_startPage; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get set stop page for this wizard form
		/// </summary>
		public WizardPage StopPage
		{
			get { return m_stopPage; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get the number of pages that have been added to this wizard form
		/// </summary>
		public int PageCount
		{
			get { return m_pageCount; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the value that represents the show/hide state of the Back button
		/// </summary>
		public bool ButtonStartHide
		{
			get { return m_buttonStartHide; }
			set { m_buttonStartHide = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the value that represents the show/hide state of the Back button
		/// </summary>
		public bool ButtonBackHide
		{
			get { return m_buttonBackHide; }
			set { m_buttonBackHide = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the value that represents the show/hide state of the Next button
		/// </summary>
		public bool ButtonNextHide
		{
			get { return m_buttonNextHide; }
			set { m_buttonNextHide = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the value that represents the show/hide state of the Cancel button
		/// </summary>
		public bool ButtonCancelHide
		{
			get { return m_buttonCancelHide; }
			set { m_buttonCancelHide = value; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get/set the value that represents the show/hide state of the Help button
		/// </summary>
		public bool ButtonHelpHide
		{
			get { return m_buttonHelpHide; }
			set { m_buttonHelpHide = value; }
		}
		//--------------------------------------------------------------------------------
		public WizardPageChain PageChain
		{
			get { return m_pageChain; }
		}
		//--------------------------------------------------------------------------------
		public Size PagePanelSize
		{
			get { return m_desiredPagePanelSize; }
		}
		#endregion Properties

		#region Constructors
        private CmWinServiceAPI db;
        public WizardFormBase()
        {
            InitializeComponent();
            // initialize the page chain
            m_pageChain = new WizardPageChain(this);
            // initialize the desired panel size to the current size of the panel
            this.m_desiredPagePanelSize = this.pagePanel.Size;
        }

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor
		/// </summary>
        public WizardFormBase(CmWinServiceAPI db)
		{
            this.db = db;
			InitializeComponent();
			// initialize the page chain
			m_pageChain = new WizardPageChain(this);
			// initialize the desired panel size to the current size of the panel
			this.m_desiredPagePanelSize = this.pagePanel.Size;
		}
		#endregion Constructors

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				this.m_graphicPanelSubtitleFont.Dispose();
				this.m_graphicPanelTitleFont.Dispose();
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Event handler methods
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fires the WizardPageCreatedEvent event.
		/// </summary>
		/// <param name="e"></param>
		public void Raise_WizardPageChangeEvent(WizardPageChangeArgs e)
		{
			UpdateWizardForm(e.ActivatedPage);
			// tell the wizard form we have a new active page
			WizardPageChangeEvent(this, e);
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fires the WizardFormStartedEvent event.
		/// </summary>
		/// <param name="e"></param>
		public void Raise_WizardFormStartedEvent(WizardFormStartedArgs e)
		{
			WizardFormStartedEvent(this, e);
		}

		#endregion Event handler methods
	
		#region Graphic panel painting
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Fired when the graphic panel is painted 0- this should only happen the 
		/// first time the wizard form is shown.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void graphicPanelTop_Paint(object sender, PaintEventArgs e)
		{
			// define all of our structures
			// to ease typing, let's graph the graphics object from the arguments
			Graphics	g					= e.Graphics;
			// our graphic panel image - set to null because we don't know yet if we even 
			// need it.
			Bitmap		image				= null;
			// The rectangle of the image - initialize to "nothing" because we don't have 
			// the image yet
			Rectangle	imageRect			= new Rectangle(0, 0, 0, 0);
			// the rectangle of the container panel itself
			Rectangle	panelRect			= new Rectangle(0, 0, this.graphicPanelTop.Width, this.graphicPanelTop.Height);
			// the rectange to be used for the gradient - by default, it's the same as 
			// the panel
			Rectangle	gradientRect		= new Rectangle(0, 0, panelRect.Width, panelRect.Height);
			// the brush used to paint the gradient - set to null because we don't know 
			// yet if we need it
			Brush		gradientBrush		= null;
			// the gradient direction 0 = left-to-right, 180=right-to-left
			int			gradientDirection	= 0;
			// shorter way to know if we need to paint the gradient if the two colors 
			// don't match, this variable will be true.
			bool		needGradient		= (this.GraphicPanelGradientColor != this.GraphicPanelBackgroundColor);

			// Sanity check for the image position - since this panel is at the top of 
			// the form, we automatically adjust the setting to it's closest equivalent 
			// value.
			switch (this.GraphicPanelImagePosition)
			{
				case WizardImagePosition.Top :
					this.GraphicPanelImagePosition = WizardImagePosition.Right;
					break;
				case WizardImagePosition.Bottom :
					this.GraphicPanelImagePosition = WizardImagePosition.Center;
					break;
				case WizardImagePosition.Middle :
					this.GraphicPanelImagePosition = WizardImagePosition.Left;
					break;
			}

            //try
            //{
            //    // retrieve the image if necessary, resize it if necessary, and postion it in the panel
            //    if (this.GraphicPanelImageResource.Length > 0)
            //    {
            //        // since this code is in a DLL, and since the bitmap is located in the 
            //        // exe's resources, we need to get the *entry* assembly in orer to load 
            //        // the appropriate resource stream.
            //        Assembly assembly = Assembly.GetEntryAssembly();
            //        // if the GraphicPanelImageResource string is incorrect, an exception 
            //        // will be thrown at the next line of code (saying the stream is null)
            //        Stream stream = assembly.GetManifestResourceStream(this.GraphicPanelImageResource);
            //        // create the bitmap from the stream
            //        image = new Bitmap(Bitmap.FromStream(stream));
            //        // establish the image rectangle size
            //        imageRect.Size = new Size(image.Width, image.Height);

            //        // if the image isn't at least as tall as the panel, we need to 
            //        // resize it
            //        if (imageRect.Size.Height != panelRect.Size.Height)
            //        {
            //            // find out how much shorter/taller it is than the panel
            //            float resizePercent = (float)panelRect.Height / (float)imageRect.Height;
            //            // and then adjust the width so that the aspect ratio remains intact
            //            imageRect.Size = new Size((int)((float)imageRect.Width * resizePercent), panelRect.Height);
            //        }

            //        // Establish the position of the image within the container panel. 
            //        // Since we earlier performed a sanity check to ensure a valid 
            //        // position, we can assume that all is well at this point. By placing 
            //        // this code outside the if statement above, we can also correctly 
            //        // determine the image location if it's the same size as the panel.
            //        // (Thanks go out to "Liesbet" on CodeProject for this fix.)
            //        switch (this.GraphicPanelImagePosition)
            //        {
            //            case WizardImagePosition.Right :
            //                imageRect.Location = new Point(panelRect.Width - imageRect.Width, 0);
            //                break;
            //            case WizardImagePosition.Left :
            //                imageRect.Location = new Point(0, 0);
            //                break;
            //            case WizardImagePosition.Center :
            //                imageRect.Location = new Point((int)(((float)panelRect.Width - (float)imageRect.Width) * 0.5), 0);
            //                //this.GraphicPanelTitle = "";
            //                //this.GraphicPanelSubtitle = "";
            //                break;
            //        }
            //    }
            //    // The direction of the gradient is determined by the location of the image. 
            //    // If the image is in the center, two gradients are painted - one from each 
            //    // outside edge of the panel.

            //    // Assume the image is at one side or the other (as opposed to the center).
            //    bool needOppositeGradient = false;
            //    if (needGradient)
            //    {
            //        switch (this.GraphicPanelImagePosition)
            //        {
            //            case WizardImagePosition.Left :
            //                if (!m_graphicPanelImageIsTransparent)
            //                {
            //                    gradientRect.Location = new Point(imageRect.Width-1, 0);
            //                    gradientRect.Size = new Size(gradientRect.Width - imageRect.Width, gradientRect.Height);
            //                    gradientDirection = 180;
            //                }
            //                break;
            //            case WizardImagePosition.Right :
            //                if (!m_graphicPanelImageIsTransparent)
            //                {
            //                    gradientRect.Location = new Point(0, 0);
            //                    gradientRect.Size = new Size(gradientRect.Width - imageRect.Width, gradientRect.Height);
            //                    gradientDirection = 0;
            //                }
            //                break;
            //            case WizardImagePosition.Center :
            //                {
            //                    needOppositeGradient = true;

            //                    gradientRect.Location = new Point(0, 0);
            //                    gradientRect.Size = new Size((int)(((float)gradientRect.Width - (float)imageRect.Width) * 0.5), gradientRect.Height);
            //                    // initially create the brush for the left-right gradient
            //                    gradientDirection = 0;
            //                }
            //                break;
            //        }
            //        // we can now create our gradient brush
            //        gradientBrush = new LinearGradientBrush(gradientRect, this.GraphicPanelGradientColor, this.GraphicPanelBackgroundColor, gradientDirection);
            //    }

            //    // clear our panel with the background color
            //    g.Clear(this.GraphicPanelBackgroundColor);

            //    // if we're going to paint a gradient, paint it
            //    if (needGradient && gradientBrush != null)
            //    {
            //        g.FillRectangle(gradientBrush, gradientRect);
            //        if (needOppositeGradient)
            //        {
            //            // clean up the brush
            //            gradientBrush.Dispose();
            //            // revsrse the direction of the gradient
            //            gradientDirection = (gradientDirection == 180) ? 0 : 180;
            //            // move the rectangle to the right side of the bitmap
            //            gradientRect.Location = new Point(gradientRect.Width + imageRect.Width, 0);
            //            // create a new gradient brush for the right side 
            //            gradientBrush = new LinearGradientBrush(gradientRect, this.GraphicPanelGradientColor, this.GraphicPanelBackgroundColor, gradientDirection);
            //            // paint!
            //            g.FillRectangle(gradientBrush, gradientRect);
            //        }
            //        // and clean up the brush
            //        gradientBrush.Dispose();
            //    }

            //    // if we have an image to display, paint it
            //    if (image != null)
            //    {
            //        g.DrawImage(image, imageRect);
            //        // and don't forget to clean up our image resource
            //        image.Dispose();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    if (ex != null) { }
            //    throw;
            //}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Paint the title and subtitle text. This should happen whenever the current 
		/// page is changed.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="subtitle"></param>
		protected void PaintTitle()
		{
			// Set our text, color and initial location - if the image is placed in 
			// the center, we don't draw the title/subtitle.
			if (this.GraphicPanelImagePosition == WizardImagePosition.Center)
			{
				this.labelTitle.Visible = false;
				this.labelSubtitle.Visible = false;
				return;
			}
			else
			{
				this.labelTitle.Visible = true;
				this.labelSubtitle.Visible = true;
			}

			// configure the title label
			this.labelTitle.AutoSize	= true;
			this.labelTitle.Text		= m_pageChain.GetCurrentPage().Title;
			this.labelTitle.Font		= m_graphicPanelTitleFont;
			this.labelTitle.ForeColor	= m_graphicPanelTitleColor;
			// configure the subtitle label
			this.labelSubtitle.AutoSize	= true;
			this.labelSubtitle.Text		= m_pageChain.GetCurrentPage().Subtitle;
			this.labelSubtitle.Font		= m_graphicPanelSubtitleFont;
			this.labelSubtitle.ForeColor= m_graphicPanelSubtitleColor;

			if (this.GraphicPanelImagePosition == WizardImagePosition.Left)
			{
				this.labelTitle.Location = new Point(this.graphicPanelTop.Width - 10 - this.labelTitle.Size.Width, this.labelTitle.Location.Y);
				this.labelSubtitle.Location = new Point(this.graphicPanelTop.Width - 10 - this.labelSubtitle.Size.Width, this.labelSubtitle.Location.Y);
			}

			// we need the panel rect so we can correctly position the text in 
			// the center of the panel
			Rectangle panelRect = new Rectangle(0, 0, this.graphicPanelTop.Width, this.graphicPanelTop.Height);

		    try
		    {
		        using (Graphics g = Graphics.FromHwndInternal(this.Handle))
		        {
					// combine the heights so we can determine first y-position.
		            int textHeight = (this.labelTitle.Height + this.labelSubtitle.Height);
					int y = (int)(((float)panelRect.Height - (float)textHeight) * 0.5f);
					// position the title
					this.labelTitle.Location = new Point(this.labelTitle.Location.X, y);
					// calculate the y-position of the subtitle
					y += this.labelTitle.Size.Height; 
					// and then position it
					this.labelSubtitle.Location = new Point(this.labelSubtitle.Location.X, this.labelSubtitle.Location.Y);
					// make them paint
					this.labelTitle.Invalidate();
					this.labelSubtitle.Invalidate();
				}
			}
			catch (Exception ex)
			{
				if (ex != null) { }
				throw;
			}

		}
		#endregion Graphic panel painting

		#region Helper methods
		//--------------------------------------------------------------------------------
		/// <summary>
		/// This method allows this object to add the wizard page to the pagePanel 
		/// container. While we're here, we establish the start and stop page if 
		/// possible.
		/// </summary>
		/// <param name="page"></param>
		public void PageCreated(WizardPage page)
		{
			pagePanel.Controls.Add(page);

			// hide the appropriate buttons (this should be specified wherever you 
			// instantiate your wizard object)
			if (ButtonStartHide)
			{
				page.ButtonStateStart &= ~WizardButtonState.Visible;
			}
			if (ButtonBackHide)
			{
				page.ButtonStateBack &= ~WizardButtonState.Visible;
			}
			if (ButtonNextHide)
			{
				page.ButtonStateNext &= ~WizardButtonState.Visible;
			}
			if (ButtonCancelHide)
			{
				page.ButtonStateCancel &= ~WizardButtonState.Visible;
			}
			if (ButtonHelpHide)
			{
				page.ButtonStateHelp &= ~WizardButtonState.Visible;
			}

			// I realize some of the exceptions seem redundant, but it helps to better 
			// diagnose a programming error.
			switch (page.WizardPageType)
			{
				case WizardPageType.Start :
					{
						if (m_startPage != null)
						{
							throw new WizardFormException("A start page has already been specified.");
						}
						if (m_stopPage != null)
						{
							throw new WizardFormException("A start page cannot be specified after a stop page has been specified.");
						}
						if (this.PageCount > 0)
						{
							throw new WizardFormException("A start page cannot be specified after other pages have been specified.");
						}
						m_startPage = page;
					}
					break;
				case WizardPageType.Stop :
					{
						if (m_stopPage != null)
						{
							throw new WizardFormException("A stop page has already been specified.");
						}
						if (m_startPage == null)
						{
							throw new WizardFormException("A stop page cannot be specified until a start page has been specified.");
						}
						m_stopPage = page;
					}
					break;
				case WizardPageType.Intermediate :
					{
						if (m_startPage == null)
						{
							throw new WizardFormException("Intermediate pages cannot be specified until a start page has been specified.");
						}
						if (m_stopPage != null)
						{
							throw new WizardFormException("Intermediate pages cannot be specified after a stop page has been specified.");
						}
					}
					break;
			}
			m_pageCount++;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Called by the wizard page when it's created to allow this object to resize 
		/// itself. The desired panel size is increased on whatever axis is larger than 
		/// the current value.  
		/// </summary>
		/// <param name="pageSize"></param>
		public void DiscoverPagePanelSize(Size pageSize)
		{
			if (pageSize.Width > m_desiredPagePanelSize.Width)
			{
				m_desiredPagePanelSize.Width = pageSize.Width;
			}
			if (pageSize.Height > m_desiredPagePanelSize.Height)
			{
				m_desiredPagePanelSize.Height = pageSize.Height;
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Seeds the page chain, resizes the form to be large enough to contain the 
		/// desired size of the pagePanel container, and finally, shows the start page.
		/// </summary>
		public void StartWizard()
		{
			if (m_pageCount == 0)
			{
				throw new WizardFormException("There are no pages in the wizard.");
			}
			if (m_startPage == null)
			{
				throw new WizardFormException("A start page has not been added to the wizard.");
			}
			if (m_stopPage == null)
			{
				throw new WizardFormException("A stop page has not been added to the wizard.");
			}

			this.Width += (m_desiredPagePanelSize.Width  - this.pagePanel.Size.Width);
			this.Height += (m_desiredPagePanelSize.Height - this.pagePanel.Size.Height);

			// seed the chain
			m_pageChain.GoNext(m_startPage);
			UpdateWizardForm(m_startPage);

			// broadcast the "wizard started" event
			Raise_WizardFormStartedEvent(new WizardFormStartedArgs());
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Determines if the specified state has the Visible flag turned on
		/// </summary>
		/// <param name="state">The state to be checked</param>
		/// <returns>True if the Visible flag is turned on</returns>
		public bool PageIsVisible(WizardButtonState state)
		{
			return (state & WizardButtonState.Visible) == WizardButtonState.Visible;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Determines if the specified state has the Enabled flag turned on
		/// </summary>
		/// <param name="state">The state to be checked</param>
		/// <returns>True if the Enabled flag is turned on</returns>
		public bool PageIsEnabled(WizardButtonState state)
		{
			return (state & WizardButtonState.Enabled) == WizardButtonState.Enabled;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Updates the state of the buttons on the form base on the specified page 
		/// settings.
		/// </summary>
		/// <param name="page">The page controlling the button state</param>
		public void UpdateWizardForm(WizardPage page)
		{
		    PaintTitle();

			// take care of changing the buttons to their appropriate state for the activated page
			this.buttonStart.Visible	= PageIsVisible(page.ButtonStateStart);
			this.buttonStart.Enabled	= PageIsEnabled(page.ButtonStateStart);
			this.buttonBack.Visible		= PageIsVisible(page.ButtonStateBack);
			this.buttonBack.Enabled		= PageIsEnabled(page.ButtonStateBack);
			this.buttonNext.Visible		= PageIsVisible(page.ButtonStateNext);
			this.buttonNext.Enabled		= PageIsEnabled(page.ButtonStateNext);
			this.buttonCancel.Visible	= PageIsVisible(page.ButtonStateCancel);
			this.buttonCancel.Enabled	= PageIsEnabled(page.ButtonStateCancel);
			this.buttonHelp.Visible		= PageIsVisible(page.ButtonStateHelp);
			this.buttonHelp.Enabled		= PageIsEnabled(page.ButtonStateHelp);

			// see if we need to change the text of the Next button
			if (page.WizardPageType == WizardPageType.Stop)
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
		#endregion Helper methods
	}
}


