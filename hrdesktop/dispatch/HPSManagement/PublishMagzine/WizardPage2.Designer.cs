namespace HPSManagement.PublishMagzine
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
            this.cmbMagzineNo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbMagzineNo
            // 
            resources.ApplyResources(this.cmbMagzineNo, "cmbMagzineNo");
            this.cmbMagzineNo.FormattingEnabled = true;
            this.cmbMagzineNo.Name = "cmbMagzineNo";
            this.cmbMagzineNo.SelectedIndexChanged += new System.EventHandler(this.cmbMagzineNo_SelectedIndexChanged);
            // 
            // WizardPage2
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbMagzineNo);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(447, 240);
            this.Name = "WizardPage2";
            this.Subtitle = "期号选择";
            this.Title = "第二步 - 请选择期号";
            this.Load += new System.EventHandler(this.WizardPage2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbMagzineNo;

    }
}
