using Inquisition.Reporting.Handlers;
using Inquisition.Reporting.Models;
using Inquisition.Reporting.Services;

namespace Inquisition.Reporting.Core
{
	public class Reporter
    {
		public string OutputPath { get; set; }
		public string FileName { get; set; }
		public bool SendEmail { get; set; } = false;

		public void Report<T>(T report) where T: IReport
		{
			LogHandler.GenerateLogFile(ref report, OutputPath, FileName);

			if (SendEmail)
			{
				EmailService.SendReport(report);
			}
		}
	}
}
