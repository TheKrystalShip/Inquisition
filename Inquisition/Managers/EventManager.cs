using Discord;
using Discord.WebSocket;

using Inquisition.Logging;

using System.Threading.Tasks;

namespace Inquisition.Managers
{
    public class EventManager
    {
        private readonly DiscordSocketClient _client;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly ChannelManager _channelManager;
        private readonly GuildManager _guildManager;
        private readonly ILogger<EventManager> _logger;

		public EventManager(
            DiscordSocketClient client,
            UserManager conversionHandler,
            RoleManager roleManager,
            ChannelManager channelManager,
            GuildManager guildManager,
            ILogger<EventManager> logger)
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
			_client.ChannelCreated += (channel) => _channelManager.ChannelCreated(channel);
			_client.ChannelDestroyed += (channel) => _channelManager.ChannelDestroyed(channel);
			_client.ChannelUpdated += (before, after) => _channelManager.ChannelUpdated(before, after);

			_client.GuildMemberUpdated += (before, after) => _guildManager.GuildMemberUpdated(before, after);
			_client.GuildUpdated += (before, after) => _guildManager.GuildUpdated(before, after);

			_client.RoleCreated += (role) => _roleManager.RoleCreated(role);
			_client.RoleDeleted += (role) => _roleManager.RoleDeleted(role);
			_client.RoleUpdated += (before, after) => _roleManager.RoleUpdated(before, after);

            _client.Ready += () => _userManager.RegisterGuildUsersAsync();
            _client.JoinedGuild += (guild) => _userManager.RegisterNewGuildUsersAsync(guild);
			_client.UserBanned += (user, guild) => _userManager.UserBanned(user, guild);
			_client.UserJoined += (user) => _userManager.UserJoined(user);
			_client.UserLeft += (user) => _userManager.UserLeft(user);
			_client.UserUnbanned += (user, guild) => _userManager.UserUnbanned(user, guild);
			_client.UserUpdated += (before, after) => _userManager.UserUpdated(before, after);
		}
	}
}
