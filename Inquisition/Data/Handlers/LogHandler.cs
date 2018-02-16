using Inquisition.Data.Models;

using System;
using System.IO;

namespace Inquisition.Data.Handlers
{
	public class LogHandler
	{
		public static void GenerateLog(ref Report report)
		{
			string reportFilePath = GenerateReportPath(report);
			Directory.CreateDirectory(reportFilePath);

			string reportFileName = String.Format("{0:HH-mm-ss}", DateTime.Now);
			report.Path = Path.Combine(reportFilePath, reportFileName + ".xml");

			XmlHandler.Serialize(report);
		}

		private static string GenerateReportPath(Report report)
		{
			string LogDir = Path.Combine("Data", "Logs");
			string date = String.Format("{0:yyyy-MMMM-dd}", DateTime.Now);
			string year = String.Format("{0:yyyy}", DateTime.Now);
			string month = String.Format("{0:MMMM}", DateTime.Now);
			string day = String.Format("{0:dddd-dd}", DateTime.Now);

			switch (report.Type)
			{
				case Models.Type.Guild:
					return Path.Combine(LogDir, report.GuildName, year, month, day, report.Channel);
				case Models.Type.General:
				case Models.Type.Database:
				case Models.Type.Inner:
				default:
					return Path.Combine(LogDir, date, "Errors");
			};
		}

		public static void WriteLine<T>(params T[] value) where T: class
		{
			string data = "";
			foreach (T t in value)
				data += t.ToString() + "	";

			Console.WriteLine($"{DateTime.Now.TimeOfDay:hh\\:mm\\:ss} {data}");
		}
	}
}
