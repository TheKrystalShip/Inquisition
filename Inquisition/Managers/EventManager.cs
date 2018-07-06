﻿using Discord;
using Discord.WebSocket;

using Inquisition.Logging;
using Inquisition.Services;

using System;
using System.Threading.Tasks;

namespace Inquisition.Managers
{
    public class EventManager
    {
        private readonly DiscordSocketClient _client;
		private readonly EventService _eventService;
        private readonly UserManager _conversionHandler;
        private readonly ILogger<EventManager> _logger;

		public EventManager(
            DiscordSocketClient client,
            EventService eventService,
            UserManager conversionHandler,
            ILogger<EventManager> logger)
		{
			_client = client;
            _eventService = eventService;
            _conversionHandler = conversionHandler;
            _logger = logger;

			_client.Log += Log;
			_client.Ready += Ready;

			SubscribeToAuditService();
		}

		private Task Log(LogMessage logMessage)
        {
			if (!logMessage.Message.Contains("OpCode"))
			{
                _logger.LogInformation(GetType().FullName + $" ({logMessage.Source})", logMessage.Message);
			}

			return Task.CompletedTask;
        }
        
        private async Task Ready()
        {
            await Task.Run(() =>
			{
                _conversionHandler.Init();

				try
				{
					foreach (SocketGuild guild in _client.Guilds)
					{
						foreach (SocketGuildUser user in guild.Users)
						{
							if (!user.IsBot)
							{
								_conversionHandler.AddUser(user);
							}
						}
					}

				}
				catch (Exception e)
				{
                    _logger.LogError(e);
				}
                finally
                {
                    _conversionHandler.Dispose();
                }
			});
        }

		private void SubscribeToAuditService()
		{
			_client.ChannelCreated += (channel) => _eventService.ChannelCreated(channel);
			_client.ChannelDestroyed += (channel) => _eventService.ChannelDestroyed(channel);
			_client.ChannelUpdated += (before, after) => _eventService.ChannelUpdated(before, after);

			_client.GuildMemberUpdated += (before, after) => _eventService.GuildMemberUpdated(before, after);
			_client.GuildUpdated += (before, after) => _eventService.GuildUpdated(before, after);

			_client.RoleCreated += (role) => _eventService.RoleCreated(role);
			_client.RoleDeleted += (role) => _eventService.RoleDeleted(role);
			_client.RoleUpdated += (before, after) => _eventService.RoleUpdated(before, after);

			_client.UserBanned += (user, guild) => _eventService.UserBanned(user, guild);
			_client.UserJoined += (user) => _eventService.UserJoined(user);
			_client.UserLeft += (user) => _eventService.UserLeft(user);
			_client.UserUnbanned += (user, guild) => _eventService.UserUnbanned(user, guild);
			_client.UserUpdated += (before, after) => _eventService.UserUpdated(before, after);
		}
	}
}