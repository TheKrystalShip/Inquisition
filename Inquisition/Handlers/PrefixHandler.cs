using Inquisition.Exceptions;
using Inquisition.Database.Core;
using Inquisition.Database.Models;

using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Handlers
{
	public class PrefixHandler : BaseHandler
    {
		private DatabaseContext db;
		private static Dictionary<string, string> PrefixDictionary { get; set; } = new Dictionary<string, string>();

		public PrefixHandler()
		{
			db = new DatabaseContext();

			List<Guild> Guilds = db.Guilds.ToList();

			foreach (Guild guild in Guilds)
			{
				PrefixDictionary.Add(guild.Id, guild.Prefix);
			}
		}

		public static string GetPrefix(string guildId)
		{
			try
			{
				return PrefixDictionary[guildId];
			}
			catch
			{
				throw new InquisitionNotFoundException("Prefix not found in dictionary");
			}
		}

		public static void SetPrefix(string guildId, string prefix)
		{
			PrefixDictionary[guildId] = prefix;
		}

		public override void Dispose()
		{
			db = null;
			PrefixDictionary = null;
		}
	}
}
