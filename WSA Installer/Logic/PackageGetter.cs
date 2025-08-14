using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WSA_Installer.Pages;

namespace WSA_Installer.Logic
{
    public class PackageGetter
    {
        private readonly FileDownloader _fileDownloader;
        private CancellationTokenSource _cancellationTokenSource;

        WSAPackages wsaPackages;
        string stagingDirectory = "";
        public PackageGetter()
        {

            // Instantiate the FileDownloader on the UI thread
            _fileDownloader = new FileDownloader();

            // Subscribe to the progress event
            // The event will now be raised on the UI thread automatically
            _fileDownloader.ProgressChanged += OnDownloadProgressChanged;

            stagingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Temp";
		}

        public async Task Download(List<WSAPackage> packages)
        {
            if (!Directory.Exists(stagingDirectory))
            {
                Directory.CreateDirectory(stagingDirectory);
            }

            // Note, later we must add support to handle more than one package being on the repo for each package type.
            // Currently, we are assuming package index 0 in each package type is the one we want since theres only 1 package for each typer. More may come later.
            

            InstallingPage pg = (InstallingPage)Form1.Instance.pages[3];
            int oldProgMax = pg.progressBar1.Maximum;
            pg.progressBar1.Maximum = 100;

            foreach (WSAPackage package in packages)
            {
                pg.AddDetail("Downloading Package: " + package.PackageName);

                // Reset the progress bar to zero
                pg.progressBar1.Value = 0;

                List<string> packageDownloadParts = package.PartsToDownload;
                try
                {
                    for (int i = 0; i < packageDownloadParts.Count; i++)
                    {
                        string part = packageDownloadParts[i];
                        string fileName = UrlUtilities.GetFilenameFromUrl(part);

                        pg.AddDetail("Downloading - " + fileName);

                        string filePath = Path.Combine(stagingDirectory, fileName);
                        if (!File.Exists(filePath))
                        {
                            File.Create(filePath).Close();
                        }

                        await _fileDownloader.DownloadFileAsync(part, filePath, packageDownloadParts.Count, i + 1);

                        package.FilePaths.Add(filePath);
                    }
                }
                catch (OperationCanceledException)
                {
                    MessageBox.Show("Download was cancelled.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
                finally
                {
                    //btnDownload.Enabled = true;
                    //btnCancel.Enabled = false;
                }

                pg.AddDetail(package.PackageName + " downloaded!");
            }

            pg.progressBar1.Maximum = oldProgMax;
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            InstallingPage pg = (InstallingPage)Form1.Instance.pages[3];
            
            pg.statusLbl.Text = "Downloading WSA Installation Packages...";



            // This code is now guaranteed to run on the UI thread.
            // No need for InvokeRequired.
            //pg.progressBar1.Maximum = (int)e.TotalBytesToReceive;
            //pg.progressBar1.Value = (int)e.BytesReceived;

            pg.progressBar1.Value = e.ProgressPercentage;
            pg.lblProgressPercentage.Text = $"Progress: {e.ProgressPercentage}%";

            string totalBytes = e.TotalBytesToReceive > 0
                ? FormatBytes(e.TotalBytesToReceive)
                : "?";

            pg.lblBytes.Text = $"Downloaded: {FormatBytes(e.BytesReceived)} / {totalBytes}";
            pg.lblSpeed.Text = $"Speed: {FormatBytes(e.DownloadSpeedBytesPerSecond)}/s";

            pg.lblTotalFiles.Text = $"Downloaded {e.CurrentFileIndex} of {e.TotalFiles} files";
        }

        static readonly string[] SizeSuffixes =
           { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        string FormatBytes(double value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + FormatBytes(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }
    }
}
