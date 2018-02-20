
using Inquisition.Database.Core;
using Inquisition.Database.Models;

using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Handlers
{
	public class PrefixHandler
    {
		public static Dictionary<string, string> PrefixDictionary { get; set; } = new Dictionary<string, string>();

		public PrefixHandler()
		{
			DatabaseContext db = new DatabaseContext();

			List<Guild> Guilds = db.Guilds.ToList();

			foreach (Guild guild in Guilds)
			{
				PrefixDictionary.Add(guild.Id, guild.Prefix);
			}
		}
    }
}
