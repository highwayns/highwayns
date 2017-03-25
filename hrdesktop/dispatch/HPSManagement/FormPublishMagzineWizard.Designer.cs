namespace HPSManagement
{
    partial class FormPublishMagzineWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPublishMagzineWizard));
            this.graphicPanelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pagePanel
            // 
            resources.ApplyResources(this.pagePanel, "pagePanel");
            // 
            // labelTopSeparatorLine
            // 
            resources.ApplyResources(this.labelTopSeparatorLine, "labelTopSeparatorLine");
            // 
            // graphicPanelTop
            // 
            resources.ApplyResources(this.graphicPanelTop, "graphicPanelTop");
            // 
            // labelBottomSeparatorLine
            // 
            resources.ApplyResources(this.labelBottomSeparatorLine, "labelBottomSeparatorLine");
            // 
            // buttonHelp
            // 
            resources.ApplyResources(this.buttonHelp, "buttonHelp");
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            // 
            // buttonNext
            // 
            resources.ApplyResources(this.buttonNext, "buttonNext");
            // 
            // buttonBack
            // 
            resources.ApplyResources(this.buttonBack, "buttonBack");
            // 
            // buttonStart
            // 
            resources.ApplyResources(this.buttonStart, "buttonStart");
            // 
            // labelSubtitle
            // 
            resources.ApplyResources(this.labelSubtitle, "labelSubtitle");
            // 
            // labelTitle
            // 
            resources.ApplyResources(this.labelTitle, "labelTitle");
            // 
            // FormPublishMagzineWizard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormPublishMagzineWizard";
            this.Load += new System.EventHandler(this.WizardExample_Load);
            this.graphicPanelTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

    }
}