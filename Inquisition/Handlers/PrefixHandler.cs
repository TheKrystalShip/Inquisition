using System;
using System.Collections.Generic;
using System.Linq;

using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Database.Models;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Handlers
{
    public class PrefixHandler
    {
		private readonly DatabaseContext _dbContext;
        private readonly Dictionary<string, string> _prefixDictionary;
        private readonly ILogger<PrefixHandler> _logger;

		public PrefixHandler(
            DatabaseContext dbContext,
            ILogger<PrefixHandler> logger)
		{
            _dbContext = dbContext;
			_prefixDictionary = new Dictionary<string, string>();
            _logger = logger;

			try
			{
				List<Guild> Guilds = _dbContext.Guilds.ToList();

				foreach (Guild guild in Guilds)
				{
					_prefixDictionary.Add(guild.Id, guild.Prefix);
				}
			}
			catch (Exception e)
			{
                _logger.LogError(e);
			}
		}

		public string GetPrefix(string guildId)
		{
			return _prefixDictionary.GetValueOrDefault(guildId);
		}

		public void SetPrefix(string guildId, string prefix)
		{
			_prefixDictionary[guildId] = prefix;
		}
	}
}
