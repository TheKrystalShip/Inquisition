﻿using System.ComponentModel.DataAnnotations;

namespace Inquisition.Database.Models
{
	public class Joke
	{
		[Key]
		public int Id { get; set; }
		public string Text { get; set; }

		public virtual User User { get; set; }
	}
}