using Inquisition.Data.Models;
using Inquisition.Properties;

using System;
using System.IO;
using System.Net.Mail;
using System.Threading;

namespace Inquisition.Data.Handlers
{
	public class EmailHandler
	{
		private string Host = "smtp.gmail.com";
		private int Port = 587;
		private string Username = Resources.Username;
		private string Password = Resources.Password;

		private string FromAddress = Resources.SenderAddress;
		private string ToAddress = Resources.TargetAddress;
		private string XSLFile = Path.Combine("Data", "XSL.xslt");

		public static event EventHandler EmailSent;
		public static event EventHandler EmailFailed;

		public static void SendReport(Report report)
			=> new Thread(new EmailHandler().SendReportThread).Start(report);

		private void SendReportThread(object parameter)
		{
			try
			{
				Report report = parameter as Report;
				MailAddress from = new MailAddress(FromAddress);
				MailAddress to = new MailAddress(ToAddress);

				SmtpClient client = new SmtpClient
				{
					Host = Host,
					Port = Port,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					EnableSsl = true,
					UseDefaultCredentials = false,
					Credentials = new System.Net.NetworkCredential(Username, Password)
				};

				MailMessage email = new MailMessage(from.Address, to.Address)
				{
					Subject = "Error Report",
					IsBodyHtml = true,
					Body = XmlHandler.Transform(report.Path, XSLFile)
				};

				email.Attachments.Add(new Attachment(report.Path));

				client.Send(email);
				EmailSent?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				EmailFailed?.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
