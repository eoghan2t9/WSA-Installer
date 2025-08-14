namespace WSA_Installer.Pages
{
    partial class SetupNeedsRestartToFinishPage
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupNeedsRestartToFinishPage));
			this.showReleaseNotesBox = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lineLbl = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// showReleaseNotesBox
			// 
			this.showReleaseNotesBox.AutoSize = true;
			this.showReleaseNotesBox.Location = new System.Drawing.Point(210, 272);
			this.showReleaseNotesBox.Name = "showReleaseNotesBox";
			this.showReleaseNotesBox.Size = new System.Drawing.Size(119, 17);
			this.showReleaseNotesBox.TabIndex = 12;
			this.showReleaseNotesBox.Text = "Show release notes";
			this.showReleaseNotesBox.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(206, 20);
			this.label5.MaximumSize = new System.Drawing.Size(300, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(273, 48);
			this.label5.TabIndex = 11;
			this.label5.Text = "Restart needed to complete WSA Setup";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(207, 105);
			this.label4.MaximumSize = new System.Drawing.Size(300, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(291, 39);
			this.label4.TabIndex = 10;
			this.label4.Text = "A restart is required since we had to turn on Hyper-V, Virtual Machine Platform, " +
    "and Windows Hypervisor Platform in your Windows Features (WIN+R \"optionalfeature" +
    "s\"). ";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(207, 84);
			this.label1.MaximumSize = new System.Drawing.Size(300, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(208, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "WSA has been installed on your computer.";
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.pictureBox1.Image = global::WSA_Installer.Properties.Resources.welcomeBanner4;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(200, 324);
			this.pictureBox1.TabIndex = 6;
			this.pictureBox1.TabStop = false;
			// 
			// lineLbl
			// 
			this.lineLbl.Enabled = false;
			this.lineLbl.Location = new System.Drawing.Point(0, 327);
			this.lineLbl.Margin = new System.Windows.Forms.Padding(0);
			this.lineLbl.Name = "lineLbl";
			this.lineLbl.Size = new System.Drawing.Size(535, 14);
			this.lineLbl.TabIndex = 13;
			this.lineLbl.Text = "────────────────────────────────────────────────────────────────────";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(207, 210);
			this.label2.MaximumSize = new System.Drawing.Size(300, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(291, 52);
			this.label2.TabIndex = 14;
			this.label2.Text = "Click Finish to close Setup and reboot later, or click Reboot Now to close the se" +
    "tup and reboot right away. Please make sure everything that you have open is sav" +
    "ed to avoid data loss. ";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(207, 150);
			this.label3.MaximumSize = new System.Drawing.Size(300, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(299, 52);
			this.label3.TabIndex = 15;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// SetupNeedsRestartToFinishPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lineLbl);
			this.Controls.Add(this.showReleaseNotesBox);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.ForeColor = System.Drawing.Color.Silver;
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "SetupNeedsRestartToFinishPage";
			this.Size = new System.Drawing.Size(541, 340);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox showReleaseNotesBox;
        private System.Windows.Forms.Label lineLbl;
        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
	}
}
