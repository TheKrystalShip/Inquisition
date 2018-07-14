﻿using Discord.WebSocket;

using Inquisition.Logging;

using System.Threading.Tasks;

namespace Inquisition.Managers
{
    public class GuildManager
    {
        private readonly ILogger<GuildManager> _logger;

        public GuildManager(ILogger<GuildManager> logger)
        {
            _logger = logger;
        }

        public Task GuildMemberUpdated(SocketGuildUser before, SocketGuildUser after)
        {
            _logger.LogInformation($"Member updated: {before.Username} in {before.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task GuildUpdated(SocketGuild before, SocketGuild after)
        {
            _logger.LogInformation($"Guild updated: {before.Name}");
            return Task.CompletedTask;
        }
    }
}