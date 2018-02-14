using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inquisition.Data.Models
{
	public class User
    {
        [Key]
		[MaxLength(20)]
		public string Id { get; set; }
		[MaxLength(50)]
        public string Username { get; set; }
		[MaxLength(50)]
        public string Nickname { get; set; }
		[MaxLength(10)]
        public string Discriminator { get; set; }
        public string AvatarUrl { get; set; }
		public int? TimezoneOffset { get; set; }
		
		public virtual Guild Guild { get; set; }
        public virtual List<Joke> Jokes { get; set; } = new List<Joke>();
        public virtual List<Reminder> Reminders { get; set; } = new List<Reminder>();
        public virtual List<Alert> Alerts { get; set; } = new List<Alert>();
        public virtual List<Alert> TargetAlerts { get; set; } = new List<Alert>();
		public virtual List<Deal> Offers { get; set; } = new List<Deal>();
    }
}
