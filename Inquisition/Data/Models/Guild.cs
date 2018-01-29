using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inquisition.Data.Models
{
	public class Guild
    {
		[Key]
		public string Id { get; set; }
		public string Name { get; set; }
		public string IconUrl { get; set; }
		public int MemberCount { get; set; }

		public virtual List<User> Users { get; set; } = new List<User>();
    }
}
