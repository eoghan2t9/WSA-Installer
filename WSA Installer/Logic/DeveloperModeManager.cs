using Microsoft.Win32;
using System;
using System.Windows.Forms; // Required for MessageBox

namespace WSA_Installer.Logic
{
	/// <summary>
	/// Provides a static utility class for enabling or disabling Windows Developer Mode via the registry.
	/// This requires administrator privileges to modify HKEY_LOCAL_MACHINE.
	/// </summary>
	public static class DeveloperModeManager
	{
		// The registry key path for the Developer Mode setting.
		private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock";

		// The registry value name that controls Developer Mode.
		private const string DeveloperModeValueName = "AllowDevelopmentWithoutDevLicense";

		/// <summary>
		/// Enables Developer Mode by setting the registry value to 1.
		/// This allows sideloading of Universal Windows Platform (UWP) apps.
		/// </summary>
		/// <returns>True if the operation was successful, otherwise false.</returns>
		public static bool EnableDeveloperMode()
		{
			try
			{
				// Open the registry key for the local machine with write permissions.
				using (RegistryKey key = Registry.LocalMachine.CreateSubKey(RegistryKeyPath, true))
				{
					if (key == null)
					{
						MessageBox.Show("Error: Could not open or create the registry key. This operation requires administrator privileges.", "Registry Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}

					// Set the value to 1 to enable Developer Mode.
					key.SetValue(DeveloperModeValueName, 1, RegistryValueKind.DWord);
					return true;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred while enabling Developer Mode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		/// <summary>
		/// Disables Developer Mode by setting the registry value to 0.
		/// </summary>
		/// <returns>True if the operation was successful, otherwise false.</returns>
		public static bool DisableDeveloperMode()
		{
			try
			{
				// Open the registry key for the local machine with write permissions.
				using (RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryKeyPath, true))
				{
					if (key == null)
					{
						// If the key doesn't exist, Developer Mode is not enabled, so we can return true.
						return true;
					}

					// Set the value to 0 to disable Developer Mode.
					key.SetValue(DeveloperModeValueName, 0, RegistryValueKind.DWord);
					return true;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred while disabling Developer Mode: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		/// <summary>
		/// Checks the current status of Windows Developer Mode.
		/// </summary>
		/// <returns>True if Developer Mode is enabled, otherwise false.</returns>
		public static bool IsDeveloperModeEnabled
		{
			get
			{
				try
				{
					using (RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryKeyPath))
					{
						if (key == null)
						{
							// The key doesn't exist, so Developer Mode is not enabled.
							return false;
						}

						// Get the registry value. A value of 1 means Developer Mode is enabled.
						object value = key.GetValue(DeveloperModeValueName);
						if (value != null && value is int intValue)
						{
							return intValue == 1;
						}

						// Value is not 1 or is not an integer.
						return false;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"An error occurred while checking Developer Mode status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			}
		}
	}
}
