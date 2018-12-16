using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheKrystalShip.Inquisition.Domain
{
    public class User
    {
        [Key]
        public ulong Id { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(10)]
        public string Discriminator { get; set; }

        public string AvatarUrl { get; set; }
        public int? TimezoneOffset { get; set; }

        public virtual List<Server> Servers { get; set; } = new List<Server>();
        public virtual List<Joke> Jokes { get; set; } = new List<Joke>();
        public virtual List<Reminder> Reminders { get; set; } = new List<Reminder>();
        public virtual List<Alert> Alerts { get; set; } = new List<Alert>();
        public virtual List<Alert> TargetAlerts { get; set; } = new List<Alert>();
        public virtual List<Deal> Deals { get; set; } = new List<Deal>();
        public virtual List<Activity> Activities { get; set; } = new List<Activity>();
    }
}
