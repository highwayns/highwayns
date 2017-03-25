using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HPSWizard
{
	/// <summary>
	/// This class maintains a list of visited wizard pages, and manages visibility of 
	/// those pages.  If a page has been visited on the way to the stop page, it will 
	/// be in this list.
	/// </summary>
	public class WizardPageChain
	{
		#region Data members
		private List<WizardPage> m_pageChain = new List<WizardPage>();
		private WizardFormBase m_parent;
		#endregion Data members

		#region Properties
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get the number of pages currently in the list
		/// </summary>
		public int Count
		{
			get { return m_pageChain.Count; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get the wizard page chain list
		/// </summary>
		public List<WizardPage> PageChain
		{
			get { return m_pageChain; }
		}
		#endregion Properties

		#region Constructors
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parent">The WizardFormBase that conatins this chain</param>
		public WizardPageChain(WizardFormBase parent)
		{
			m_parent = parent;
			this.m_pageChain.Clear();
		}
		#endregion Constructors

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get the current page (the last page in the list).
		/// </summary>
		/// <returns></returns>
		public WizardPage GetCurrentPage()
		{
			if (this.Count > 0)
			{
				return (WizardPage)this.m_pageChain[this.Count-1]; 
			}
			else
			{
				throw new WizardFormException("No pages in page chain list.");
			}
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Moves to the first wizard page. All pages except the first page are also 
		/// removed from the page chain list, and the remaining page is shown.
		/// </summary>
		/// <returns>The new current wizard page</returns>
		/// <remarks>Added on 02/19/2009 by jms.</remarks>
		public WizardPage GoFirst()
		{
			if (this.Count > 1)
			{
				this.GetCurrentPage().Visible = false;
				// remove all but the firsat page from the page chain
				this.m_pageChain.RemoveRange(1, this.Count - 1);
			}
			else
			{
				throw new WizardFormException("No pages in page chain list.");
			}
			WizardPage currentPage = this.GetCurrentPage();
			currentPage.Visible = true;
			return currentPage;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Moves backwards through the chain of viewed property pages. It removes the 
		/// last page from the list, and shows the new last page in the list.
		/// </summary>
		/// <returns>The new current wizard page</returns>
		public WizardPage GoBack()
		{
			if (this.Count > 1)
			{
				this.GetCurrentPage().Visible = false;
				this.m_pageChain.RemoveAt(this.Count - 1);
			}
			else
			{
				throw new WizardFormException("No pages in page chain list.");
			}
			WizardPage currentPage = this.GetCurrentPage();
			currentPage.Visible = true;
			return currentPage;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified page to the list, hides the old current page, and 
		/// shows the newly added page.
		/// </summary>
		/// <param name="nextPage">The wizard page to add to the list</param>
		/// <returns>The new current wizard page</returns>
		public WizardPage GoNext(WizardPage nextPage)
		{
			m_pageChain.Add(nextPage);
			WizardPage currentPage = this.GetCurrentPage();
			if (this.Count > 1)
			{
				((WizardPage)(m_pageChain[this.Count-2])).Visible = false;
			}
			currentPage.Visible = true;
			return currentPage;
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Cycles through each page in the list (starting with the first one), and 
		/// calls the SaveData function in that page. If a page returns false, this 
		/// method will return the page that faulted.
		/// </summary>
		/// <returns>The WizardPage object that failed during the save data process</returns>
		public WizardPage SaveData()
		{
			WizardPage invalidPage = null;
			foreach (WizardPage page in m_pageChain)
			{
				if (!page.SaveData())
				{
					invalidPage = page;
					break;
				}
			}
			return invalidPage;
		}
	}
}

