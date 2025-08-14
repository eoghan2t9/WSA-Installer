using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSA_Installer.Logic
{
    public class DownloadProgressChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the percentage of the download that has been completed (0-100).
        /// </summary>
        public int ProgressPercentage { get; }

        /// <summary>
        /// Gets the number of bytes that have been downloaded so far.
        /// </summary>
        public long BytesReceived { get; }

        /// <summary>
        /// Gets the total number of bytes to download. Returns -1 if the total size is unknown.
        /// </summary>
        public long TotalBytesToReceive { get; }

        /// <summary>
        /// Gets the current download speed in bytes per second.
        /// </summary>
        public double DownloadSpeedBytesPerSecond { get; }

        public int TotalFiles { get; }

        public int CurrentFileIndex { get; }

        /// <summary>
        /// Initializes a new instance of the DownloadProgressChangedEventArgs class.
        /// </summary>
        /// <param name="progressPercentage">The percentage of the download completed.</param>
        /// <param name="bytesReceived">The number of bytes received.</param>
        /// <param name="totalBytesToReceive">The total number of bytes to receive.</param>
        /// <param name="downloadSpeedBytesPerSecond">The current download speed in bytes per second.</param>
        public DownloadProgressChangedEventArgs(int progressPercentage, long bytesReceived, long totalBytesToReceive, double downloadSpeedBytesPerSecond, int totalFiles, int currentFileIndex)
        {
            ProgressPercentage = progressPercentage;
            BytesReceived = bytesReceived;
            TotalBytesToReceive = totalBytesToReceive;
            DownloadSpeedBytesPerSecond = downloadSpeedBytesPerSecond;
            TotalFiles = totalFiles;
            CurrentFileIndex = currentFileIndex;
        }
    }

    public class FileDownloader
    {
        // Event to report progress updates
        public event EventHandler<DownloadProgressChangedEventArgs> ProgressChanged;

        private readonly HttpClient _httpClient;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly SynchronizationContext _syncContext;
        private int TotalFiles;
        private int CurrentFileIndex;

        public FileDownloader()
        {
            // Capture the SynchronizationContext of the thread where the class is instantiated.
            // In a WinForms app, this should be the UI thread.
            _syncContext = SynchronizationContext.Current;

            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Asynchronously downloads a file from a URL to a specified path.
        /// </summary>
        public async Task DownloadFileAsync(string url, string filePath, int totalFiles, int currentFileIndex)
        {
            TotalFiles = totalFiles;
            CurrentFileIndex = currentFileIndex;

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            try
            {
                await DownloadFileInternalAsync(url, filePath, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation
            }
            catch (Exception ex)
            {
                Debugger.Break();
                MessageBox.Show($"Download failed: {ex.Message}");
            }
        }

        private async Task DownloadFileInternalAsync(string url, string filePath, CancellationToken cancellationToken)
        {
            long totalBytesToReceive = -1;
            long bytesReceived = 0;
            DateTime lastProgressTime = DateTime.Now;
            long lastBytesReceived = 0;

            try
            {
                using (var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    // Instead of just EnsureSuccessStatusCode(), let's check it manually
                    // and throw a more descriptive exception if it fails.
                    if (!response.IsSuccessStatusCode)
                    {
                        // This is the key change. We throw an exception with specific details.
                        throw new HttpRequestException($"Download failed with status code: {(int)response.StatusCode} {response.ReasonPhrase}");
                    }

                    if (response.Content.Headers.ContentLength.HasValue)
                    {
                        totalBytesToReceive = response.Content.Headers.ContentLength.Value;
                    }

                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        var buffer = new byte[8192];
                        int bytesRead;

                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                            bytesReceived += bytesRead;

                            var currentTime = DateTime.Now;
                            var timeElapsed = (currentTime - lastProgressTime).TotalSeconds;

                            if (timeElapsed >= 0.1)
                            {
                                var bytesChange = bytesReceived - lastBytesReceived;
                                double downloadSpeed = bytesChange / timeElapsed;

                                int progressPercentage = totalBytesToReceive > 0
                                    ? (int)((double)bytesReceived / totalBytesToReceive * 100)
                                    : 0;

                                // Now, the FileDownloader class posts the event itself
                                OnProgressChanged(new DownloadProgressChangedEventArgs(progressPercentage, bytesReceived, totalBytesToReceive, downloadSpeed, TotalFiles, CurrentFileIndex));

                                lastProgressTime = currentTime;
                                lastBytesReceived = bytesReceived;
                            }
                        }

                        if (totalBytesToReceive > 0)
                        {
                            OnProgressChanged(new DownloadProgressChangedEventArgs(100, bytesReceived, totalBytesToReceive, 0, TotalFiles, CurrentFileIndex));
                        }
                    }
                }
            }
            finally
            {
                _cancellationTokenSource?.Dispose();
            }
        }

        public void CancelDownload()
        {
            _cancellationTokenSource?.Cancel();
        }

        protected virtual void OnProgressChanged(DownloadProgressChangedEventArgs e)
        {
            // Check if a SynchronizationContext was captured and if we are on a different thread
            if (_syncContext != null && _syncContext != SynchronizationContext.Current)
            {
                // Post the event to the UI thread using the captured context
                _syncContext.Post(state => ProgressChanged?.Invoke(this, (DownloadProgressChangedEventArgs)state), e);
            }
            else
            {
                // If on the same thread or no context was captured, just invoke directly
                ProgressChanged?.Invoke(this, e);
            }
        }
    }
}
