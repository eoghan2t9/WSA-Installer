using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WSA_Installer.Pages;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.AxHost;

namespace WSA_Installer.Logic
{
	public enum UserDataPackageType
	{
		Default,
		Custom
	}

	// Class to hold all installation options
	public class WsaInstallationOptions
	{
		public UserDataPackageType PackageType { get; set; }
		public string CustomPackagePath { get; set; }
		public bool InstallAdditionalTools { get; set; }
	}

	public class Installer
	{
		public static Installer Instance;

		private string _installLocation = string.Empty;
		public string InstallLocation
		{
			get
			{
				return _installLocation;
			}
			set
			{
				_installLocation = value;
			}
		}

		public delegate void _installationFinishedDel(object sender, EventArgs e);

		private _installationFinishedDel _installationFinished;
		public event _installationFinishedDel InstallationFinished
		{
			add { _installationFinished += value; }
			remove { _installationFinished -= value; }
		}

		private UninstallerManager uninstallerManager;
		private Guid UninstallGuid { get; set; }
		private string UninstallRegKeyPath
		{
			get
			{
				return @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
			}
		}

		public string DisplayName
		{
			get
			{
				return "WSA";
			}
		}

		private bool _isInstalled = false;
		public bool IsInstalled
		{
			get
			{
				return _isInstalled;
			}
			set
			{
				_isInstalled = value;
			}
		}

		public int InstallProgress = 0;

		public WsaInstallationOptions InstallationOptions { get; set; } = null;

		public bool RestartNeeded = false;


		public Installer()
		{
			InstallationOptions = new WsaInstallationOptions
			{
				PackageType = UserDataPackageType.Default,
				CustomPackagePath = null, // Not needed since we chose Default
				InstallAdditionalTools = false
			};

			if (Instance == null)
			{
				Instance = this;
			}
			uninstallerManager = new UninstallerManager();
			_isInstalled = uninstallerManager.IsRegistered == true && (uninstallerManager.UninstallGuid != Guid.Empty);

			if (IsInstalled == true)
			{
				UninstallGuid = uninstallerManager.UninstallGuid;
				InstallLocation = uninstallerManager.InstallLocation;
			}
		}

		public async Task Install(List<WSAPackage> packages)
		{
			// Need to put our installation code for WSA here
			uninstallerManager.InstallLocation = this.InstallLocation;

			// Extract any packages we need
			foreach (WSAPackage pkg in packages)
			{
				string firstUrl = "";
				foreach (string url in pkg.PackageParts)
				{
					if (url.Contains(".001") || url.EndsWith(".zip"))
					{
						firstUrl = url;
						break;
					}
				}

				string extractionDir = "";
				switch (pkg.PackageName)
				{
					case "Stock WSA":
					{
						extractionDir = this.InstallLocation;

						break;
					}
					case "Default WSA User Data Pack":
					{
						//continue;
						extractionDir = $"C:\\Users\\{Environment.UserName}\\AppData\\Local\\Packages\\MicrosoftCorporationII.WindowsSubsystemForAndroid_8wekyb3d8bbwe";

						break;
					}
					case "GApps Integration":
					{
						continue;
						extractionDir = this.InstallLocation;
						break;
					}
					case "Aow Tools":
					{
						// Ask user for install dir for Aow Tools
						extractionDir = this.InstallLocation + @"\AdditionalTools\Aow Tools";
						break;
					}
				}


				string fileName = UrlUtilities.GetFilenameFromUrl(firstUrl);
				ExtractArchives ext = new ExtractArchives(extractionDir, fileName, pkg.PackageName);
				ext.ExtractionFinished += Ext_ExtractionFinished;
				ext.UpdateDetails += Ext_UpdateDetails;
				ext.UpdateProgress += Ext_UpdateProgress;
				ext.UpdateStatus += Ext_UpdateStatus;
				await ext.Extract_WSA();

				// Create shortcuts for each wsa app that has been extracted via the user data package.
				if (extractionDir == $"C:\\Users\\{Environment.UserName}\\AppData\\Local\\Packages\\MicrosoftCorporationII.WindowsSubsystemForAndroid_8wekyb3d8bbwe")
				{
					string[] icoFiles = Directory.GetFiles(extractionDir + @"\LocalState", "*.ico");
					Dictionary<string, string> packageToAppNames = new Dictionary<string, string>();
					packageToAppNames.Add("com.android.settings", "Android Settings");
					packageToAppNames.Add("app.revanced.android.gms", "MicroG Settings");
					packageToAppNames.Add("com.android.vending", "Play Store");
					packageToAppNames.Add("app.rvx.android.apps.youtube.music", "RVX Music");
					packageToAppNames.Add("app.rvx.android.youtube", "RVX");
					packageToAppNames.Add("nextapp.fx", "FX File Explorer");
					packageToAppNames.Add("app.rvx.manager.flutter", "RVX Manager");

					foreach (string iconFile in icoFiles)
					{
						string appPackageName = Path.GetFileNameWithoutExtension(iconFile);

						if (appPackageName == "com.amazon.venezia") continue;
						if (appPackageName == "com.google.android.gms") continue;

						string appDisplayName = "";

						if (packageToAppNames.ContainsKey(appPackageName))
						{
							appDisplayName = packageToAppNames[appPackageName];
						}


						// The full path where the shortcut (.lnk file) will be created.
						// Environment.GetFolderPath(Environment.SpecialFolder.Desktop) gets the current user's desktop path.
						string shortcutLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), appPackageName);

						// The target the shortcut will point to.
						string targetPath = $"C:\\Users\\{Environment.UserName}\\AppData\\Local\\Microsoft\\WindowsApps\\MicrosoftCorporationII.WindowsSubsystemForAndroid_8wekyb3d8bbwe\\WsaClient.exe";

						// The command-line arguments to be passed to the target.
						string arguments = @"/launch wsa://" + appPackageName;

						ShellLink.Shortcut shortcut = ShellLink.Shortcut.CreateShortcut(targetPath);

						shortcut.StringData = new ShellLink.Structures.StringData();
						shortcut.StringData.IconLocation = iconFile;
						shortcut.StringData.CommandLineArguments = arguments;

						shortcut.IconIndex = 0;

						if (!Directory.Exists($"C:\\Users\\{Environment.UserName}\\Desktop\\WSA Apps"))
						{
							Directory.CreateDirectory($"C:\\Users\\{Environment.UserName}\\Desktop\\WSA Apps");
						}

						if (appDisplayName != string.Empty)
						{
							shortcut.StringData.NameString = appDisplayName;
							shortcut.WriteToFile($"C:\\Users\\{Environment.UserName}\\Desktop\\WSA Apps\\" + appDisplayName + ".lnk");
						}
						else
						{
							shortcut.StringData.NameString = appPackageName;
							shortcut.WriteToFile($"C:\\Users\\{Environment.UserName}\\Desktop\\WSA Apps\\" + appPackageName + ".lnk");
						}
					}
				}
			}

			// Deleting these 3 files allows us to side load the WSA appx package once extracted.
			if (File.Exists(InstallLocation + @"\[Content_Types].xml"))
			{
				File.Delete(InstallLocation + @"\[Content_Types].xml");
			}

			if (File.Exists(InstallLocation + @"\AppxBlockMap.xml"))
			{
				File.Delete(InstallLocation + @"\AppxBlockMap.xml");
			}

			if (File.Exists(InstallLocation + @"\AppxSignature.p7x"))
			{
				File.Delete(InstallLocation + @"\AppxSignature.p7x");
			}

			VCRedistInstaller vCRedistInstaller = new VCRedistInstaller();
			vCRedistInstaller.UpdateDetails += Ext_UpdateDetails;
			vCRedistInstaller.UpdateProgress += Ext_UpdateProgress;
			vCRedistInstaller.UpdateStatus += Ext_UpdateStatus;
			await vCRedistInstaller.InstallRuntimes(this.InstallLocation + @"\Visual-C-Runtimes-All-in-One-Mar-2025");

			// We need to build a way to detect if any of these needed changed will require a restart.
			// IF any single thing needs to be changed,  then we tell the system that we need to restart. Otherwise,  run the rest of the installation code. 
			// Add status and details updates to all of these
			Ext_UpdateProgress(0);

			// Enable Virtualization features
			Ext_UpdateDetails("Enabling Virtualization features");
			Ext_UpdateStatus("Enabling Virtualization features...");
			FeatureEnabler ftEnabler = new FeatureEnabler();
			ftEnabler.UpdateDetails += Ext_UpdateDetails;
			ftEnabler.UpdateProgress += Ext_UpdateProgress;
			ftEnabler.UpdateStatus += Ext_UpdateStatus;

			bool restartRequired = ftEnabler.EnsureVirtualizationFeaturesEnabled();
			if (restartRequired)
			{
				Ext_UpdateProgress(25);
			}
			else
			{
				Ext_UpdateProgress(50);
			}
			Thread.Sleep(500);


			// Enable developer mode and setup the restart for the installer.
			if (!DeveloperModeManager.IsDeveloperModeEnabled)
			{
				Ext_UpdateDetails("Enabling Developer mode");
				Ext_UpdateStatus("Enabling Developer mode...");
				DeveloperModeManager.EnableDeveloperMode();

				restartRequired = true;
			}
			if (restartRequired)
			{
				Ext_UpdateProgress(50);
				Thread.Sleep(500);
			}
			else
			{
				Ext_UpdateProgress(100);
			}

			if (restartRequired)
			{
				Ext_UpdateDetails("Disabling Disable Autorun Delay");
				Ext_UpdateStatus("Disabling Autorun Delay...");
				AutorunDelayManager.DisableAutorunDelay();
				Ext_UpdateProgress(75);
				Thread.Sleep(500);

				Ext_UpdateDetails("Setting installer to auto run on next reboot");
				Ext_UpdateStatus("Setting installer to auto run on next reboot...");
				RunOnceInstaller.SetRunOnce("WSA Installer", "-i");
				Ext_UpdateProgress(100);
				Thread.Sleep(500);
			}

			RestartNeeded = restartRequired;
			Form1.Instance.RestartNeeded = restartRequired;

			if (restartRequired)
			{
				InstallerState installerState = new InstallerState();
				installerState.InstallationOptions = this.InstallationOptions;
				installerState.SelectedPackages = packages;
				installerState.InstallationDirectory = this.InstallLocation;

				string stateJson = JsonConvert.SerializeObject(installerState, Formatting.Indented);
				string assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				File.WriteAllText(assemblyDirectory + @"\InstallerState.json", stateJson);
			}
			else
			{
				InstallerState installerState = new InstallerState();
				installerState.InstallationOptions = this.InstallationOptions;
				installerState.SelectedPackages = packages;
				installerState.InstallationDirectory = this.InstallLocation;
				await PostRestartInstall(installerState);

				string assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				Directory.Delete(assemblyDirectory + @"\Temp", true);
			}

			finishEvent();
		}

		public async Task PostRestartInstall(InstallerState state)
		{
			await Task.Run(() =>
			{

				Ext_UpdateStatus("Installing Appx packages...");
				Ext_UpdateProgress(0);

				// Do our appx register and appx package installation scripts here. 
				WsaInstaller installer = new WsaInstaller(state.InstallationDirectory, state.InstallationDirectory + "\\AdditionalTools\\Aow Tools");

				bool wsaInstalled = installer.InstallWsa();
				bool additionalToolsInstalled = false;

				if (!wsaInstalled)
				{
					Ext_UpdateDetails("WSA Appx Package failed to registered! See appxLog.log for details");
				}
				else
				{
					Ext_UpdateDetails("WSA Appx Package Registered");
				}

				if (state.InstallationOptions.InstallAdditionalTools)
				{
					Ext_UpdateProgress(50);

					additionalToolsInstalled = installer.InstallAowTools();

					if (!additionalToolsInstalled)
					{
						Ext_UpdateDetails("Aow Tools Appx Package failed to registered! See appxLog.log for details");
					}
					else
					{
						Ext_UpdateDetails("Aow Tools Appx Package Registered");
					}

					Ext_UpdateProgress(100);
				}
				else
				{
					Ext_UpdateProgress(100);
				}

				//uninstallerManager.CreateUninstaller();

				finishEvent();
			});
		}

		private void Ext_UpdateStatus(string status)
		{
			if (Form1.Instance != null)
			{
				if (Form1.Instance.InvokeRequired)
				{
					Form1.Instance.Invoke(new UpdateEventDels.UpdateStatusDel(Ext_UpdateStatus), status);
				}
				else
				{
					InstallingPage pg = (InstallingPage)Form1.Instance.pages[3];
					pg.statusLbl.Text = status;
				}
			}
		}

		private void Ext_UpdateProgress(int prog)
		{
			if (Form1.Instance != null)
			{
				if (Form1.Instance.InvokeRequired)
				{
					Form1.Instance.Invoke(new UpdateEventDels.UpdateProgDel(Ext_UpdateProgress), prog);
				}
				else
				{
					InstallProgress += 1;
					InstallingPage pg = (InstallingPage)Form1.Instance.pages[3];
					pg.progressBar1.Value = prog;
				}
			}
		}

		private void Ext_UpdateDetails(string details)
		{
			if (Form1.Instance != null)
			{
				if (Form1.Instance.InvokeRequired)
				{
					Form1.Instance.Invoke(new UpdateEventDels.UpdateDetailsDel(Ext_UpdateDetails), details);
				}
				else
				{
					InstallingPage pg = (InstallingPage)Form1.Instance.pages[3];
					pg.detailsBox.Items.Add(details);
					pg.detailsBox.SelectedIndex = pg.detailsBox.Items.Count - 1;
				}
			}
		}

		private void Ext_ExtractionFinished(object sender, EventArgs e)
		{
			Ext_UpdateProgress(0);
			Ext_UpdateDetails("Registering Uninstaller");
			Ext_UpdateStatus("Registering Uninstaller...");
			//uninstallerManager.CreateUninstaller();

			Ext_UpdateDetails("Registering GitSE shell extension");
			Ext_UpdateStatus("Registering shell extension...");
			Ext_UpdateProgress(0);
			//RegisterShellExtension();

			Ext_UpdateStatus("Installation completed");
			finishEvent();
		}

		public delegate void finishEventDel();
		private void finishEvent()
		{
			if (Form1.Instance != null)
			{
				if (Form1.Instance.InvokeRequired)
				{
					Form1.Instance.Invoke(new finishEventDel(finishEvent));
				}
				else
				{
					if (_installationFinished != null)
					{
						InstallingPage pg = (InstallingPage)Form1.Instance.pages[3];
						Form1.frmSpinner.Stop();
						_installationFinished(this, new EventArgs());
					}
				}
			}
		}
	}
}
