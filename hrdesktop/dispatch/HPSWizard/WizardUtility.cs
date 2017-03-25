using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HPSWizard
{
	public static class WizardUtility
	{

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Determines if the programmer is using the designer to modify this control 
		/// (or controls derived from this class).
		/// </summary>
		/// <returns>True if the designer is displaying this control</returns>
		public static bool IsDesignTime()
		{
			// There are several methods for determining if you're in the designer or 
			// running as an application. I've left the first two methods that I tried 
			// in the code (commented out).

			// This method has to be used IN THE CLASS that you're checking.
			//return ((GetService(typeof(IDesignerHost)) != null);

			// this one just plain didn't work for me, but it may work in other situations.
			//return (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

			// finally - one that worked as desired. I see a potential problem, though, 
			// if Micrsoft decides to change the IDEs ProcessName property. I can verify 
			// that this will at least work in VS2005 and VS2008.
			return (Process.GetCurrentProcess().ProcessName.ToLower() == "devenv");
		}


	}
}
