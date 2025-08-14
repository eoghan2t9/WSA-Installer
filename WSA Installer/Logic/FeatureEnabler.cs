using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace WSA_Installer.Logic
{
	public class FeatureEnabler
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

		public FeatureEnabler()
		{

		}

		/// <summary>
		/// Checks for and enables necessary virtualization features if they are not already enabled.
		/// </summary>
		public bool EnsureVirtualizationFeaturesEnabled()
		{
			string[] features =
			{
				"Microsoft-Hyper-V-All",
				"VirtualMachinePlatform",
				"HypervisorPlatform",
				"Microsoft-Windows-Subsystem-Linux"
			};

			bool restartRequired = false;

			foreach (var feature in features)
			{
				try
				{
					FeatureState state = GetFeatureState(feature);

					if (state == FeatureState.Disabled)
					{
						if (_updateDetails != null) _updateDetails($"Feature '{feature}' is not enabled. Attempting to enable it now.");

						if (EnableFeature(feature))
						{
							restartRequired = true;
						}
					}
					else if (state == FeatureState.Enabled)
					{
						if (_updateDetails != null) _updateDetails($"Feature '{feature}' is already enabled.");
					}
					else // Includes EnablePending, DisablePending, Unknown
					{
						if (_updateDetails != null) _updateDetails($"Status of feature '{feature}' is pending or unknown. A restart might be required.");
					}
				}
				catch (Exception ex)
				{
					if (_updateDetails != null) _updateDetails($"An error occurred while processing feature {feature}: {ex.Message}");
				}
			}

			if (restartRequired)
			{
				if (_updateDetails != null) _updateDetails("One or more features have been enabled. A system restart is required for the changes to take effect.");
			}
			else
			{
				if (_updateDetails != null) _updateDetails("All required virtualization features are already enabled.");
			}

			return restartRequired;
		}

		private enum FeatureState
		{
			Unknown,
			Enabled,
			Disabled,
			EnablePending,
			DisablePending
		}

		/// <summary>
		/// Checks the current state of a Windows feature using DISM.
		/// </summary>
		/// <param name="featureName">The name of the feature to check.</param>
		/// <returns>The current state of the feature.</returns>
		private FeatureState GetFeatureState(string featureName)
		{
			Process process = new Process();
			process.StartInfo.FileName = "dism.exe";
			process.StartInfo.Arguments = $"/Online /Get-FeatureInfo /FeatureName:{featureName}";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.CreateNoWindow = true;
			process.Start();

			string output = process.StandardOutput.ReadToEnd();
			process.WaitForExit();

			if (output.Contains("State : Enabled"))
			{
				return FeatureState.Enabled;
			}
			else if (output.Contains("State : Disabled"))
			{
				return FeatureState.Disabled;
			}
			else if (output.Contains("State : Enable Pending"))
			{
				return FeatureState.EnablePending;
			}
			else if (output.Contains("State : Disable Pending"))
			{
				return FeatureState.DisablePending;
			}

			return FeatureState.Unknown;
		}

		/// <summary>
		/// Enables a specific Windows feature using DISM.
		/// </summary>
		/// <param name="featureName">The name of the feature to enable.</param>
		/// <returns>True if the feature was enabled successfully, false otherwise.</returns>
		private bool EnableFeature(string featureName)
		{
			try
			{
				Process process = new Process();
				process.StartInfo.FileName = "dism.exe";
				process.StartInfo.Arguments = $"/Online /Enable-Feature /FeatureName:{featureName} /All /NoRestart";
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.CreateNoWindow = true;
				process.Start();

				string output = process.StandardOutput.ReadToEnd();
				process.WaitForExit();

				if (process.ExitCode == 0 || process.ExitCode == 3010) // 3010 is success, restart required
				{
					if (_updateDetails != null) _updateDetails($"Successfully enabled feature: {featureName}\n\nOutput:\n{output}");
					return true;
				}
				else
				{
					if (_updateDetails != null) _updateDetails($"Failed to enable feature: {featureName}\n\nExit Code: {process.ExitCode}\nOutput:\n{output}");
					return false;
				}
			}
			catch (Exception ex)
			{
				if (_updateDetails != null) _updateDetails($"An exception occurred while enabling {featureName}: {ex.Message}");
				return false;
			}
		}
	}
}
