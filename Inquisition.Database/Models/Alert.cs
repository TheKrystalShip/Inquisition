using System.ComponentModel.DataAnnotations;

namespace TheKrystalShip.Inquisition.Database.Models
{
	public class Alert
	{
		[Key]
		public int Id { get; set; }

		public virtual User User { get; set; }
		public virtual User TargetUser { get; set; }
	}
}
