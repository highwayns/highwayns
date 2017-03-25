namespace HPSManagement
{
    partial class FormFileConverter
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
            this.btnConvertToPdf = new System.Windows.Forms.Button();
            this.btnConvertToJpg = new System.Windows.Forms.Button();
            this.btnGetText = new System.Windows.Forms.Button();
            this.btnSplit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConvertToPdf
            // 
            this.btnConvertToPdf.Location = new System.Drawing.Point(120, 32);
            this.btnConvertToPdf.Name = "btnConvertToPdf";
            this.btnConvertToPdf.Size = new System.Drawing.Size(117, 23);
            this.btnConvertToPdf.TabIndex = 0;
            this.btnConvertToPdf.Text = "ConvertOfficeToPdf";
            this.btnConvertToPdf.UseVisualStyleBackColor = true;
            this.btnConvertToPdf.Click += new System.EventHandler(this.btnConvertToPdf_Click);
            // 
            // btnConvertToJpg
            // 
            this.btnConvertToJpg.Location = new System.Drawing.Point(120, 103);
            this.btnConvertToJpg.Name = "btnConvertToJpg";
            this.btnConvertToJpg.Size = new System.Drawing.Size(117, 23);
            this.btnConvertToJpg.TabIndex = 1;
            this.btnConvertToJpg.Tag = "using EPocalipse.IFilter;";
            this.btnConvertToJpg.Text = "ConvertPDFToJpg";
            this.btnConvertToJpg.UseVisualStyleBackColor = true;
            this.btnConvertToJpg.Click += new System.EventHandler(this.btnConvertToJpg_Click);
            // 
            // btnGetText
            // 
            this.btnGetText.Location = new System.Drawing.Point(120, 168);
            this.btnGetText.Name = "btnGetText";
            this.btnGetText.Size = new System.Drawing.Size(117, 23);
            this.btnGetText.TabIndex = 2;
            this.btnGetText.Tag = "using EPocalipse.IFilter;";
            this.btnGetText.Text = "GetOfficeText";
            this.btnGetText.UseVisualStyleBackColor = true;
            this.btnGetText.Click += new System.EventHandler(this.btnGetText_Click);
            // 
            // btnSplit
            // 
            this.btnSplit.Location = new System.Drawing.Point(120, 226);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(117, 23);
            this.btnSplit.TabIndex = 3;
            this.btnSplit.Tag = "using EPocalipse.IFilter;";
            this.btnSplit.Text = "Split PDF";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // FormFileConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 261);
            this.Controls.Add(this.btnSplit);
            this.Controls.Add(this.btnGetText);
            this.Controls.Add(this.btnConvertToJpg);
            this.Controls.Add(this.btnConvertToPdf);
            this.Name = "FormFileConverter";
            this.Text = "FormFileConverter";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConvertToPdf;
        private System.Windows.Forms.Button btnConvertToJpg;
        private System.Windows.Forms.Button btnGetText;
        private System.Windows.Forms.Button btnSplit;
    }
}