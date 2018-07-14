using System;
using System.ComponentModel.DataAnnotations;

namespace Inquisition.Database.Models
{
    public class Server
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public virtual User User { get; set; }
        public virtual Guild Guild { get; set; }

        [MaxLength(50)]
        public string Nickname { get; set; }
    }
}
