using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using TheKrystalShip.Inquisition.Database.SQLite;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Handlers
{
    public class PrefixHandler
    {
		private readonly SQLiteContext _dbContext;
        private readonly ConcurrentDictionary<ulong, string> _prefixDictionary;
        private readonly ILogger<PrefixHandler> _logger;

		public PrefixHandler(SQLiteContext dbContext)
		{
            _dbContext = dbContext;
			_prefixDictionary = new ConcurrentDictionary<ulong, string>();
            _logger = new Logger<PrefixHandler>();

			try
			{
				var Guilds = _dbContext.Guilds.Select(x => new { x.Id, x.Prefix }).ToList();

				foreach (var guild in Guilds)
				{
					_prefixDictionary.TryAdd(guild.Id, guild.Prefix);
				}
			}
			catch (Exception e)
			{
                _logger.LogError(e);
			}
		}

		public string Get(ulong guildId)
		{
			return _prefixDictionary.GetValueOrDefault(guildId);
		}

		public void Set(ulong guildId, string prefix)
		{
            _prefixDictionary.AddOrUpdate(guildId, prefix, (key, oldValue) => prefix);
		}
	}
}
