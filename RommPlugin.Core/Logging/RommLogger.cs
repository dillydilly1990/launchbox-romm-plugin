using System;
using System.IO;

namespace RommPlugin.Core.Logging
{
    public static class RommLogger
    {
        private static readonly string LogDirectory;
        private static bool _enabled;
        private static readonly object _lock = new object();

        static RommLogger()
        {
            LogDirectory = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Plugins",
                "RomM LaunchBox Integration",
                "Logs"
            );
        }

        public static void Initialize(bool enabled)
        {
            _enabled = enabled;
        }

        public static void Log(string message)
        {
            if (!_enabled) return;
            WriteToFile("INFO", message);
        }

        public static void LogError(string message)
        {
            WriteToFile("ERROR", message);
        }

        public static void LogException(Exception ex)
        {
            WriteToFile("ERROR", ex.ToString());
        }

        private static void WriteToFile(string level, string message)
        {
            try
            {
                lock (_lock)
                {
                    Directory.CreateDirectory(LogDirectory);
                    var filePath = Path.Combine(LogDirectory, $"romm-{DateTime.Now:yyyy-MM-dd}.log");
                    var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                    File.AppendAllText(filePath, line + Environment.NewLine);
                }
            }
            catch
            {
            }
        }
    }
}
