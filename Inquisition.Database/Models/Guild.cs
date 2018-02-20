using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inquisition.Database.Models
{
	public class Guild
	{
		[Key]
		public string Id { get; set; }

		[MaxLength(100)]
		public string Name { get; set; }
		public string IconUrl { get; set; }
		public int MemberCount { get; set; }
		public string MemberAuditChannelId { get; set; }
		public string Prefix { get; set; }

		public virtual List<User> Users { get; set; } = new List<User>();
	}
}
