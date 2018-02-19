using Inquisition.Reporting.Models;

using System;
using System.IO;

namespace Inquisition.Reporting.Handlers
{
	public class LogHandler
	{
		public static void GenerateLogFile<T>(ref T report, string outputPath, string fileName) where T: IReport
		{
			string reportFilePath = outputPath ?? Path.Combine("Data", "Logs");
			Directory.CreateDirectory(reportFilePath);

			string reportFileName = fileName ?? String.Format("{0:HH-mm-ss}", DateTime.Now);
			report.Path = Path.Combine(reportFilePath, reportFileName + ".xml");

			XmlHandler.Serialize(report);
		}
	}
}
