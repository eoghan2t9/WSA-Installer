using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSA_Installer.Logic
{
    public class WSAPackages
    {
        public WSAPackage WSA_Stock { get; set; }

        public List<WSAUserDataPackage> UserDataPackages { get; set; }

        public List<WSAPackage> ToolsPackages { get; set; }

        public WSAPackages()
        {
            UserDataPackages = new List<WSAUserDataPackage>();
            ToolsPackages = new List<WSAPackage>();
        }

    }

    public class WSAPackage
    {
        public List<string> PackageParts { get; set; }
        public List<string> PartsToDownload { get; set; }
        public string PackageName { get; set; }

        public List<string> FilePaths { get; set; }

        public WSAPackage()
        {
            PackageParts = new List<string>();
            FilePaths = new List<string>();
        }
    }


    public class WSAUserDataPackage : WSAPackage
    {
        public string PackageApps { get; set; }
    }

}
