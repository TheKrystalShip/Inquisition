using Inquisition.Reporting;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Inquisition.Data.Models
{
	[XmlRoot]
	public class Report : IReport
	{
		public Guid Guid { get; set; }
		
		public string GuildName { get; set; }
		public string GuildID { get; set; }
		
		public string UserName { get; set; }
		public string UserID { get; set; }
		
		public string Channel { get; set; }
		public string Message { get; set; }

		public string ErrorMessage { get; set; }
		public string StackTrace { get; set; }

		[XmlArray("InnerExceptions")]
		public List<InnerReport> InnerExceptions { get; set; } = new List<InnerReport>();

		public string Path { get; set; }

		public class InnerReport : IReport
		{
			public Guid Guid { get; set; }
			public string ErrorMessage { get; set; }
			public string StackTrace { get; set; }
			public string Path { get; set; }
		}
	}
}
