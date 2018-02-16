using Inquisition.Data.Handlers;
using Inquisition.Data.Models;

using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Handlers
{
	public class PrefixHandler
    {
		public static Dictionary<string, string> PrefixDictionary { get; set; } = new Dictionary<string, string>();

		public PrefixHandler()
		{
			DbHandler db = new DbHandler();

			List<Guild> Guilds = db.Guilds.ToList();

			foreach (Guild guild in Guilds)
			{
				PrefixDictionary.Add(guild.Id, guild.Prefix);
			}
		}
    }
}
