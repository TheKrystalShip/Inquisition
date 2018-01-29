using Inquisition.Data.Models;

using System;
using System.IO;

namespace Inquisition.Data.Handlers
{
	public class LogHandler
	{
		private static string LogDir = Path.Combine("Data", "Logs");

		public static void GenerateLog(ref Report report)
		{
			string date = String.Format("{0:yyyy-MMM-dd}", DateTime.Now);
			string year = String.Format("{0:yyyy}", DateTime.Now);
			string month = String.Format("{0:MMM}", DateTime.Now);
			string day = String.Format("{0:dd}", DateTime.Now);

			string reportFileDir;

			switch (report.GuildName)
			{
				case null:
					reportFileDir = Path.Combine(LogDir, date, "Errors");
					break;
				default:
					reportFileDir = Path.Combine(LogDir, report.GuildName, year, month, day, report.Channel);
					break;
			}

			Directory.CreateDirectory(reportFileDir);

			string reportFileName = String.Format("{0:HH-mm-ss}", DateTime.Now);

			report.Path = reportFileDir + Path.DirectorySeparatorChar + reportFileName + ".xml";

			XmlHandler.Serialize(report);
		}
	}
}
