using Discord;
using Discord.WebSocket;

using Inquisition.Logging;

using System;
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
			_client.Ready += ReadyAsync;
            _client.JoinedGuild += ClientJoinedGuildAsync;

			SubscribeToEvents();
		}

        private async Task ClientJoinedGuildAsync(SocketGuild guild)
        {
            await Task.Run(() =>
            {
                _logger.LogInformation($"Joined guild: {guild.Name}");
                _logger.LogInformation($"Starting user registration for: {guild.Name}");
                int usersAdded = 0;

                foreach (SocketGuildUser user in guild.Users)
                {
                    _userManager.AddUser(user);
                    usersAdded++;
                }

                _logger.LogInformation($"Added {usersAdded} users from: {guild.Name}");
                _logger.LogInformation($"Finished user registration for: {guild.Name}");
            });
        }

        private Task Log(LogMessage logMessage)
        {
			if (!logMessage.Message.Contains("OpCode"))
			{
                _logger.LogInformation(GetType().FullName + $" ({logMessage.Source})", logMessage.Message);
			}

			return Task.CompletedTask;
        }
        
        private async Task ReadyAsync()
        {
            await Task.Run(() =>
			{
				try
				{
					foreach (SocketGuild guild in _client.Guilds)
					{
						foreach (SocketGuildUser user in guild.Users)
						{
							if (!user.IsBot)
							{
								_userManager.AddUser(user);
							}
						}

                        _logger.LogInformation($"Added {_userManager.UsersAdded} new users in {guild.Name}");
					}

				}
				catch (Exception e)
				{
                    _logger.LogError(e);
				}
                finally
                {
                    _userManager.Dispose();
                }
			});
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

			_client.UserBanned += (user, guild) => _userManager.UserBanned(user, guild);
			_client.UserJoined += (user) => _userManager.UserJoined(user);
			_client.UserLeft += (user) => _userManager.UserLeft(user);
			_client.UserUnbanned += (user, guild) => _userManager.UserUnbanned(user, guild);
			_client.UserUpdated += (before, after) => _userManager.UserUpdated(before, after);
		}
	}
}
