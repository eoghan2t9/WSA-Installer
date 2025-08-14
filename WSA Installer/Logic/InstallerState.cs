using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSA_Installer.Logic
{
	public class InstallerState
	{
		public WsaInstallationOptions InstallationOptions { get; set; }
		public List<WSAPackage> SelectedPackages { get; set; }
		public string InstallationDirectory { get; set; }
	}
}
