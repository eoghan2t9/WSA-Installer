using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace WSA_Installer.Logic
{
	public class WsaInstaller
	{
		private readonly string _wsaInstallationPath;
		private readonly string _aowToolsInstallationPath;

		private FileLogger appxLog;

		/// <summary>
		/// Initializes a new instance of the WsaInstaller class with configurable installation paths.
		/// </summary>
		/// <param name="wsaInstallationPath">The root directory path for the WSA installation files.</param>
		/// <param name="aowToolsInstallationPath">The directory path for the Aow Tools installation files.</param>
		public WsaInstaller(string wsaInstallationPath, string aowToolsInstallationPath)
		{
			_wsaInstallationPath = wsaInstallationPath;
			_aowToolsInstallationPath = aowToolsInstallationPath;
			appxLog = new FileLogger("appxLog.txt");
		}

		/// <summary>
		/// Installs Windows Subsystem for Android (WSA).
		/// </summary>
		/// <returns>True if the installation was successful, otherwise false.</returns>
		public bool InstallWsa()
		{
			appxLog.Log("--------------------------------------------InstallWsa--------------------------------------------");
			appxLog.Log("Attempting to install WSA...");

			string manifestPath = Path.Combine(_wsaInstallationPath, "AppxManifest.xml");
			string[] dependencyPaths = new string[]
			{
			Path.Combine(_wsaInstallationPath, "Dependencies", "Microsoft.UI.Xaml.2.8_8.2501.31001.0_x64__8wekyb3d8bbwe.Appx"),
			Path.Combine(_wsaInstallationPath, "Dependencies", "Microsoft.VCLibs.140.00.UWPDesktop_14.0.33728.0_x64__8wekyb3d8bbwe.Appx"),
			Path.Combine(_wsaInstallationPath, "Dependencies", "Microsoft.VCLibs.140.00_14.0.33519.0_x64__8wekyb3d8bbwe.Appx")
			};

			string[] cmds = new string[]
			{
				$"Add-AppxPackage -Path '{dependencyPaths[0]}'",
				$"Add-AppxPackage -Path ' {dependencyPaths[1]} '",
				$"Add-AppxPackage -Path ' {dependencyPaths[2]} '",
				$"Add-AppxPackage -Register '{manifestPath}'"
			};

			//string cmd = $"{{ Add-AppxPackage -Path '{dependencyPaths[0]}'; Add-AppxPackage -Path '{dependencyPaths[1]}'; Add-AppxPackage -Path '{dependencyPaths[2]}'; Add-AppxPackage -Register '{manifestPath}'}}";

			bool res = ExecutePowerShellCommand(cmds[0]);
			res = ExecutePowerShellCommand(cmds[1]);
			res = ExecutePowerShellCommand(cmds[2]);
			res = ExecutePowerShellCommand(cmds[3]);
			return res;
		}

		/// <summary>
		/// Installs the Aow Tools.
		/// </summary>
		/// <returns>True if the installation was successful, otherwise false.</returns>
		public bool InstallAowTools()
		{
			appxLog.Log("--------------------------------------------InstallAowTools--------------------------------------------");
			appxLog.Log("Attempting to install Aow Tools...");

			string manifestPath = Path.Combine(_aowToolsInstallationPath, "7061touchwp.AowTools_1.6.5.0_x64__m9vp3t2f55f5t", "AppxManifest.xml");
			string[] dependencyPaths = new string[]
			{
			Path.Combine(_aowToolsInstallationPath, "Microsoft.NET.Native.Framework.2.2_2.2.29512.0_x64__8wekyb3d8bbwe.Appx"),
			Path.Combine(_aowToolsInstallationPath, "Microsoft.NET.Native.Runtime.2.2_2.2.28604.0_x64__8wekyb3d8bbwe.Appx")
			};

			string[] cmds = new string[]
			{
				$"Add-AppxPackage -Path '{dependencyPaths[0]}'",
				$"Add-AppxPackage -Path '{dependencyPaths[1]}'",
				$"Add-AppxPackage -Register '{manifestPath}'"
			};

			//string cmd = $"{{ Add-AppxPackage -Path '{dependencyPaths[0]}'; Add-AppxPackage -Path '{dependencyPaths[1]}'; Add-AppxPackage -Register '{manifestPath}'}}";

			bool res = ExecutePowerShellCommand(cmds[0]);
			res = ExecutePowerShellCommand(cmds[1]);
			res = ExecutePowerShellCommand(cmds[2]);
			return res;
		}

		private bool ExecutePowerShellCommand(string command)
		{
			appxLog.Log("--------------------------------------------ExecutePowerShellCommand--------------------------------------------");
			appxLog.Log("Executing: powershell.exe " + string.Format("-NoProfile -ExecutionPolicy Bypass -Command {0}", command));

			ProcessStartInfo processInfo = new ProcessStartInfo
			{
				FileName = "powershell.exe",
				Arguments = string.Format("-NoProfile -ExecutionPolicy Bypass -Command {0}", command),
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			try
			{
				using (Process process = new Process())
				{
					process.StartInfo = processInfo;
					process.Start();

					// Capture standard output and error streams
					string output = process.StandardOutput.ReadToEnd();
					string errors = process.StandardError.ReadToEnd();

					process.WaitForExit();

					if (!string.IsNullOrEmpty(output))
					{
						appxLog.Log("PowerShell Output:");
						appxLog.Log(output);
					}

					if (process.ExitCode != 0 || !string.IsNullOrEmpty(errors))
					{
						appxLog.Log("PowerShell process exited with code: " + process.ExitCode);
						if (!string.IsNullOrEmpty(errors))
						{
							appxLog.Log("PowerShell Errors:");
							appxLog.Log(errors);
						}
						return false;
					}

					appxLog.Log("Command executed successfully.");
					return true;
				}
			}
			catch (Exception ex)
			{
				// This will catch exceptions such as the user canceling the UAC prompt
				appxLog.Log("An exception occurred while trying to run the PowerShell command: " + ex.Message);
				return false;
			}
		}
	}
}
