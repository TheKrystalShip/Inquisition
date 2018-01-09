using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Xml;

namespace Inquisition.Services
{
    public class LoggingService
    {
        public static void Log(SocketCommandContext context)
        {
            try
            {
                string date = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                string auditLogPath = Path.Combine("Data", "Logs", context.Guild.Name, date, "Audit-log");

                Directory.CreateDirectory(auditLogPath);

                string auditLogFile = auditLogPath + Path.DirectorySeparatorChar + context.Channel.Name + ".xml";

                switch (File.Exists(auditLogFile))
                {
                    case true:
                        AppendToAuditLog(context, auditLogFile);
                        break;
                    default:
                        WriteToAuditLog(context, auditLogFile);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void WriteToAuditLog(SocketCommandContext context, string auditLogFile)
        {
            try
            {
                SocketMessage msg = context.Message;

                using (XmlWriter xw = XmlWriter.Create(auditLogFile,
                    new XmlWriterSettings { Indent = true, Async = true, CloseOutput = true }))
                {
                    string time = String.Format("{0:hh:mm:ss}", msg.CreatedAt);

                    xw.WriteStartDocument();
                    xw.WriteStartElement("messages");

                    xw.WriteStartElement("message");
                    xw.WriteAttributeString("created-at", time);

                    string message = context.Message.Content.Replace("<@304353122019704842> ", "");

                    xw.WriteElementString("user", context.Message.Author.Username);
                    xw.WriteElementString("content", message);

                    xw.WriteEndElement();
                    xw.WriteEndDocument();

                    xw.Flush();
                    xw.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void AppendToAuditLog(SocketCommandContext context, string auditLogFile)
        {
            try
            {
                SocketMessage msg = context.Message;

                XmlDocument doc = new XmlDocument();
                doc.Load(auditLogFile);

                XmlNode root = doc.SelectSingleNode("messages");

                string time = String.Format("{0:hh:mm:ss}", msg.CreatedAt);

                XmlElement messageElement = doc.CreateElement("message");
                messageElement.SetAttribute("created-at", time);

                string message = context.Message.Content.Replace("<@304353122019704842> ", "");

                XmlElement contentElement = doc.CreateElement("content");
                contentElement.InnerText = message;

                XmlElement userElement = doc.CreateElement("user");
                userElement.InnerText = context.Message.Author.Username;

                messageElement.AppendChild(userElement);
                messageElement.AppendChild(contentElement);

                root.AppendChild(messageElement);

                doc.Save(auditLogFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void ErrorLog(Exception e)
        {
            try
            {
                string date = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                string errorLogPath = Path.Combine("Data", "Logs", "Errors", date);

                string time = String.Format("{0:hh-mm-ss}", DateTime.Now);
                string errorLogFile = errorLogPath + Path.DirectorySeparatorChar + time + ".log";

                using (XmlWriter xw = XmlWriter.Create(errorLogFile,
                    new XmlWriterSettings { Indent = true, CloseOutput = true, Async = true }))
                {

                    xw.WriteStartDocument();
                    xw.WriteStartElement("exceptions");

                    xw.WriteStartElement("exception");
                    xw.WriteAttributeString("date-time", $"{DateTime.Now}");

                    xw.WriteStartElement("details");

                    xw.WriteElementString("message", $"{e.Message}");
                    xw.WriteElementString("source", $"{e.Source}");
                    xw.WriteElementString("inner-exception", $"{e.InnerException}");
                    xw.WriteElementString("target-site", $"{e.TargetSite}");
                    xw.WriteElementString("stack-trace", $"{e.StackTrace}");

                    xw.WriteEndElement();

                    xw.WriteEndElement();

                    xw.WriteEndElement();
                    xw.WriteEndDocument();

                    xw.Flush();
                    xw.Close();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error at logging errors");
            }
        }

        public static void ErrorLog(SocketCommandContext context, Exception e)
        {
            try
            {
                string date = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                string guildErrorPath = Path.Combine("Data", "Logs", context.Guild.Name, date, "Errors");

                Directory.CreateDirectory(guildErrorPath);

                string time = String.Format("{0:hh-mm-ss}", DateTime.Now);
                string guildErrorFile = guildErrorPath + Path.DirectorySeparatorChar + time + ".xml";

                using (XmlWriter xw = XmlWriter.Create(guildErrorFile,
                    new XmlWriterSettings { Indent = true, CloseOutput = true, Async = true }))
                {
                    string message = context.Message.Content.Replace("<@304353122019704842> ", "");

                    xw.WriteStartDocument();
                    xw.WriteStartElement("exceptions");

                    xw.WriteStartElement("exception");
                    xw.WriteAttributeString("date-time", $"{DateTime.Now}");

                    xw.WriteStartElement("message");
                    xw.WriteAttributeString("channel", $"{context.Channel}");

                    xw.WriteElementString("text", $"{message}");

                    xw.WriteStartElement("user");
                    xw.WriteElementString("id", $"{context.User.Id}");
                    xw.WriteElementString("username", $"{context.User.Username}");
                    xw.WriteElementString("discriminator", $"{context.User.Discriminator}");
                    xw.WriteEndElement();

                    xw.WriteEndElement();

                    xw.WriteStartElement("details");
                    xw.WriteElementString("message", $"{e.Message}");
                    xw.WriteElementString("source", $"{e.Source}");
                    xw.WriteElementString("inner-exception", $"{e.InnerException}");
                    xw.WriteElementString("target-site", $"{e.TargetSite}");
                    xw.WriteElementString("stack-trace", $"{e.StackTrace}");
                    xw.WriteEndElement();
                    xw.WriteEndElement();

                    xw.WriteEndElement();
                    xw.WriteEndDocument();

                    xw.Flush();
                    xw.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
