using Inquisition.Reporting.Handlers;
using Inquisition.Reporting.Models;

using System;
using System.Net.Mail;
using System.Threading;

namespace Inquisition.Reporting.Services
{
	public class EmailService
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string FromAddress { get; set; }
		public string ToAddress { get; set; }
		public string XSLFile { get; set; }

		public void SendReport<T>(T report) where T: IReport
			=> new Thread(SendReportThread).Start(report);

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
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
