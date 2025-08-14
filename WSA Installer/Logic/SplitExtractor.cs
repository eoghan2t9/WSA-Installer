using SevenZip;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace WSA_Installer.Logic
{
    public class SplitExtractor
    {

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

        public delegate void UpdateDetailsDel(string dets);

        private UpdateDetailsDel _updateDetails;
        public event UpdateDetailsDel UpdateDetails
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

        public delegate void UpdateStatusDel(string dets);

        private UpdateStatusDel _updateStatus;
        public event UpdateStatusDel UpdateStatus
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

        public delegate void UpdateProgDel(int prog);

        private UpdateProgDel _updateProg;
        public event UpdateProgDel UpdateProgress
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

        static string appPath = Path.GetDirectoryName(Application.ExecutablePath);

        public string InstallLocation = "";
        private string tempPath = "";

        private string stagingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Temp";

        public SplitExtractor(string installLoc)
        {
            this.InstallLocation = installLoc;
        }

        Thread thrd = null;
        public void SplitExtract_WSA()
        {
            thrd = new Thread(new ThreadStart(ExtractSplitArchive));
            thrd.Start();
        }

        public void ExtractSplitArchive()
        {
            string firstArchivePath = stagingDirectory + @"\WSA_STOCK.zip.001";
            string destinationDirectory = this.InstallLocation;

            if (_updateStatus != null) _updateStatus("Extracting WSA Stock...");

            // Ensure the destination directory exists
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            //Console.WriteLine($"Starting extraction of '{firstArchivePath}'...");

            try
            {
                // Use ReaderFactory to get a reader that supports progress events.
                // As before, just open the first file of the split archive.
                using (var reader = ReaderFactory.Open(File.OpenRead(firstArchivePath)))
                {
                    // Subscribe to the progress event
                    reader.EntryExtractionProgress += Reader_EntryExtractionProgress;

                    // Loop through each entry in the archive
                    while (reader.MoveToNextEntry())
                    {
                        // Skip directories
                        if (!reader.Entry.IsDirectory)
                        {
                            try
                            {
                                string path = this.InstallLocation + @"\" + reader.Entry.Key;
                                if (_updateStatus != null) _updateStatus("Extracting: " + Path.GetFileName(path));
                                if (_updateDetails != null) _updateDetails("Extract: " + Path.GetFileName(path));

                                //Console.WriteLine($"Extracting: {reader.Entry.Key}");

                                var options = new ExtractionOptions()
                                {
                                    //PreserveAttributes = true,
                                    //PreserveFileTime = true,
                                    ExtractFullPath = true,
                                    Overwrite = true
                                };

                                try
                                {
                                    string destFile = destinationDirectory + @"\" + reader.Entry.Key.Replace("/", @"\");
                                    string destDir = Path.GetDirectoryName(destFile);
                                    if (!Directory.Exists(destDir))
                                    {
                                        Directory.CreateDirectory(destDir);
                                    }

                                    FileStream str = null;
                                    if (!File.Exists(destFile))
                                    {
                                        str = File.Create(destFile);
                                        //str.Close();
                                        //str.Dispose();
                                    }
                                    if (str != null)
                                    {
                                        // This method will trigger the EntryExtractionProgress event
                                        reader.WriteEntryTo(str);
                                    }
                                    else
                                    {
                                        // This method will trigger the EntryExtractionProgress event
                                        reader.WriteEntryToFile(destFile, options);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debugger.Break();
                                }
                            }
                            catch (Exception ex)
                            {
                                Debugger.Break();
                            }
                        }
                    }
                }
                //Console.WriteLine("\nExtraction completed successfully!");
            }
            catch (Exception ex)
            {
                Debugger.Break();
                //Console.WriteLine($"\nAn error occurred: {ex.Message}");
            }

            // ------------------------------------------------- OLD CODE

            //// Ensure the destination directory exists
            //if (!Directory.Exists(destinationDirectory))
            //{
            //    Directory.CreateDirectory(destinationDirectory);
            //}

            //Console.WriteLine($"Starting extraction of '{firstArchivePath}'...");
            //Console.WriteLine($"Destination: '{destinationDirectory}'");

            //try
            //{
            //    // Open the first part of the archive (.001 file).
            //    // SharpCompress will automatically find the other parts (.002, .003, etc.)
            //    // as long as they are in the same folder.
            //    using (var archive = ArchiveFactory.Open(firstArchivePath))
            //    {
            //        IArchiveEntry[] entries = archive.Entries.ToArray();
            //        for (int i = 0; i < entries.Length; i++)
            //        {
            //            IArchiveEntry entry = entries[i];
            //            entry.WriteToDirectory(destinationDirectory);

            //            string path = this.InstallLocation + @"\" + e.FileInfo.FileName;

                        


            //            if (_updateDetails != null) _updateDetails("Extract: " + Path.GetFileName(path));

            //            double a = (((double)i + 1.0) / (double)entries.Length) * 100.0;
            //            if (_updateProg != null) _updateProg((int)Math.Round(a, 2));
            //        }
                   

            //        foreach (var entry in archive.Entries)
            //        {
            //            // Skip directory entries
            //            if (!entry.IsDirectory)
            //            {
            //                Console.WriteLine($"Extracting: {entry.Key}");

            //                // Extract the entry to the destination directory, overwriting if necessary.
            //                entry.WriteToDirectory(destinationDirectory, new ExtractionOptions()
            //                {
            //                    PreserveAttributes = true,
            //                    PreserveFileTime = true,
            //                    ExtractFullPath = true,
            //                    Overwrite = true
            //                });
            //            }
            //        }
            //    }
            //    Console.WriteLine("Extraction completed successfully!");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"An error occurred: {ex.Message}");
            //}
        }

        public void ExtractWithProgress(string firstArchivePath, string destinationDirectory)
        {
            
        }

        /// <summary>
        /// Event handler for the extraction progress.
        /// </summary>
        private void Reader_EntryExtractionProgress(object sender, ReaderExtractionEventArgs<IEntry> e)
        {
            // Calculate the percentage completed
            double percentage = (double)e.ReaderProgress.BytesTransferred / e.Item.Size * 100;

            // To prevent division by zero for empty files
            if (double.IsNaN(percentage) || double.IsInfinity(percentage))
            {
                percentage = 100;
            }

            if (_updateProg != null) _updateProg((int)Math.Round(percentage, 2));

            // Write the progress to the console on a single line
            //Console.Write($"\rProgress: {percentage:F2}% complete... ");
        }
    }
}
