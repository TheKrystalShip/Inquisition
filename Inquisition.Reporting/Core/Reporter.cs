
using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace Inquisition.Reporting
{
	public class Test
	{
		private Reporter Reporter;

		public Test()
		{
			Reporter = new Reporter(new ReporterConfig()
			{
				
			});
		}
	}

	public class Reporter
    {
		private readonly ReporterConfig Config;

		public Reporter(ReporterConfig config)
		{
			Config = config;
		}

		/// <summary>
		/// Generates a log file based on a model implementing the IReport interface with
		/// the option to also send it via email.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="report">Model implementing IReport interface</param>
		/// <exception cref="InvalidOperationException">Email parameters not specified</exception>
		public void Report<T>(T report) where T: IReport
		{
			Log(ref report, Config.OutputPath, Config.FileName);

			if (Config.SendEmail)
			{
				foreach (PropertyInfo property in Config.GetType().GetProperties())
				{
					if (property.GetValue(Config) is null)
						throw new InvalidOperationException("Email parameters not specified");
				}

				EmailService emailService = new EmailService() {
					Host = Config.Host,
					Port = Config.Port,
					Username = Config.Username,
					Password = Config.Password,
					FromAddress = Config.FromAddress,
					ToAddress = Config.ToAddress,
					XSLFile = Config.XSLFile
				};

				emailService.SendReport(report);
			}
		}

		private void Log<T>(ref T report, string outputPath, string fileName) where T: IReport
		{
			string reportFilePath = outputPath ?? Path.Combine("Data", "Logs");
			Directory.CreateDirectory(reportFilePath);

			string reportFileName = fileName ?? String.Format("{0:HH-mm-ss}", DateTime.Now);
			report.Path = Path.Combine(reportFilePath, reportFileName + ".xml");

			XmlHandler.Serialize(report);
		}
	}
}
