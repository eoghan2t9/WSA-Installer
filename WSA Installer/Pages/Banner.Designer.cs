namespace WSA_Installer.Pages
{
    partial class Banner
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
			this.subTextLbl = new System.Windows.Forms.Label();
			this.headerLbl = new System.Windows.Forms.Label();
			this.logoPicture = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.logoPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// subTextLbl
			// 
			this.subTextLbl.AutoSize = true;
			this.subTextLbl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.subTextLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.subTextLbl.ForeColor = System.Drawing.Color.Silver;
			this.subTextLbl.Location = new System.Drawing.Point(185, 42);
			this.subTextLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.subTextLbl.Name = "subTextLbl";
			this.subTextLbl.Size = new System.Drawing.Size(233, 15);
			this.subTextLbl.TabIndex = 12;
			this.subTextLbl.Text = "Choose the folder in which to install GitSE";
			// 
			// headerLbl
			// 
			this.headerLbl.AutoSize = true;
			this.headerLbl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.headerLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.headerLbl.ForeColor = System.Drawing.Color.Silver;
			this.headerLbl.Location = new System.Drawing.Point(167, 20);
			this.headerLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.headerLbl.Name = "headerLbl";
			this.headerLbl.Size = new System.Drawing.Size(157, 15);
			this.headerLbl.TabIndex = 11;
			this.headerLbl.Text = "Choose Install Location";
			// 
			// logoPicture
			// 
			this.logoPicture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.logoPicture.ForeColor = System.Drawing.Color.Silver;
			this.logoPicture.Image = global::WSA_Installer.Properties.Resources.banner3;
			this.logoPicture.Location = new System.Drawing.Point(0, 7);
			this.logoPicture.Name = "logoPicture";
			this.logoPicture.Size = new System.Drawing.Size(162, 72);
			this.logoPicture.TabIndex = 13;
			this.logoPicture.TabStop = false;
			// 
			// Banner
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
			this.Controls.Add(this.logoPicture);
			this.Controls.Add(this.subTextLbl);
			this.Controls.Add(this.headerLbl);
			this.ForeColor = System.Drawing.Color.Silver;
			this.Size = new System.Drawing.Size(575, 81);
			((System.ComponentModel.ISupportInitialize)(this.logoPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox logoPicture;
        public System.Windows.Forms.Label subTextLbl;
        public System.Windows.Forms.Label headerLbl;
    }
}
