using System.ComponentModel.DataAnnotations;

namespace Inquisition.Data.Models
{
	public class Game
    {
		[Key]
		[MaxLength(100)]
		public string Name { get; set; }

		[MaxLength(10)]
		public string Version { get; set; }

		[MaxLength(10)]
		public string Port { get; set; }

		public bool IsOnline { get; set; }

		[MaxLength(500)]
		public string Path { get; set; }

		[MaxLength(500)]
		public string LaunchArgs { get; set; }
    }
}
