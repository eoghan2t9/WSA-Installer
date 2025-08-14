namespace WSA_Installer.Pages
{
    partial class InstallingPage
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
			this.lblTotalFiles = new System.Windows.Forms.Label();
			this.lblBytes = new System.Windows.Forms.Label();
			this.lblSpeed = new System.Windows.Forms.Label();
			this.lblProgressPercentage = new System.Windows.Forms.Label();
			this.showDetailsBtn = new System.Windows.Forms.Button();
			this.statusLbl = new System.Windows.Forms.Label();
			this.progressBar1 = new WSA_Installer.ProgressBarEx();
			this.detailsBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// lblTotalFiles
			// 
			this.lblTotalFiles.AutoSize = true;
			this.lblTotalFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.lblTotalFiles.ForeColor = System.Drawing.Color.Silver;
			this.lblTotalFiles.Location = new System.Drawing.Point(404, 221);
			this.lblTotalFiles.Name = "lblTotalFiles";
			this.lblTotalFiles.Size = new System.Drawing.Size(118, 13);
			this.lblTotalFiles.TabIndex = 27;
			this.lblTotalFiles.Text = "Downloaded 0 of 0 files";
			// 
			// lblBytes
			// 
			this.lblBytes.AutoSize = true;
			this.lblBytes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.lblBytes.ForeColor = System.Drawing.Color.Silver;
			this.lblBytes.Location = new System.Drawing.Point(227, 221);
			this.lblBytes.Name = "lblBytes";
			this.lblBytes.Size = new System.Drawing.Size(124, 13);
			this.lblBytes.TabIndex = 26;
			this.lblBytes.Text = "Downloaded: 0 bytes / ?";
			// 
			// lblSpeed
			// 
			this.lblSpeed.AutoSize = true;
			this.lblSpeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.lblSpeed.ForeColor = System.Drawing.Color.Silver;
			this.lblSpeed.Location = new System.Drawing.Point(114, 221);
			this.lblSpeed.Name = "lblSpeed";
			this.lblSpeed.Size = new System.Drawing.Size(77, 13);
			this.lblSpeed.TabIndex = 25;
			this.lblSpeed.Text = "Speed: 0 KB/s";
			// 
			// lblProgressPercentage
			// 
			this.lblProgressPercentage.AutoSize = true;
			this.lblProgressPercentage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.lblProgressPercentage.ForeColor = System.Drawing.Color.Silver;
			this.lblProgressPercentage.Location = new System.Drawing.Point(13, 221);
			this.lblProgressPercentage.Name = "lblProgressPercentage";
			this.lblProgressPercentage.Size = new System.Drawing.Size(77, 13);
			this.lblProgressPercentage.TabIndex = 24;
			this.lblProgressPercentage.Text = "Progress: 0.0%";
			// 
			// showDetailsBtn
			// 
			this.showDetailsBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.showDetailsBtn.Location = new System.Drawing.Point(434, 46);
			this.showDetailsBtn.Name = "showDetailsBtn";
			this.showDetailsBtn.Size = new System.Drawing.Size(88, 23);
			this.showDetailsBtn.TabIndex = 2;
			this.showDetailsBtn.Text = "Show details";
			this.showDetailsBtn.UseVisualStyleBackColor = true;
			this.showDetailsBtn.Click += new System.EventHandler(this.showDetailsBtn_Click);
			// 
			// statusLbl
			// 
			this.statusLbl.AutoSize = true;
			this.statusLbl.ForeColor = System.Drawing.Color.Silver;
			this.statusLbl.Location = new System.Drawing.Point(13, 2);
			this.statusLbl.Name = "statusLbl";
			this.statusLbl.Size = new System.Drawing.Size(57, 13);
			this.statusLbl.TabIndex = 1;
			this.statusLbl.Text = "Installing...";
			// 
			// progressBar1
			// 
			this.progressBar1.Enabled = false;
			this.progressBar1.Location = new System.Drawing.Point(16, 17);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(506, 23);
			this.progressBar1.Step = 1;
			this.progressBar1.TabIndex = 0;
			// 
			// detailsBox
			// 
			this.detailsBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.detailsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.detailsBox.ForeColor = System.Drawing.Color.Silver;
			this.detailsBox.FormattingEnabled = true;
			this.detailsBox.Location = new System.Drawing.Point(16, 46);
			this.detailsBox.Name = "detailsBox";
			this.detailsBox.Size = new System.Drawing.Size(506, 171);
			this.detailsBox.TabIndex = 3;
			this.detailsBox.Visible = false;
			// 
			// InstallingPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.Controls.Add(this.lblTotalFiles);
			this.Controls.Add(this.lblBytes);
			this.Controls.Add(this.lblSpeed);
			this.Controls.Add(this.lblProgressPercentage);
			this.Controls.Add(this.showDetailsBtn);
			this.Controls.Add(this.statusLbl);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.detailsBox);
			this.ForeColor = System.Drawing.Color.Silver;
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "InstallingPage";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button showDetailsBtn;
        public WSA_Installer.ProgressBarEx progressBar1;
        public System.Windows.Forms.ListBox detailsBox;
        public System.Windows.Forms.Label statusLbl;
        public System.Windows.Forms.Label lblTotalFiles;
        public System.Windows.Forms.Label lblBytes;
        public System.Windows.Forms.Label lblSpeed;
        public System.Windows.Forms.Label lblProgressPercentage;
    }
}
