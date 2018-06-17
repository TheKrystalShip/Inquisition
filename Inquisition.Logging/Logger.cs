using System;

namespace Inquisition.Logging
{
    public class Logger<T> : ILogger<T> where T : class
    {        
        public void LogError(Exception e)
        {
            Log(ConsoleColor.Red, typeof(T).ToString(), null, e);
        }

        public void LogError(Exception e, string message)
        {
            Log(ConsoleColor.Red, typeof(T).ToString(), message, e);
        }

        public void LogError(string source, string message)
        {
            Log(ConsoleColor.Red, source, message);
        }

        public void LogError(string message)
        {
            Log(ConsoleColor.Red, typeof(T).ToString(), message);
        }

        public void LogInformation(string message)
        {
            LogInformation(typeof(T).ToString(), message);
        }

        public void LogInformation(string source, string message)
        {
            Log(ConsoleColor.Green, source, message);            
        }

        private void Log(ConsoleColor sourceForegroundColor, string source, string message, Exception e = null)
        {
            WriteDate();

            if (source != null)
            {
                WriteSource(sourceForegroundColor, source);
            }

            if (message != null)
            {
                WriteMessage(message);
            }

            if (e != null)
            {
                WriteException(e);
            }

            Console.WriteLine();
        }

        private void WriteDate()
        {
            string date = $"{DateTime.Now.TimeOfDay:hh\\:mm\\:ss}";
            Console.Write(date);
        }

        private void WriteSource(ConsoleColor color, string source)
        {
            Console.ForegroundColor = color;
            Console.Write("    " + source);
            Console.WriteLine();
            Console.ResetColor();
        }

        private void WriteMessage(string message)
        {
            Console.WriteLine("    " + message);
        }

        private void WriteException(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("    " + e.ToString());
            Console.ResetColor();
        }
    }
}
