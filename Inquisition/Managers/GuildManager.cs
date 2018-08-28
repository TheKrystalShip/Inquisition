using Discord.WebSocket;

using System.Threading.Tasks;

using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Managers
{
    public class GuildManager
    {
        private readonly ILogger<GuildManager> _logger;

        public GuildManager(ILogger<GuildManager> logger)
        {
            _logger = logger;
        }

        public Task GuildMemberUpdatedAsync(SocketGuildUser before, SocketGuildUser after)
        {
            _logger.LogInformation($"Member updated: {before.Username} in {before.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task GuildUpdatedAsync(SocketGuild before, SocketGuild after)
        {
            _logger.LogInformation($"Guild updated: {before.Name}");
            return Task.CompletedTask;
        }
    }
}
