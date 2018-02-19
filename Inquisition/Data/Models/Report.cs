using Inquisition.Reporting.Models;

using System;

namespace Inquisition.Data.Models
{
	public class Report : IReport
	{
		public Guid Guid { get; set; }
		
		public Reporting.Models.Type Type { get; set; }
		
		public string GuildName { get; set; }
		public string GuildID { get; set; }
		
		public string UserName { get; set; }
		public string UserID { get; set; }
		
		public string Channel { get; set; }
		public string Message { get; set; }

		public string ErrorMessage { get; set; }
		public string StackTrace { get; set; }

		public string Path { get; set; }
	}
}
