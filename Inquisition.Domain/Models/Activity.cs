using System;
using System.ComponentModel.DataAnnotations;

namespace TheKrystalShip.Inquisition.Domain
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Arguments { get; set; }
        public DateTime ScheduledTime { get; set; }
        public DateTime DueTime { get; set; }

        public virtual User User { get; set; }
        public virtual Guild Guild { get; set; }
    }
}
