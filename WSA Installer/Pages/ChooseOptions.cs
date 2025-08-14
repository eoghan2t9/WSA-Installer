using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using WSA_Installer.Logic;

namespace WSA_Installer.Pages
{
    public partial class ChooseOptions : Page
    {
        private DriveInfo _driveInfo;
        public string InstallLocation = "";
        public bool SkipToUninstallerTesting = false;
        public ChooseOptions(Banner prntBanner) : base(prntBanner)
        {
            InitializeComponent();
            this.Tag = "ChooseOptions";
            this.NoBanner = false;
        }

        public override void ChangeBannerText()
        {
            parentBanner.headerLbl.Text = "Select Installation Options";
            parentBanner.subTextLbl.Text = "Choose what options to be included with the WSA installation.";
        }

        private void wsaDefaultUserDataRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            //Form1.Instance.Installer.InstallationOptions.PackageType = UserDataPackageType.Default;
            label1.Enabled = false;
            wsaBrowseCustomUserDataPackageBox.Enabled = false;
            wsaCustomWsaUserDataPackageBox.Enabled = false;
        }

        private void wsaCustomUserDataRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            //Form1.Instance.Installer.InstallationOptions.PackageType = UserDataPackageType.Custom;
            label1.Enabled = true;
            wsaBrowseCustomUserDataPackageBox.Enabled = true;
            wsaCustomWsaUserDataPackageBox.Enabled = true;
        }

        public WsaInstallationOptions GenerateOptions()
        {
            WsaInstallationOptions options = new WsaInstallationOptions();
            if (wsaDefaultUserDataRadioBtn.Checked)
            {
                options.PackageType = UserDataPackageType.Default;
            }

            if (wsaCustomUserDataRadioBtn.Checked)
            {
                options.PackageType = UserDataPackageType.Custom;
                
                if (wsaCustomWsaUserDataPackageBox.Text != String.Empty)
                {
                    if (File.Exists(wsaCustomWsaUserDataPackageBox.Text))
                    {
                        options.CustomPackagePath = wsaCustomWsaUserDataPackageBox.Text;
                    }
                    else
                    {
                        options.PackageType = UserDataPackageType.Default;
                        wsaDefaultUserDataRadioBtn.Checked = true;
                        wsaCustomUserDataRadioBtn.Checked = false;
                    }
                }
                else
                {
                    options.PackageType = UserDataPackageType.Default;
                    wsaDefaultUserDataRadioBtn.Checked = true;
                    wsaCustomUserDataRadioBtn.Checked = false;
                }
            }

            options.InstallAdditionalTools = wsaToolsBox.Checked;

            return options;
        }

        private void wsaBrowseCustomUserDataPackageBox_Click(object sender, EventArgs e)
        {
            // Use a 'using' statement to ensure the dialog is properly disposed
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set the initial directory (optional, but good practice)
                openFileDialog.InitialDirectory = "c:\\";

                // This is the core of the filter functionality
                openFileDialog.Filter = "Archive Files (*.7z, *.zip)|*.7z;*.zip|7-Zip Files (*.7z)|*.7z|ZIP Files (*.zip)|*.zip|All files (*.*)|*.*";

                // Set the default filter to be the first one ("Archive Files")
                openFileDialog.FilterIndex = 1;

                // Restore the directory to the original directory before closing
                openFileDialog.RestoreDirectory = true;

                // Show the dialog and check if the user clicked "OK"
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    wsaCustomWsaUserDataPackageBox.Text = openFileDialog.FileName;

                    Form1.Instance.Installer.InstallationOptions.CustomPackagePath = openFileDialog.FileName;

                    // Get the path of specified file
                    string selectedFilePath = openFileDialog.FileName;

                    // Now you can use the file path for your operations
                    //MessageBox.Show($"File selected: {selectedFilePath}", "File Path");
                }
            }
        }

		private void wsaToolsBox_CheckedChanged(object sender, EventArgs e)
		{

		}
	}
}
