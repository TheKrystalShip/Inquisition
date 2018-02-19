using Inquisition.Reporting.Handlers;
using Inquisition.Reporting.Models;
using Inquisition.Reporting.Properties;

using System;
using System.IO;
using System.Net.Mail;
using System.Threading;

namespace Inquisition.Reporting.Services
{
	public class EmailService
	{
		private string Host { get; set; } = Resources.Host;
		private int Port { get; set; } = 587;
		private string Username { get; set; } = Resources.Username;
		private string Password { get; set; } = Resources.Password;
		private string FromAddress { get; set; } = Resources.SenderAddress;
		private string ToAddress { get; set; } = Resources.TargetAddress;
		private string XSLFile { get; set; } = Path.Combine("Data", "XSL.xslt");

		public static event EventHandler EmailSent;
		public static event EventHandler EmailFailed;

		public static void SendReport<T>(T report) where T: IReport
			=> new Thread(new EmailService().SendReportThread).Start(report);

		private void SendReportThread(object parameter)
		{
			try
			{
				IReport report = parameter as IReport;
				SmtpClient client = new SmtpClient
				{
					Host = Host,
					Port = Port,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					EnableSsl = true,
					UseDefaultCredentials = false,
					Credentials = new System.Net.NetworkCredential(Username, Password)
				};

				MailAddress from = new MailAddress(FromAddress);
				MailAddress to = new MailAddress(ToAddress);

				MailMessage email = new MailMessage(from.Address, to.Address)
				{
					Subject = $"{DateTime.Now} - Error Report",
					IsBodyHtml = true,
					Body = XmlHandler.Transform(report.Path, XSLFile)
				};

				client.Send(email);

				EmailSent?.Invoke(this, EventArgs.Empty);
			}
			catch
			{
				EmailFailed?.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
