using Inquisition.Reporting.Handlers;
using Inquisition.Reporting.Models;
using Inquisition.Reporting.Services;

using System;

namespace Inquisition.Reporting.Core
{
	public class Reporter
    {
		/// <summary>
		/// Directory to write XML file to.
		/// </summary>
		public string OutputPath { get; set; }
		/// <summary>
		/// XML Filename, extension added automatically.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Bool value to send email after generating error log file. If set to true, must specify email
		/// service details.
		/// </summary>
		public bool SendEmail { get; set; } = false;

		//Email Service
		/// <summary>
		/// Email service host url (Example: smtp.gmail.com).
		/// </summary>
		public string Host { get; set; }
		/// <summary>
		/// SMTP Client port (Example: 587 for Gmail).
		/// </summary>
		public int Port { get; set; }
		/// <summary>
		/// Email account username.
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// Email account password.
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// Email address from which email is sent.
		/// </summary>
		public string FromAddress { get; set; }
		/// <summary>
		/// Email address to send email to.
		/// </summary>
		public string ToAddress { get; set; }
		/// <summary>
		/// XSLT file to transform IReport model into a valid Email body html.
		/// </summary>
		public string XSLFile { get; set; }

		/// <summary>
		/// Generates a log file based on a model implementing the IReport interface with
		/// the option to also send it via email.
		/// </summary>
		/// <exception cref="InvalidOperationException">Email parameters not specified</exception>
		/// <typeparam name="T"></typeparam>
		/// <param name="report">Model implementing IReport interface</param>
		public void Report<T>(T report) where T: IReport
		{
			LogHandler.GenerateLogFile(ref report, OutputPath, FileName);

			if (SendEmail)
			{
				if (Host is null || Username is null || Password is null || FromAddress is null || ToAddress is null || XSLFile is null || Port == 0)
				{
					throw new InvalidOperationException("Email parameters not specified");
				}

				EmailService emailService = new EmailService() {
					Host = Host,
					Port = Port,
					Username = Username,
					Password = Password,
					FromAddress = FromAddress,
					ToAddress = ToAddress,
					XSLFile = XSLFile
				};

				emailService.SendReport(report);
			}
		}
	}
}
