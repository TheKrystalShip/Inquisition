using System;
using System.ComponentModel.DataAnnotations;

namespace TheKrystalShip.Inquisition.Domain
{
    public class Deal
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public ulong MessageId { get; set; }
        public DateTime ExpireDate { get; set; }
        public virtual User User { get; set; }
    }
}
