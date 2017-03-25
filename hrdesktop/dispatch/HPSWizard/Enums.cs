using System;

namespace HPSWizard
{
	public enum WizardPageType		{ Start=0, Intermediate=1, Stop=2 };
	public enum WizardStepType		{ None=0, Previous=1, Next=2, Finished=3 };
	public enum WizardImagePosition	{ Left=0, Center=1, Right=2, Top=3, Middle=4, Bottom=5 };
	[Flags]
	public enum WizardButtonState	{ Enabled=1, Visible=2 };
}
