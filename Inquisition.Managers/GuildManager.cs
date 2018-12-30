using Discord.WebSocket;

using System.Threading.Tasks;

using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Managers
{
    public class GuildManager : IGuildManager
    {
        private readonly ILogger<GuildManager> _logger;

        public GuildManager()
        {
            _logger = new Logger<GuildManager>();
        }

        public Task OnGuildMemberUpdatedAsync(SocketGuildUser before, SocketGuildUser after)
        {
            _logger.LogInformation($"Member updated: {before.Username} in {before.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task OnGuildUpdatedAsync(SocketGuild before, SocketGuild after)
        {
            _logger.LogInformation($"Guild updated: {before.Name}");
            return Task.CompletedTask;
        }
    }
}
