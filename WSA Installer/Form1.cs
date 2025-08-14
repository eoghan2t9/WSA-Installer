using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WSA_Installer.Logic;
using WSA_Installer.Pages;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WSA_Installer
{
	public partial class Form1 : Form
	{
		public static Form1 Instance = null;
		public static FormSpinner frmSpinner = null;

		public Installer Installer = null;
		static string appPath = Path.GetDirectoryName(Application.ExecutablePath);
		static string _7z86Path = appPath + @"\7z-x86.dll";
		static string _7z64Path = appPath + @"\7z-x64.dll";
		static string _SevenSharpPath = appPath + @"\SevenZipSharp.dll";

		private int pageIdx = 0;
		public Control[] pages = null;
		private Control currentPage = null;

		private Banner bannerBox = null;

		private Guid InstalledGuid = Guid.Empty;

		private PackageGetter pkgGetter;
		private WSAPackages wsaPackages;

		private bool InstallFinishingAfterReboot = false;
		private bool LogToFile = false;

		private SetupNeedsRestartToFinishPage needRestartPage;

		public bool RestartNeeded = false;

		public Form1(string[] args)
		{
			if (args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i] == "-i")
					{
						InstallFinishingAfterReboot = true;
					}
					if (args[i] == "-l")
					{
						LogToFile = true;
					}
				}
			}

			InitializeComponent();
			Form1.Instance = this;
			Installer = new Installer();
			Installer.InstallationFinished += Installer_InstallationFinished;
			this.FormClosing += Form1_FormClosing;
			frmSpinner = new FormSpinner(this);

			createLibs();

			bannerBox = new Banner();
			bannerBox.Visible = true;


			pages = new Control[]
			{
				new WelcomePage(),
				new ChooseInstallFolder(bannerBox),
				new ChooseOptions(bannerBox),
				new InstallingPage(bannerBox),
				//new UninstallerTesterPage(bannerBox),
				//new ComfirmUninstall(bannerBox),
				new SetupFinishedPage()
			};

			needRestartPage = new SetupNeedsRestartToFinishPage();
			Utils.ThemeAllControls(needRestartPage);

			foreach (Control page in pages)
			{
				Utils.ThemeAllControls(page);
			}

			if (Installer.IsInstalled == true)
			{
				pageIdx = pages.Length - 2;
			}

			pkgGetter = new PackageGetter();

			using (HttpClient client = new HttpClient())
			{
				// Blocking call: .Result waits synchronously for the task to complete
				string content = client.GetStringAsync("https://github.com/mastercodeon31415/WSA-Installer/raw/refs/heads/main/Installer-Data/Packages.json").Result;
				wsaPackages = JsonConvert.DeserializeObject<WSAPackages>(content);
			}

			if (InstallFinishingAfterReboot)
			{
				pageIdx = 3;
				if (this.currentPage != null)
				{
					this.Controls.Remove(this.currentPage);
				}

				this.currentPage = pages[this.pageIdx];
				this.Controls.Add(pages[this.pageIdx]);

				Page pg = (Page)this.currentPage;
				pg.ChangeBannerText();

				updatePage();
			}
			else
			{
				updatePage();
			}

			Utils.ThemeAllControls(this);

			//Task.Run(async () => { updatePage(); });
			Utils.ThemeAllControls(this);
			Utils.ThemeAllControls(this);
			Utils.ThemeAllControls(this);
			Utils.ThemeAllControls(this);

			foreach (Control ctrl in this.Controls)
			{
				ctrl.BackColor = this.BackColor;
				ctrl.ForeColor = this.ForeColor;
			}

			bannerBox.BackColor = this.BackColor;
			bannerBox.ForeColor = this.ForeColor;

			DarkModeTitleBar.Enable(this);
		}

		private async void Installer_InstallationFinished(object sender, EventArgs e)
		{
			if (RestartNeeded)
			{
				setNeedsRestartPage();
			}
			else
			{
				if (this.pageIdx + 1 < pages.Length)
				{
					this.nextBtn.Text = "Finish";
					this.nextBtn.Enabled = true;
					this.backBtn.Visible = false;

					//this.cancelBtn.Enabled = false;

					this.cancelBtn.Visible = false;
					this.nextBtn.Location = cancelBtn.Location;
					this.ControlBox = true;

					lineLbl.Visible = true;
					if (this.Controls.Contains(bannerBox))
					{
						this.Controls.Remove(bannerBox);
					}

					this.currentPage.Location = new Point(12, 12);

					// Goto SetupFinishedPage
					pageIdx = pages.Length - 1;

					// Uncomment this line and comment the one above to go to the uninstaller testing page once the install is finished
					// pageIdx += 1;
				}

				await updatePage();

				SetupFinishedPage setupFinishedPage = (SetupFinishedPage)this.currentPage;
				setupFinishedPage.IsRestartNeeded = false;
			}
		}

		private void setNeedsRestartPage()
		{
			if (this.currentPage != null)
			{
				this.Controls.Remove(this.currentPage);
			}

			this.currentPage = needRestartPage;
			this.Controls.Add(needRestartPage);

			this.nextBtn.Text = "Finish";
			this.nextBtn.Enabled = true;
			this.backBtn.Text = "Reboot Now";
			this.backBtn.Enabled = true;
			this.ControlBox = true;

			lineLbl.Visible = true;
			if (this.Controls.Contains(bannerBox))
			{
				this.Controls.Remove(bannerBox);
			}

			this.currentPage.Location = new Point(12, 12);
		}

		private async void nextBtn_Click(object sender, EventArgs e)
		{
			if (currentPage.Tag == (object)"InstallerPage")
			{
				if (InstallFinishingAfterReboot)
				{
					pageIdx = pages.Length - 1;
					bannerBox.Visible = false;
					await updatePage();

					SetupFinishedPage setupFinishedPage = (SetupFinishedPage)this.currentPage;
					setupFinishedPage.IsRestartNeeded = false;
				}
				else
				{
					pageIdx = pages.Length - 1;
					bannerBox.Visible = false;
					await updatePage();

					setNeedsRestartPage();
					return;
				}
			}

			if (nextBtn.Text == "Finish")
			{
				this.Close();
			}

			if (currentPage.Tag == (object)"ChooseOptions")
			{
				ChooseOptions pg = (ChooseOptions)currentPage;
				Installer.InstallationOptions = pg.GenerateOptions();
			}

			if (currentPage.Tag == (object)"ChooseInstallFolder")
			{
				ChooseInstallFolder chooseDirPg = (ChooseInstallFolder)this.currentPage;

				if (chooseDirPg.destinationBox.Text == @"C:\")
				{
					DarkModeMessageBox.Show(this, "Can't install to the root of C drive! (C:\\)\nPlease select a proper installation directory", "WSA Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);

					return;
				}

				//if (pg.SkipToUninstallerTesting == true)
				//{
				//	pageIdx = 3;
				//	await updatePage();
				//	return;
				//}
			}
			if (this.pageIdx + 1 < pages.Length)
			{
				this.pageIdx += 1;
			}

			await updatePage();
		}

		private async void backBtn_Click(object sender, EventArgs e)
		{
			if (this.backBtn.Text == "Reboot Now")
			{
				Process.Start("shutdown", "/r /t 0");
			}
			else
			{
				if (this.pageIdx - 1 >= 0)
				{
					this.pageIdx -= 1;
				}

				await updatePage();
			}
		}

		private async Task updatePage()
		{
			if (this.currentPage != null)
			{
				this.Controls.Remove(this.currentPage);
			}

			this.currentPage = pages[this.pageIdx];
			//this.currentPage.Location = new Point(12, 12);
			this.Controls.Add(pages[this.pageIdx]);

			Page pg = (Page)this.currentPage;
			pg.ChangeBannerText();

			switch (this.currentPage.Tag)
			{
				case "WelcomePage":
				{
					this.backBtn.Visible = false;
					this.nextBtn.Text = "Next >";
					if (this.Controls.Contains(bannerBox)) this.Controls.Remove(bannerBox);
					this.ControlBox = true;
					break;
				}

				case "SetupFinishedPage":
				{
					this.nextBtn.Text = "Finish";
					this.backBtn.Text = "Reboot Now";
					this.backBtn.Enabled = true;
					lineLbl.Visible = true;
					if (this.Controls.Contains(bannerBox))
					{
						this.Controls.Remove(bannerBox);
					}

					this.currentPage.Location = new Point(12, 12);
					break;
				}

				case "ChooseInstallFolder":
				{
					this.nextBtn.Text = "Next >";
					this.nextBtn.Enabled = true;
					this.backBtn.Enabled = true;
					this.cancelBtn.Enabled = true;
					this.ControlBox = true;
					break;
				}

				case "ChooseOptions":
				{
					this.nextBtn.Text = "Install";
					this.nextBtn.Enabled = true;
					this.backBtn.Enabled = true;
					this.cancelBtn.Enabled = true;
					this.ControlBox = true;
					break;
				}

				case "InstallerPage":
				{
					this.nextBtn.Text = "Next >";
					this.nextBtn.Enabled = false;
					this.backBtn.Enabled = false;

					if (((Page)currentPage).NoBanner == true)
					{
						if (this.Controls.Contains(bannerBox))
						{
							this.Controls.Remove(bannerBox);
						}
						this.currentPage.Location = new Point(0, 0);

					}
					else
					{
						this.lineLbl.Visible = true;
						this.backBtn.Visible = true;
						this.currentPage.Location = new Point(12, 80);
						if (!this.Controls.Contains(bannerBox)) this.Controls.Add(bannerBox);
					}

					if (InstallFinishingAfterReboot)
					{
						InstallingPage instPg = (InstallingPage)pg;
						instPg.lblBytes.Visible = false;
						instPg.lblProgressPercentage.Visible = false;
						instPg.lblSpeed.Visible = false;
						instPg.lblTotalFiles.Visible = false;

						string assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
						InstallerState state = JsonConvert.DeserializeObject<InstallerState>(File.ReadAllText(assemblyDirectory + @"\InstallerState.json"));

						await Installer.PostRestartInstall(state);

						File.Delete(assemblyDirectory + @"\InstallerState.json");

						// This deletes the cached files!!!
						//Directory.Delete(assemblyDirectory + @"\Temp", true);

						break;
					}
					else
					{
						List<WSAPackage> pkgList = new List<WSAPackage>();

						pkgList.Add(wsaPackages.WSA_Stock);

						if (Installer.InstallationOptions.PackageType == UserDataPackageType.Default)
						{
							pkgList.Add(wsaPackages.UserDataPackages[0]);
						}

						if (Installer.InstallationOptions.InstallAdditionalTools)
						{
							pkgList.Add(wsaPackages.ToolsPackages[0]);
						}

						string stagingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Temp";
						List<string> pkgFileNames = new List<string>();

						foreach (WSAPackage pkg in pkgList)
						{
							List<string> tempFileNames = new List<string>();
							List<string> neededFileNames = new List<string>();
							pkg.PartsToDownload = new List<string>();

							foreach (string pkgUrl in pkg.PackageParts)
							{
								string[] parts = pkgUrl.Split('/');
								string fileName = Uri.UnescapeDataString(parts[parts.Length - 1]);
								tempFileNames.Add(fileName);

								if (!File.Exists(stagingDirectory + @"\" + fileName))
								{
									neededFileNames.Add(fileName);
									pkg.PartsToDownload.Add(pkgUrl);
								}
							}
						}

						// NO CODE WILL RUN UNTIL THIS DOWNLOADING LINE HAS FINISHED DOWNLOADING!
						await pkgGetter.Download(pkgList);

						/* The download is cancelable, but once that is done, we take away the user's ability to cancel.
						   This ensures that the user cant cancel the install mid operation and fuck shit up. 
						   Also take away the control box so they cant close it. Still can minimize via Win+M or from the task bar
						*/
						this.cancelBtn.Enabled = false;
						this.ControlBox = false; // ??? Might keep this, dont know

						InstallingPage instPg = (InstallingPage)pg;
						instPg.lblBytes.Visible = false;
						instPg.lblProgressPercentage.Visible = false;
						instPg.lblSpeed.Visible = false;
						instPg.lblTotalFiles.Visible = false;

						// Save package list
						// Save Installation options
						// Save installation directory


						// NO CODE WILL RUN UNTIL THIS INSTALLING LINE HAS FINISHED INSTALLING!
						await Installer.Install(pkgList);
					}

					break;
				}

				case "ComfirmUninstallPage":
				{
					this.nextBtn.Text = "Uninstall";
					this.nextBtn.Enabled = true;
					this.backBtn.Visible = false;
					this.cancelBtn.Enabled = true;
					break;
				}

				case "UninstallerTesting":
				{
					this.nextBtn.Text = "Next >";
					this.nextBtn.Enabled = false;
					this.backBtn.Enabled = false;
					this.cancelBtn.Enabled = true;
					break;
				}
			}

			if (((Page)currentPage).NoBanner == true)
			{
				if (this.Controls.Contains(bannerBox))
				{
					this.Controls.Remove(bannerBox);
				}
				this.currentPage.Location = new Point(0, 0);

			}
			else
			{
				this.lineLbl.Visible = true;
				this.backBtn.Visible = true;
				this.currentPage.Location = new Point(12, 80);
				if (!this.Controls.Contains(bannerBox)) this.Controls.Add(bannerBox);
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.currentPage.Tag != (object)"SetupFinishedPage" && this.currentPage.Tag != (object)"SetupNeedsRestartToFinishPage")
			{
				if (DarkModeMessageBox.Show(this, "Are you sure you want to quit WSA Setup?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
				{
					deleteLibs();
				}
				else
				{
					e.Cancel = true;
				}
			}
			else
			{
				deleteLibs();
			}
		}

		private void createLibs()
		{
			if (Environment.Is64BitProcess == true)
			{
				File.WriteAllBytes(_7z64Path, WSA_Installer.Properties.Resources._7z_x64);
			}
			else
			{
				File.WriteAllBytes(_7z86Path, WSA_Installer.Properties.Resources._7z_x86);
			}
			File.WriteAllBytes(_SevenSharpPath, WSA_Installer.Properties.Resources.SevenZipSharp);
		}

		private void deleteLibs()
		{
			string appPath = Path.GetDirectoryName(Application.ExecutablePath);
			string _7z86Path = appPath + @"\7z-x86.dll";
			string _7z64Path = appPath + @"\7z-x64.dll";
			string _SevenSharpPath = appPath + @"\SevenZipSharp.dll";
			if (Environment.Is64BitProcess == true)
			{
				if (File.Exists(_7z64Path) == true) File.Delete(_7z64Path);
			}
			else
			{
				if (File.Exists(_7z86Path) == true) File.Delete(_7z86Path);
			}

			if (File.Exists(_SevenSharpPath) == true)
			{
				Process.Start(new ProcessStartInfo()
				{
					Arguments = "/C choice /C Y /N /D Y /T 1 & Del \"" + _SevenSharpPath + "\"",
					WindowStyle = ProcessWindowStyle.Hidden,
					CreateNoWindow = true,
					FileName = "cmd.exe"
				});
			}
		}

		private void cancelBtn_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		protected override void WndProc(ref Message m)
		{
			DarkModeTitleBar.WndProc(ref m);
			base.WndProc(ref m);
		}
	}
}
