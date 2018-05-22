
using Inquisition.Database;
using Inquisition.Database.Models;
using Inquisition.Logging;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Handlers
{
	public class PrefixHandler : Handler
    {
		private DatabaseContext db;
		private static Dictionary<string, string> PrefixDictionary { get; set; }

		public PrefixHandler()
		{
			PrefixDictionary = new Dictionary<string, string>();

			try
			{
				db = new DatabaseContext();
				List<Guild> Guilds = db.Guilds.ToList();

				foreach (Guild guild in Guilds)
				{
					PrefixDictionary.Add(guild.Id, guild.Prefix);
				}
			}
			catch (Exception e)
			{
				LogHandler.WriteLine(LogTarget.Console, e.Message);
			}
		}

		public static string GetPrefix(string guildId)
		{
			return PrefixDictionary.GetValueOrDefault(guildId);
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
