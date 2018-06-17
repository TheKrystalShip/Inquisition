using Discord.WebSocket;
using System;
using System.IO;

namespace Inquisition.Services
{
    public class LoggingService
    {
        private static string logFilePath;

        public LoggingService()
        {
            Directory.CreateDirectory("Data/Logs");

            logFilePath = String.Format("Data/Logs/{0:yyyy-MM-dd}.log", DateTime.Now);

            if (!File.Exists(logFilePath))
            {
                Console.WriteLine($"Creating log file {logFilePath}...");
                File.Create(logFilePath);
                Console.WriteLine("Done.");
            }
        }

        public static void Log(SocketMessage msg)
        {
            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                sw.WriteLine($"{msg.Channel} | {msg.Author}: {msg.Content}");
            }
        }
    }
}
