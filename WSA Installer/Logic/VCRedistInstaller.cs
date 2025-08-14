using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace WSA_Installer.Logic
{
	public class VCRedistInstaller
	{
		private UpdateEventDels.UpdateDetailsDel _updateDetails;
		public event UpdateEventDels.UpdateDetailsDel UpdateDetails
		{
			add
			{
				_updateDetails += value;
			}

			remove
			{
				_updateDetails -= value;
			}
		}



		private UpdateEventDels.UpdateStatusDel _updateStatus;
		public event UpdateEventDels.UpdateStatusDel UpdateStatus
		{
			add
			{
				_updateStatus += value;
			}

			remove
			{
				_updateStatus -= value;
			}
		}



		private UpdateEventDels.UpdateProgDel _updateProg;
		public event UpdateEventDels.UpdateProgDel UpdateProgress
		{
			add
			{
				_updateProg += value;
			}

			remove
			{
				_updateProg -= value;
			}
		}

		public async Task InstallRuntimes(string path)
		{
			//if (_updateDetails != null) _updateDetails("\nMicrosoft Visual C++ All-In-One Runtimes by W1zzard @ TechPowerUp");
			//if (_updateDetails != null) _updateDetails("https://www.techpowerup.com/download/visual-c-redistributable-runtime-package-all-in-one/\n");
			if (_updateDetails != null) _updateDetails("Installing runtime packages...\n");
			//string appPath = AppDomain.CurrentDomain.BaseDirectory;

			if (_updateDetails != null) _updateDetails("Installing runtime packages...\n");

			bool is64Bit = Environment.Is64BitOperatingSystem;


			if (is64Bit)
			{
				if (_updateDetails != null) _updateDetails("Installing 64-bit runtimes and 32-bit runtimes");
				await InstallRuntimesX64(path);
			}
			else
			{
				if (_updateDetails != null) _updateDetails("Installing 32-bit runtimes");
				await InstallRuntimesX86(path);
			}

			if (_updateDetails != null) _updateDetails("\nInstallation completed successfully");
		}

		private async Task InstallRuntimesX86(string path)
		{
			if (_updateDetails != null) _updateDetails("2005...");
			await ExecuteProcess(Path.Combine(path, "vcredist2005_x86.exe"), "/q");

			if (_updateDetails != null) _updateDetails("2008...");
			await ExecuteProcess(Path.Combine(path, "vcredist2008_x86.exe"), "/qb");

			if (_updateDetails != null) _updateDetails("2010...");
			await ExecuteProcess(Path.Combine(path, "vcredist2010_x86.exe"), "/passive /norestart");

			if (_updateDetails != null) _updateDetails("2012...");
			await ExecuteProcess(Path.Combine(path, "vcredist2012_x86.exe"), "/passive /norestart");

			if (_updateDetails != null) _updateDetails("2013...");
			await ExecuteProcess(Path.Combine(path, "vcredist2013_x86.exe"), "/passive /norestart");

			if (_updateDetails != null) _updateDetails("2015 - 2022...");
			await ExecuteProcess(Path.Combine(path, "vcredist2015_2017_2019_2022_x86.exe"), "/passive /norestart");
		}

		private async Task InstallRuntimesX64(string path)
		{
			if (_updateDetails != null) _updateDetails("2005...");
			await ExecuteProcess(Path.Combine(path, "vcredist2005_x86.exe"), "/q");
			await ExecuteProcess(Path.Combine(path, "vcredist2005_x64.exe"), "/q");

			if (_updateDetails != null) _updateDetails("2008...");
			await ExecuteProcess(Path.Combine(path, "vcredist2008_x86.exe"), "/qb");
			await ExecuteProcess(Path.Combine(path, "vcredist2008_x64.exe"), "/qb");

			if (_updateDetails != null) _updateDetails("2010...");
			await ExecuteProcess(Path.Combine(path, "vcredist2010_x86.exe"), "/passive /norestart");
			await ExecuteProcess(Path.Combine(path, "vcredist2010_x64.exe"), "/passive /norestart");

			if (_updateDetails != null) _updateDetails("2012...");
			await ExecuteProcess(Path.Combine(path, "vcredist2012_x86.exe"), "/passive /norestart");
			await ExecuteProcess(Path.Combine(path, "vcredist2012_x64.exe"), "/passive /norestart");

			if (_updateDetails != null) _updateDetails("2013...");
			await ExecuteProcess(Path.Combine(path, "vcredist2013_x86.exe"), "/passive /norestart");
			await ExecuteProcess(Path.Combine(path, "vcredist2013_x64.exe"), "/passive /norestart");

			if (_updateDetails != null) _updateDetails("2015 - 2022...");
			await ExecuteProcess(Path.Combine(path, "vcredist2015_2017_2019_2022_x86.exe"), "/passive /norestart");
			await ExecuteProcess(Path.Combine(path, "vcredist2015_2017_2019_2022_x64.exe"), "/passive /norestart");
		}

		private async Task ExecuteProcess(string fileName, string arguments)
		{
			await Task.Run(() =>
			{
				try
				{
					ProcessStartInfo startInfo = new ProcessStartInfo(fileName, arguments)
					{
						UseShellExecute = false,
						CreateNoWindow = true,
						WindowStyle = ProcessWindowStyle.Hidden
					};

					using (Process process = Process.Start(startInfo))
					{
						process.WaitForExit();


					}
				}
				catch (Exception ex)
				{
					if (_updateDetails != null) _updateDetails($"Error executing {fileName}: {ex.Message}");
				}
			});
		}
	}
}
