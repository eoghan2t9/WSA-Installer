namespace WSA_Installer.Pages
{
    partial class ChooseOptions
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.wsaToolsBox = new System.Windows.Forms.CheckBox();
			this.wsaCustomUserDataRadioBtn = new System.Windows.Forms.RadioButton();
			this.wsaDefaultUserDataRadioBtn = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.wsaBrowseCustomUserDataPackageBox = new System.Windows.Forms.Button();
			this.wsaCustomWsaUserDataPackageBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// wsaToolsBox
			// 
			this.wsaToolsBox.AutoSize = true;
			this.wsaToolsBox.Location = new System.Drawing.Point(21, 140);
			this.wsaToolsBox.Name = "wsaToolsBox";
			this.wsaToolsBox.Size = new System.Drawing.Size(131, 17);
			this.wsaToolsBox.TabIndex = 25;
			this.wsaToolsBox.Text = "Install Additional Tools";
			this.wsaToolsBox.UseVisualStyleBackColor = true;
			this.wsaToolsBox.CheckedChanged += new System.EventHandler(this.wsaToolsBox_CheckedChanged);
			// 
			// wsaCustomUserDataRadioBtn
			// 
			this.wsaCustomUserDataRadioBtn.AutoSize = true;
			this.wsaCustomUserDataRadioBtn.Enabled = false;
			this.wsaCustomUserDataRadioBtn.Location = new System.Drawing.Point(21, 59);
			this.wsaCustomUserDataRadioBtn.Name = "wsaCustomUserDataRadioBtn";
			this.wsaCustomUserDataRadioBtn.Size = new System.Drawing.Size(157, 17);
			this.wsaCustomUserDataRadioBtn.TabIndex = 28;
			this.wsaCustomUserDataRadioBtn.Text = "Custom User Data Package";
			this.wsaCustomUserDataRadioBtn.UseVisualStyleBackColor = true;
			this.wsaCustomUserDataRadioBtn.CheckedChanged += new System.EventHandler(this.wsaCustomUserDataRadioBtn_CheckedChanged);
			// 
			// wsaDefaultUserDataRadioBtn
			// 
			this.wsaDefaultUserDataRadioBtn.AutoSize = true;
			this.wsaDefaultUserDataRadioBtn.Checked = true;
			this.wsaDefaultUserDataRadioBtn.Location = new System.Drawing.Point(21, 36);
			this.wsaDefaultUserDataRadioBtn.Name = "wsaDefaultUserDataRadioBtn";
			this.wsaDefaultUserDataRadioBtn.Size = new System.Drawing.Size(184, 17);
			this.wsaDefaultUserDataRadioBtn.TabIndex = 27;
			this.wsaDefaultUserDataRadioBtn.TabStop = true;
			this.wsaDefaultUserDataRadioBtn.Text = "Default WSA User Data Package";
			this.wsaDefaultUserDataRadioBtn.UseVisualStyleBackColor = true;
			this.wsaDefaultUserDataRadioBtn.CheckedChanged += new System.EventHandler(this.wsaDefaultUserDataRadioBtn_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Enabled = false;
			this.label1.Location = new System.Drawing.Point(51, 83);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(139, 13);
			this.label1.TabIndex = 26;
			this.label1.Text = "Custom User Data Package";
			// 
			// wsaBrowseCustomUserDataPackageBox
			// 
			this.wsaBrowseCustomUserDataPackageBox.BackColor = System.Drawing.SystemColors.Control;
			this.wsaBrowseCustomUserDataPackageBox.Enabled = false;
			this.wsaBrowseCustomUserDataPackageBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.wsaBrowseCustomUserDataPackageBox.Location = new System.Drawing.Point(312, 98);
			this.wsaBrowseCustomUserDataPackageBox.Margin = new System.Windows.Forms.Padding(2);
			this.wsaBrowseCustomUserDataPackageBox.Name = "wsaBrowseCustomUserDataPackageBox";
			this.wsaBrowseCustomUserDataPackageBox.Size = new System.Drawing.Size(88, 23);
			this.wsaBrowseCustomUserDataPackageBox.TabIndex = 3;
			this.wsaBrowseCustomUserDataPackageBox.Text = "Browse...";
			this.wsaBrowseCustomUserDataPackageBox.UseVisualStyleBackColor = false;
			this.wsaBrowseCustomUserDataPackageBox.Click += new System.EventHandler(this.wsaBrowseCustomUserDataPackageBox_Click);
			// 
			// wsaCustomWsaUserDataPackageBox
			// 
			this.wsaCustomWsaUserDataPackageBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.wsaCustomWsaUserDataPackageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.wsaCustomWsaUserDataPackageBox.Enabled = false;
			this.wsaCustomWsaUserDataPackageBox.ForeColor = System.Drawing.Color.Silver;
			this.wsaCustomWsaUserDataPackageBox.Location = new System.Drawing.Point(52, 98);
			this.wsaCustomWsaUserDataPackageBox.Margin = new System.Windows.Forms.Padding(2);
			this.wsaCustomWsaUserDataPackageBox.Name = "wsaCustomWsaUserDataPackageBox";
			this.wsaCustomWsaUserDataPackageBox.Size = new System.Drawing.Size(256, 20);
			this.wsaCustomWsaUserDataPackageBox.TabIndex = 2;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(10, 8);
			this.label5.MaximumSize = new System.Drawing.Size(526, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(333, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Select options for WSA installation. Click Install to start the installation";
			// 
			// ChooseOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.Controls.Add(this.wsaToolsBox);
			this.Controls.Add(this.wsaCustomUserDataRadioBtn);
			this.Controls.Add(this.wsaDefaultUserDataRadioBtn);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.wsaBrowseCustomUserDataPackageBox);
			this.Controls.Add(this.wsaCustomWsaUserDataPackageBox);
			this.Controls.Add(this.label5);
			this.ForeColor = System.Drawing.Color.Silver;
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "ChooseOptions";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox wsaToolsBox;
        private System.Windows.Forms.Button wsaBrowseCustomUserDataPackageBox;
        private System.Windows.Forms.TextBox wsaCustomWsaUserDataPackageBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton wsaDefaultUserDataRadioBtn;
        private System.Windows.Forms.RadioButton wsaCustomUserDataRadioBtn;
	}
}
