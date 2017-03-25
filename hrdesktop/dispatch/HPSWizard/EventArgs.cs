using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace HPSWizard
{
	public delegate void WizardPageActivateHandler(object sender, WizardPageActivateArgs e);
	public delegate void WizardPageChangeHandler(object sender, WizardPageChangeArgs e);
	public delegate void WizardPageCreatedHandler(object sender, WizardPageCreatedArgs e);
	public delegate void WizardFormStartedHandler(object sender, WizardFormStartedArgs e);

	public class WizardPageActivateArgs : EventArgs
	{
		private WizardPage m_activePage = null;
		private WizardStepType m_stepType = WizardStepType.None;

		public WizardPage ActivatedPage
		{
			get { return m_activePage; }
		}
		public WizardStepType StepType
		{
			get { return m_stepType; }
		}

		public WizardPageActivateArgs(WizardPage page, WizardStepType step)
		{
			m_activePage = page;
			m_stepType = step;
		}
	}

	public class WizardPageChangeArgs : EventArgs
	{
		private WizardPage		m_activePage	= null;
		private WizardStepType	m_stepType		= WizardStepType.None;

		public WizardStepType StepType
		{
			get { return m_stepType; }
		}
		public WizardPage ActivatedPage
		{
			get { return m_activePage; }
		}

		public WizardPageChangeArgs(WizardPage page, WizardStepType step)
		{
			m_activePage	= page;
			m_stepType		= step;
		}
	}

	public class WizardPageCreatedArgs : EventArgs
	{
		private Size m_size;

		public Size Size
		{
			get { return m_size; }
		}

		public WizardPageCreatedArgs(Size size)
		{
			m_size = size;
		}
	}

	public class WizardFormStartedArgs : EventArgs
	{
		public WizardFormStartedArgs()
		{
		}
	}
}


