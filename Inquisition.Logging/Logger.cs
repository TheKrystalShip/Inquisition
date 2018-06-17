using System;

namespace Inquisition.Logging
{
    public class Logger<T> : ILogger<T> where T : class
    {
        public void LogError(Exception e)
        {
            Log(ConsoleColor.Red, ConsoleColor.White, typeof(T).ToString(), e.Message, e);
        }

        public void LogError(Exception e, string message)
        {
            Log(ConsoleColor.Red, ConsoleColor.White, typeof(T).ToString(), message, e);
        }

        public void LogError(string source, string message)
        {
            Log(ConsoleColor.Red, ConsoleColor.White, source, message);
        }

        public void LogInformation(string message)
        {
            LogInformation(typeof(T).ToString(), message);
        }

        public void LogInformation(string source, string message)
        {
            Log(ConsoleColor.Green, ConsoleColor.White, source, message);            
        }

        private void Log(ConsoleColor sourceForegroundColor, ConsoleColor messageForegroundColor, string source, string message, Exception e = null)
        {
            string date = $"{DateTime.Now.TimeOfDay:hh\\:mm\\:ss}";
            Console.Write(date);

            Console.ForegroundColor = sourceForegroundColor;
            Console.Write(" " + source);
            Console.WriteLine();

            Console.ForegroundColor = messageForegroundColor;
            Console.WriteLine("    " + message);

            if (e != null)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
