namespace HPSManagement.PublishMagzine
{
	partial class WizardPage41
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardPage41));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            resources.ApplyResources(this.webBrowser1, "webBrowser1");
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            // 
            // WizardPage41
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.webBrowser1);
            this.MinimumSize = new System.Drawing.Size(447, 240);
            this.Name = "WizardPage41";
            this.Subtitle = "网页登陆";
            this.Title = "第四步 -  请登录到网页";
            this.Load += new System.EventHandler(this.WizardPage41_Load);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.WebBrowser webBrowser1;


    }
}
