using System;

namespace Inquisition.Database.Repositories
{
    public class Message
    {
        public string Date { get; private set; }
        public string Source { get; private set; }
        public string Content { get; private set; }

        public Message(string source, string content)
        {
            Date = $"{DateTime.Now.TimeOfDay:hh\\:mm\\:ss}";
            Source = source;
            Content = content;
        }
    }
}
