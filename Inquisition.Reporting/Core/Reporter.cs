using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Inquisition.Reporting
{
	public class Reporter
    {
		private readonly ReporterConfig Config;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="config">
		/// ReporterConfig to specify some paramenters on how to make reports
		/// </param>
		public Reporter(ReporterConfig config)
		{
			Config = config;
		}

		/// <summary>
		/// Generates a log file based on a model implementing the IReport interface with
		/// the option to also send it via email.
		/// </summary>
		/// <typeparam name="T">Implementation of IReport interface</typeparam>
		/// <param name="report">Model implementing IReport interface</param>
		/// <exception cref="ArgumentNullException"></exception>
		public async Task Report<T>(T report) where T: class, IReport
		{
			Directory.CreateDirectory(Config.OutputPath);
			report.Path = Path.Combine(Config.OutputPath, Config.FileName);
			XmlHandler.Serialize(report);

			if (Config.SendEmail)
			{
				foreach (PropertyInfo property in Config.GetType().GetProperties())
				{
					if (property.GetValue(Config) is null)
						throw new ArgumentNullException($"{nameof(property)} not specified");
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

				await emailService.SendReport(report);
			}
		}
	}
}
