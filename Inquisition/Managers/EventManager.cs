using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Managers
{
    public class EventManager
    {
        private readonly DiscordSocketClient _client;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly ChannelManager _channelManager;
        private readonly GuildManager _guildManager;
        private readonly ILogger<EventManager> _logger;

        public EventManager(DiscordSocketClient client, UserManager conversionHandler, RoleManager roleManager, ChannelManager channelManager, GuildManager guildManager, ILogger<EventManager> logger)
        {
            _client = client;
            _userManager = conversionHandler;
            _roleManager = roleManager;
            _channelManager = channelManager;
            _guildManager = guildManager;
            _logger = logger;

            _client.Log += Log;

            SubscribeToEvents();
        }

        private Task Log(LogMessage logMessage)
        {
            if (!logMessage.Message.Contains("OpCode"))
            {
                _logger.LogInformation(GetType().FullName + $" ({logMessage.Source})", logMessage.Message);
            }

            return Task.CompletedTask;
        }

        private void SubscribeToEvents()
        {
            _client.ChannelCreated += async (channel) => await _channelManager.ChannelCreatedAsync(channel).ConfigureAwait(false);
            _client.ChannelDestroyed += async (channel) => await _channelManager.ChannelDestroyedAsync(channel).ConfigureAwait(false);
            _client.ChannelUpdated += async (before, after) => await _channelManager.ChannelUpdatedAsync(before, after).ConfigureAwait(false);

            _client.GuildMemberUpdated += async (before, after) => await _guildManager.GuildMemberUpdatedAsync(before, after).ConfigureAwait(false);
            _client.GuildUpdated += async (before, after) => await _guildManager.GuildUpdatedAsync(before, after).ConfigureAwait(false);

            _client.RoleCreated += async (role) => await _roleManager.RoleCreatedAsync(role).ConfigureAwait(false);
            _client.RoleDeleted += async (role) => await _roleManager.RoleDeletedAsync(role).ConfigureAwait(false);
            _client.RoleUpdated += async (before, after) => await _roleManager.RoleUpdated(before, after).ConfigureAwait(false);

            _client.Ready += async () => await _userManager.RegisterGuildUsersAsync().ConfigureAwait(false);
            _client.JoinedGuild += async (guild) => await _userManager.RegisterNewGuildUsersAsync(guild).ConfigureAwait(false);
            _client.UserBanned += async (user, guild) => await _userManager.UserBannedAsync(user, guild).ConfigureAwait(false);
            _client.UserJoined += async (user) => await _userManager.UserJoinedAsync(user).ConfigureAwait(false);
            _client.UserLeft += async (user) => await _userManager.UserLeftAsync(user).ConfigureAwait(false);
            _client.UserUnbanned += async (user, guild) => await _userManager.UserUnbannedAsync(user, guild).ConfigureAwait(false);
            _client.UserUpdated += async (before, after) => await _userManager.UserUpdatedAsync(before, after).ConfigureAwait(false);
        }
    }
}
