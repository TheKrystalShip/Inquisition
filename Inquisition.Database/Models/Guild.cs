using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inquisition.Database.Models
{
	public class Guild
    {
		[Key]
		[MaxLength(20)]
		public string Id { get; set; }
		public string Name { get; set; }

		public virtual List<User> Users { get; set; } = new List<User>();
    }
}
