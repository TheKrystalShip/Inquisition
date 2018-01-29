using System.ComponentModel.DataAnnotations;

namespace Inquisition.Data.Models
{
	public class Alert
    {
        [Key]
        public int Id { get; set; }
		
        public virtual User User { get; set; }
		public virtual User TargetUser { get; set; }
    }
}
