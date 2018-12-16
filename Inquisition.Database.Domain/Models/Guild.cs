using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheKrystalShip.Inquisition.Domain
{
    public class Guild
    {
        [Key]
        public ulong Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public int MemberCount { get; set; }
        public ulong AuditChannelId { get; set; }
        public string Prefix { get; set; } = "--";

        public virtual List<Server> Servers { get; set; } = new List<Server>();
    }
}
