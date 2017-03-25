namespace HPSManagement.CreateMagzine
{
	partial class WizardPage2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardPage2));
            this.label1 = new System.Windows.Forms.Label();
            this.txtMagzineNo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtMagzineNo
            // 
            resources.ApplyResources(this.txtMagzineNo, "txtMagzineNo");
            this.txtMagzineNo.Name = "txtMagzineNo";
            this.txtMagzineNo.TextChanged += new System.EventHandler(this.txtMagzineNo_TextChanged);
            // 
            // WizardPage2
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.txtMagzineNo);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(447, 240);
            this.Name = "WizardPage2";
            this.Subtitle = "期号选择";
            this.Title = "第二步 请选择期号";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMagzineNo;

    }
}
