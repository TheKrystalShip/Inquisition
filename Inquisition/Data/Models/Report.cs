using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Inquisition.Data.Models
{
	[XmlRoot]
	public class Report
	{
		[Key]
		public Guid Guid { get; set; }

		public Severity Severity { get; set; }
		public Type Type { get; set; }

		[MaxLength(100)]
		public string GuildName { get; set; }
		[MaxLength(100)]
		public string GuildID { get; set; }

		[MaxLength(100)]
		public string UserName { get; set; }
		[MaxLength(100)]
		public string UserID { get; set; }

		[MaxLength(100)]
		public string Channel { get; set; }
		[MaxLength(500)]
		public string Message { get; set; }

		public string ErrorMessage { get; set; }
		public string StackTrace { get; set; }

		[XmlArray("InnerExceptions")]
		public List<Report> InnerExceptions { get; set; } = new List<Report>();

		public string Path { get; set; }
	}
}
