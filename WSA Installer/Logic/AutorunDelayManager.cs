using Microsoft.Win32;
using System;
using System.Windows.Forms; // Required for MessageBox

namespace WSA_Installer.Logic
{
	/// <summary>
	/// A static class for managing the Windows 11 autorun delay setting via the registry.
	/// </summary>
	public static class AutorunDelayManager
	{
		// The registry key path where the delay settings are stored.
		private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize";

		// The registry value name for the startup delay.
		private const string StartupDelayValueName = "StartupDelayInMSec";

		// The registry value name for the idle state check.
		private const string WaitForIdleValueName = "WaitForIdleState";

		/// <summary>
		/// Disables the autorun delay for startup applications by modifying the registry.
		/// This sets the StartupDelayInMSec and WaitForIdleState values to 0.
		/// Note: This change only affects the current user.
		/// </summary>
		/// <returns>True if the operation was successful, otherwise false.</returns>
		public static bool DisableAutorunDelay()
		{
			try
			{
				// Open the registry key for the current user.
				using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryKeyPath, true))
				{
					if (key == null)
					{
						MessageBox.Show("Error: Could not open or create the registry key. This operation requires administrator privileges.", "Registry Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}

					// Set the values to 0 to disable the delay.
					// The delay is 0 milliseconds.
					key.SetValue(StartupDelayValueName, 0, RegistryValueKind.DWord);

					// Disable the idle state check which also contributes to the delay.
					key.SetValue(WaitForIdleValueName, 0, RegistryValueKind.DWord);
				}
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred while disabling the autorun delay: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		/// <summary>
		/// Re-enables the default autorun delay behavior by deleting the registry values.
		/// The default behavior is to have a delay for startup applications.
		/// </summary>
		/// <returns>True if the operation was successful, otherwise false.</returns>
		public static bool EnableAutorunDelay()
		{
			try
			{
				using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
				{
					if (key == null)
					{
						// The key doesn't exist, so the delay is at its default setting (enabled).
						return true;
					}

					// Delete the values to revert to the default Windows behavior.
					if (key.GetValue(StartupDelayValueName) != null)
					{
						key.DeleteValue(StartupDelayValueName);
					}

					if (key.GetValue(WaitForIdleValueName) != null)
					{
						key.DeleteValue(WaitForIdleValueName);
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred while re-enabling the autorun delay: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		/// <summary>
		/// Checks the current status of the autorun delay setting.
		/// </summary>
		/// <returns>True if the delay is disabled (registry values are set to 0), otherwise false.</returns>
		public static bool IsAutorunDelayDisabled()
		{
			try
			{
				using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
				{
					if (key == null)
					{
						// The key doesn't exist, so the delay is at its default setting (enabled).
						return false;
					}

					// Check if the values are set to 0, which indicates the delay is disabled.
					int? delayValue = key.GetValue(StartupDelayValueName) as int?;
					int? idleValue = key.GetValue(WaitForIdleValueName) as int?;

					return delayValue == 0 && idleValue == 0;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred while checking the autorun delay status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}
	}
}
