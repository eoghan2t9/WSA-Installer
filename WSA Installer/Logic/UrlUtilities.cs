using System;
using System.IO;
using System.Net; // Required for WebUtility

namespace WSA_Installer.Logic
{
    public class UrlUtilities
    {
        /// <summary>
        /// Extracts and URL-decodes the filename from a given URL string.
        /// </summary>
        /// <param name="url">The URL string from which to extract the filename.</param>
        /// <returns>
        /// The URL-decoded filename as a string.
        /// Returns an empty string if the URL is invalid or no filename can be extracted.
        /// </returns>
        public static string GetFilenameFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty; // Handle null or empty input
            }

            try
            {
                // 1. Create a Uri object to parse the URL
                Uri uri = new Uri(url, UriKind.Absolute);

                // 2. Get the absolute path segment of the URL
                string path = uri.AbsolutePath;

                // 3. Extract the filename from the path
                string filename = Path.GetFileName(path);

                // 4. URL-decode the filename using Uri.UnescapeDataString
                // This is crucial: Uri.UnescapeDataString decodes percent-encoded characters (%XX)
                // but correctly leaves literal '+' characters as '+', unlike WebUtility.UrlDecode
                // which converts '+' to a space.
                string decodedFilename = Uri.UnescapeDataString(filename);

                return decodedFilename;
            }
            catch (UriFormatException)
            {
                // Handle cases where the input string is not a valid URL
                Console.WriteLine($"Error: Invalid URL format provided: {url}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors during processing
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
