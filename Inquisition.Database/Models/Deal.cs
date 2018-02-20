using System;
using System.ComponentModel.DataAnnotations;

namespace Inquisition.Database.Models
{
	public class Deal
	{
		[Key]
		public int Id { get; set; }
		public string Url { get; set; }
		public string MessageId { get; set; }
		public DateTime ExpireDate { get; set; }
		public virtual User User { get; set; }
	}
}
