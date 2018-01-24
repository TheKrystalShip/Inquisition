using System.Collections.Generic;
using System.Xml.Serialization;

namespace Inquisition.Data.Models
{
	public enum Severity
	{
		Critical,
		Warning
	}
	public enum Type
	{
		Guild,
		General,
		Database,
		Inner
	}

	[XmlRoot]
	public class Report
	{
		public Severity Severity { get; set; }
		public Type Type { get; set; }

		public string GuildName { get; set; }
        public string GuildID { get; set; }

        public string UserName { get; set; }
        public string UserID { get; set; }

        public string Channel { get; set; }
        public string Message { get; set; }

        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }

		[XmlArray("InnerExceptions")]
		public List<Report> InnerExceptions { get; set; } = new List<Report>();

        public string Path { get; set; }
	}
}
