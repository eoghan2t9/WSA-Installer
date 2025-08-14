using SevenZip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSA_Installer.Logic
{
    public class ExtractArchives
    {
        static string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        static string _7z86Path = appPath + @"\7z-x86.dll";
        static string _7z64Path = appPath + @"\7z-x64.dll";

        public string InstallLocation = "";
        private string tempPath = "";

        private string stagingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Temp";

        private EventHandler _ExtractionFinished;
        public event EventHandler ExtractionFinished
        {
            add
            {
                _ExtractionFinished += value;
            }
            remove
            {
                _ExtractionFinished -= value;
            }
        }

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
        private string ArchiveFileName;
        private string PackageName;
		public ExtractArchives(string installLoc, string archiveFileName, string packageName)
        {
            ArchiveFileName = archiveFileName;
			InstallLocation = installLoc;
            PackageName = packageName;

			if (Environment.Is64BitProcess == true)
            {
                SevenZipBase.SetLibraryPath(_7z64Path);
            }
            else
            {
                SevenZipBase.SetLibraryPath(_7z86Path);
            }

            tempPath = Environment.GetEnvironmentVariable("TEMP");

        }
        Thread thrd = null;

        public async Task Extract_WSA()
        {
            await Task.Run(() =>
            {
                wsaExtract();
            });
            //thrd = new Thread(new ThreadStart(wsaExtract));
            //thrd.Start();
        }

        private void wsaExtract()
        {
            List<string> filenames = new List<string>();

            string stockZip = stagingDirectory + @"\" + ArchiveFileName;
            SevenZipExtractor ext = new SevenZipExtractor(stockZip);

            ext.FileExtractionFinished += Ext_FileExtractionFinished;
            ext.FileExtractionStarted += Ext_FileExtractionStarted;
            ext.ExtractionFinished += Ext_ExtractionFinished;
            ext.Extracting += Ext_Extracting;


            if (_updateStatus != null) _updateStatus($"Extracting {PackageName}...");
            ext.ExtractArchive(this.InstallLocation);
            //ext.ExtractFiles(this.InstallLocation, Enumerable.Range(0, ext.ArchiveFileData.Count).ToArray());
        }

        private void Ext_Extracting(object sender, ProgressEventArgs e)
        {
            if (_updateProg != null) _updateProg(e.PercentDone);
        }

        private void Ext_FileExtractionStarted(object sender, FileInfoEventArgs e)
        {
            string path = this.InstallLocation + @"\" + e.FileInfo.FileName;
            if (_updateDetails != null) _updateDetails("Extract:       " + path);
            if (_updateStatus != null) _updateStatus("Extracting: " + Path.GetFileName(path));
        }

        private void Ext_FileExtractionFinished(object sender, FileInfoEventArgs e)
        {
            SevenZipExtractor ext = (SevenZipExtractor)sender;

            if (_updateDetails != null) _updateDetails("Extracted:   " + this.InstallLocation + @"\" + e.FileInfo.FileName);

            
            if (_updateProg != null) _updateProg(100);
        }

        StringBuilder sb = new StringBuilder();

        private void Ext_ExtractionFinished(object sender, EventArgs e)
        {
            SevenZipExtractor ext = (SevenZipExtractor)sender;
            ext.Dispose();

            if (_updateDetails != null) _updateDetails($"Finished extracting {PackageName}");

            thrd = null;
        }

        //delegate void updateProgDel(int percentDone);
        //delegate void updateDetailsBoxDel(string detail);

        //private void updateDetailsBox(string details)
        //{
        //    if (Form1.Instance != null)
        //    {
        //        if (Form1.Instance.InvokeRequired)
        //        {
        //            Form1.Instance.Invoke(new updateDetailsBoxDel(updateDetailsBox), details);
        //        }
        //        else
        //        {
        //            detailsBox.Items.Add(details);
        //            detailsBox.SelectedIndex = detailsBox.Items.Count - 1;
        //        }
        //    }
        //}

        //private void updateProg(int percentDone)
        //{
        //    if (Form1.Instance != null)
        //    {
        //        if (Form1.Instance.InvokeRequired)
        //        {
        //            Form1.Instance.Invoke(new updateProgDel(updateProg), percentDone);
        //        }
        //        else
        //        {
        //            //progCT.Text = "7z Progress: " + percentDone.ToString() + "%";
        //            ProgressBar p = (ProgressBar)progCT;
        //            p.Value = percentDone;
        //        }
        //    }
        //}

        private string extractGitZips()
        {
            string zipPath = tempPath;
            if (Environment.Is64BitOperatingSystem == true)
            {
                zipPath += @"\MinGit-x64.7z";
                if (File.Exists(zipPath) == false)
                {
                    //File.WriteAllBytes(zipPath, WSA_Installer.Properties.Resources.MinGit_x64);
                }
                else
                {
                    File.Delete(zipPath);
                    //File.WriteAllBytes(zipPath, WSA_Installer.Properties.Resources.MinGit_x64);
                }
            }
            else
            {
                zipPath += @"\MinGit-x86.7z";
                if (File.Exists(zipPath) == false)
                {
                    //File.WriteAllBytes(zipPath, WSA_Installer.Properties.Resources.MinGit_x86);
                }
                else
                {
                    File.Delete(zipPath);
                    //File.WriteAllBytes(zipPath, WSA_Installer.Properties.Resources.MinGit_x86);
                }
            }

            return zipPath;
        }

        private string extractGitSE()
        {
            string zipPath = tempPath;
            zipPath += @"\GitSE.7z";
            if (File.Exists(zipPath) == false)
            {
                //File.WriteAllBytes(zipPath, WSA_Installer.Properties.Resources.GitSE);
            }
            else
            {
                File.Delete(zipPath);
                //File.WriteAllBytes(zipPath, WSA_Installer.Properties.Resources.GitSE);
            }

            return zipPath;
        }

        private string deleteGitZips()
        {
            string zipPath = System.Environment.GetEnvironmentVariable("TEMP");
            if (Environment.Is64BitOperatingSystem == true)
            {
                zipPath += @"\MinGit-x64.7z";
                if (File.Exists(zipPath) == true)
                {
                    File.Delete(zipPath);
                }
            }
            else
            {
                zipPath += @"\MinGit-x86.7z";
                if (File.Exists(zipPath) == true)
                {
                    File.Delete(zipPath);
                }
            }

            return zipPath;
        }

        private string deleteGitSE()
        {
            string zipPath = tempPath;
            zipPath += @"\GitSE.7z";
            if (File.Exists(zipPath) == true)
            {
                File.Delete(zipPath);
            }

            return zipPath;
        }
    }
}
