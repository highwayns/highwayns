using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HPSWizard
{
	/// <summary>
	/// This class exists to satisfy FXCop rules about not manually instantiating 
	/// System.Exceptions in code
	/// </summary>
	[Serializable()]
	public class WizardFormException : System.Exception
	{
		//--------------------------------------------------------------------------------
		public WizardFormException() 
			: base()
		{ }
	    
		//--------------------------------------------------------------------------------
		public WizardFormException(string message) 
			: base(message)
		{}

		//--------------------------------------------------------------------------------
		public WizardFormException(string message, System.Exception inner) 
			: base(message, inner)
		{ }

		//--------------------------------------------------------------------------------
		// Constructor needed for serialization when exception propagates from a remoting 
		// server to the client.
		protected WizardFormException(System.Runtime.Serialization.SerializationInfo info,
											 System.Runtime.Serialization.StreamingContext context) 
			: base(info, context)
		{ }
	}
}
