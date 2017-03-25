using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using CookComputing.XmlRpc;

namespace AsyncBettyApplication
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button butGetName;
    private System.Windows.Forms.ListBox lstOutput;
    private System.Windows.Forms.TextBox txtStateNumber;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.butGetName = new System.Windows.Forms.Button();
            this.txtStateNumber = new System.Windows.Forms.TextBox();
            this.lstOutput = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // butGetName
            // 
            this.butGetName.Location = new System.Drawing.Point(202, 26);
            this.butGetName.Name = "butGetName";
            this.butGetName.Size = new System.Drawing.Size(81, 22);
            this.butGetName.TabIndex = 2;
            this.butGetName.Text = "Get Name";
            this.butGetName.Click += new System.EventHandler(this.butGetName_Click);
            // 
            // txtStateNumber
            // 
            this.txtStateNumber.Location = new System.Drawing.Point(125, 26);
            this.txtStateNumber.Name = "txtStateNumber";
            this.txtStateNumber.Size = new System.Drawing.Size(57, 21);
            this.txtStateNumber.TabIndex = 1;
            // 
            // lstOutput
            // 
            this.lstOutput.ItemHeight = 12;
            this.lstOutput.Location = new System.Drawing.Point(10, 103);
            this.lstOutput.Name = "lstOutput";
            this.lstOutput.Size = new System.Drawing.Size(297, 184);
            this.lstOutput.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.butGetName);
            this.groupBox1.Controls.Add(this.txtStateNumber);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(10, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 69);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "examples.getStateName";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "State Number";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(332, 295);
            this.Controls.Add(this.lstOutput);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "AsyncBettyApp";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

    }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

    delegate void AppendSuccessDelegate(int stateNum, string stateName);
    delegate void AppendExceptionDelegate(Exception ex);

    static void GetStateNameCallback(IAsyncResult result)
    {
      XmlRpcAsyncResult clientResult = (XmlRpcAsyncResult)result;
      IStateName betty = (IStateName)clientResult.ClientProtocol;
      BettyAsyncState asyncState = (BettyAsyncState)result.AsyncState;
      try 
      {
        string s = betty.EndGetStateName(result);
        asyncState.theForm.Invoke(new AppendSuccessDelegate(
          asyncState.theForm.AppendSuccess), asyncState.stateNumber, s);
      }
      catch (Exception ex)
      {
        asyncState.theForm.Invoke(new AppendExceptionDelegate(
          asyncState.theForm.AppendException), ex);
      }
    }

    private void butGetName_Click(object sender, System.EventArgs e)
    {
      IStateName betty = XmlRpcProxyGen.Create<IStateName>();
      betty.Timeout = 10000;
      try
      {
        AsyncCallback acb = new AsyncCallback(GetStateNameCallback);
        int num = Convert.ToInt32(txtStateNumber.Text);
        BettyAsyncState asyncState = new BettyAsyncState(num, this);
        IAsyncResult asr = betty.BeginGetStateName(num, acb, asyncState);
        if (asr.CompletedSynchronously)
        {
          string ret = betty.EndGetStateName(asr);
          AppendSuccess(num, ret);
        }
      }
      catch (Exception ex)
      {
        AppendException(ex);
      }
    }

    private void AppendSuccess(int stateNum, string stateName)
    {
      string s = String.Format("State {0} = {1}", stateNum, stateName);
      lstOutput.Items.Insert(0,s);
    }

    private void AppendException(Exception ex)
    {
      string s;
      try
      {
        throw ex;
      }
      catch(XmlRpcFaultException fex)
      {
        s = String.Format("Fault Response: {0} {1}", 
                          fex.FaultCode,fex.FaultString);
      }
      catch(WebException webEx)
      {
        s = String.Format("WebException: {0}", webEx.Message);
        if (webEx.Response != null)
          webEx.Response.Close();
      }
      catch(Exception excep)
      {
        s = String.Format("Exception: {0}", excep.Message);
      }
      lstOutput.Items.Insert(0,s);
    }
	}

  class BettyAsyncState
  {
    public BettyAsyncState(int StateNumber, Form1 TheForm)
    {
      stateNumber = StateNumber;
      theForm = TheForm;
    }
    public int stateNumber;
    public Form1 theForm;
  }
}
