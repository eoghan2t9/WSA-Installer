using Microsoft.Win32;
using System;
using System.Reflection;
using System.Windows.Forms; // Required for MessageBox

namespace WSA_Installer.Logic
{
	/// <summary>
	/// Provides a utility to add the current executable to the Windows RunOnce registry key.
	/// </summary>
	public static class RunOnceInstaller
	{
		// The registry key path for the local machine's RunOnce entries.
		// This key is used for programs that should run once after a system restart.
		private const string RunOnceKeyPath = @"Software\Microsoft\Windows\CurrentVersion\RunOnce";

		/// <summary>
		/// Adds the currently executing assembly to the HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\RunOnce registry key.
		/// This ensures the program will run one time after the next system restart.
		/// This requires administrator privileges to write to HKEY_LOCAL_MACHINE.
		/// </summary>
		/// <param name="keyName">A unique name for the registry entry.</param>
		/// <param name="commandLineArgs">Optional command-line arguments to pass to the program.</param>
		/// <returns>True if the operation was successful, otherwise false.</returns>
		public static bool SetRunOnce(string keyName, string commandLineArgs = "")
		{
			try
			{
				// Get the full path of the current executable.
				string executablePath = Assembly.GetExecutingAssembly().Location;
				string command = $"\"{executablePath}\" {commandLineArgs}".Trim();

				// Open the registry key for the local machine with write permissions.
				using (RegistryKey key = Registry.LocalMachine.CreateSubKey(RunOnceKeyPath, true))
				{
					if (key == null)
					{
						MessageBox.Show("Error: Could not open or create the registry key. This operation likely requires administrator privileges.", "Registry Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}

					// Set the value with the executable path.
					key.SetValue(keyName, command, RegistryValueKind.String);
				}
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred while setting the RunOnce key: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		/// <summary>
		/// Removes a specified key from the HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\RunOnce registry.
		/// This can be used to clean up the entry after the program has run.
		/// </summary>
		/// <param name="keyName">The unique name of the registry entry to remove.</param>
		/// <returns>True if the operation was successful, otherwise false.</returns>
		public static bool RemoveRunOnce(string keyName)
		{
			try
			{
				// Open the registry key for the local machine with write permissions.
				using (RegistryKey key = Registry.LocalMachine.OpenSubKey(RunOnceKeyPath, true))
				{
					if (key == null || key.GetValue(keyName) == null)
					{
						// The key doesn't exist or the value is not present, so nothing to do.
						return true;
					}

					// Delete the specified value.
					key.DeleteValue(keyName);
				}
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred while removing the RunOnce key: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}
	}
}
