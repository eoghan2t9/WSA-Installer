using System;
using System.IO;
using System.Text;

namespace WSA_Installer.Logic
{
	public class FileLogger
	{
		private readonly string _logFilePath;
		private static readonly object _lock = new object();

		/// <summary>
		/// Initializes a new instance of the FileLogger class.
		/// </summary>
		/// <param name="logFileName">The name of the log file (e.g., "application.log").</param>
		public FileLogger(string logFileName)
		{
			// Get the directory where the application is running
			string logDirectory = AppDomain.CurrentDomain.BaseDirectory;

			// Combine the directory and filename to get the full path
			_logFilePath = Path.Combine(logDirectory, logFileName);

			// Ensure the directory exists
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error creating log directory: {ex.Message}");
			}
		}

		/// <summary>
		/// Writes a message to the log file.
		/// </summary>
		/// <param name="message">The message to log.</param>
		public void Log(string message)
		{
			try
			{
				// Lock the file to prevent issues with multiple threads writing at the same time
				lock (_lock)
				{
					// Format the log entry with a timestamp
					string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}{Environment.NewLine}";

					// Append the text to the file. This will create the file if it doesn't exist.
					File.AppendAllText(_logFilePath, logEntry, Encoding.UTF8);
				}
			}
			catch (Exception ex)
			{
				// Fallback to console if file logging fails
				Console.WriteLine($"Failed to write to log file '{_logFilePath}'. Error: {ex.Message}");
				Console.WriteLine($"Original Message: {message}");
			}
		}
	}
}
