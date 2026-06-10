using System;
using System.Diagnostics;

namespace Lunara2D.Common
{
    public static class LDebug
    {
        private static readonly string LogCategory = "LUNARA2D";

        public static void LogInfo(string content)
        {
            WriteLog("INFO", content, ConsoleColor.Gray);
        }

        public static void LogError(string content)
        {
            WriteLog("ERROR", content, ConsoleColor.Red);
        }

        public static void LogWarn(string content)
        {
            WriteLog("WARNING", content, ConsoleColor.Yellow);
        }

        private static void WriteLog(string level, string message, ConsoleColor color)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string formattedMessage = $"[{timestamp}] [{LogCategory}] [{level}] {message}";

            Console.ForegroundColor = color;
            Console.WriteLine(formattedMessage);
            Console.ResetColor();

            Debug.WriteLine(formattedMessage);
        }
    }
}