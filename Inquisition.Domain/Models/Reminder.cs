using System;
using System.ComponentModel.DataAnnotations;

namespace TheKrystalShip.Inquisition.Domain
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }

        public string Message { get; set; }
        public DateTimeOffset DueDate { get; set; }

        public virtual User User { get; set; }
    }
}
