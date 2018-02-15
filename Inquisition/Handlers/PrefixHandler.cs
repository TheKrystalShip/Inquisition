using Inquisition.Data.Handlers;
using Inquisition.Data.Models;

using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Handlers
{
	public class PrefixHandler
    {
		private DbHandler db;
		public static Dictionary<string, string> PrefixDictionary { get; set; } = new Dictionary<string, string>();

		public PrefixHandler(DbHandler dbHandler)
		{
			db = dbHandler;
			LoadPrefixes();
		}

		public void LoadPrefixes()
		{
			List<Guild> Guilds = db.Guilds.ToList();

			foreach (Guild guild in Guilds)
			{
				PrefixDictionary.Add(guild.Id, guild.Prefix);
			}
		}
    }
}
