using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Inquisition.Reporting
{
	internal class EmailService
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string FromAddress { get; set; }
		public string ToAddress { get; set; }
		public string XSLFile { get; set; }

		/// <summary>
		/// Send email with an error report
		/// </summary>
		/// <param name="report"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public async Task SendReport(IReport report)
		{
			try
			{
				SmtpClient client = new SmtpClient
				{
					Host = Host,
					Port = Port,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					EnableSsl = true,
					UseDefaultCredentials = false,
					Credentials = new NetworkCredential(Username, Password)
				};

				MailAddress from = new MailAddress(FromAddress);
				MailAddress to = new MailAddress(ToAddress);

				MailMessage email = new MailMessage(from.Address, to.Address)
				{
					Subject = $"{DateTime.Now} - Error Report",
					IsBodyHtml = true,
					Body = XmlHandler.Transform(report.Path, XSLFile)
				};

				await client.SendMailAsync(email);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
