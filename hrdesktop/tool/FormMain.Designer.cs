﻿namespace highwayns
{
    partial class FormMain
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
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnJobType = new System.Windows.Forms.Button();
            this.btnDatabase = new System.Windows.Forms.Button();
            this.btnTranslate = new System.Windows.Forms.Button();
            this.btnMessage = new System.Windows.Forms.Button();
            this.btnAddress = new System.Windows.Forms.Button();
            this.btnDBData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(49, 26);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 0;
            this.btnConvert.Text = "FileConvert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnJobType
            // 
            this.btnJobType.Location = new System.Drawing.Point(156, 26);
            this.btnJobType.Name = "btnJobType";
            this.btnJobType.Size = new System.Drawing.Size(75, 23);
            this.btnJobType.TabIndex = 1;
            this.btnJobType.Text = "JobType";
            this.btnJobType.UseVisualStyleBackColor = true;
            this.btnJobType.Click += new System.EventHandler(this.btnJobType_Click);
            // 
            // btnDatabase
            // 
            this.btnDatabase.Location = new System.Drawing.Point(269, 26);
            this.btnDatabase.Name = "btnDatabase";
            this.btnDatabase.Size = new System.Drawing.Size(75, 23);
            this.btnDatabase.TabIndex = 2;
            this.btnDatabase.Text = "DataBase";
            this.btnDatabase.UseVisualStyleBackColor = true;
            this.btnDatabase.Click += new System.EventHandler(this.btnDatabase_Click);
            // 
            // btnTranslate
            // 
            this.btnTranslate.Location = new System.Drawing.Point(375, 26);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(75, 23);
            this.btnTranslate.TabIndex = 3;
            this.btnTranslate.Text = "Translate";
            this.btnTranslate.UseVisualStyleBackColor = true;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // btnMessage
            // 
            this.btnMessage.Location = new System.Drawing.Point(49, 68);
            this.btnMessage.Name = "btnMessage";
            this.btnMessage.Size = new System.Drawing.Size(75, 23);
            this.btnMessage.TabIndex = 4;
            this.btnMessage.Text = "Message";
            this.btnMessage.UseVisualStyleBackColor = true;
            this.btnMessage.Click += new System.EventHandler(this.btnMessage_Click);
            // 
            // btnAddress
            // 
            this.btnAddress.Location = new System.Drawing.Point(156, 68);
            this.btnAddress.Name = "btnAddress";
            this.btnAddress.Size = new System.Drawing.Size(75, 23);
            this.btnAddress.TabIndex = 5;
            this.btnAddress.Text = "Address";
            this.btnAddress.UseVisualStyleBackColor = true;
            this.btnAddress.Click += new System.EventHandler(this.btnAddress_Click);
            // 
            // btnDBData
            // 
            this.btnDBData.Location = new System.Drawing.Point(269, 68);
            this.btnDBData.Name = "btnDBData";
            this.btnDBData.Size = new System.Drawing.Size(75, 23);
            this.btnDBData.TabIndex = 6;
            this.btnDBData.Text = "DBData";
            this.btnDBData.UseVisualStyleBackColor = true;
            this.btnDBData.Click += new System.EventHandler(this.btnDBData_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 261);
            this.Controls.Add(this.btnDBData);
            this.Controls.Add(this.btnAddress);
            this.Controls.Add(this.btnMessage);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.btnDatabase);
            this.Controls.Add(this.btnJobType);
            this.Controls.Add(this.btnConvert);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnJobType;
        private System.Windows.Forms.Button btnDatabase;
        private System.Windows.Forms.Button btnTranslate;
        private System.Windows.Forms.Button btnMessage;
        private System.Windows.Forms.Button btnAddress;
        private System.Windows.Forms.Button btnDBData;
    }
}