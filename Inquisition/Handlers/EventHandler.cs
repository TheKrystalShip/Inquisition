using Discord;
using Discord.WebSocket;

using Inquisition.Logging;
using Inquisition.Services;

using System;
using System.Threading.Tasks;

namespace Inquisition.Handlers
{
	public class EventHandler : Handler
    {
        private DiscordSocketClient Client;
		private EventService EventService;

		public EventHandler(DiscordSocketClient client)
			=> Init(client);

		private void Init(DiscordSocketClient client)
		{
			Client = client;
			EventService = new EventService(Client);

			Client.Log += Log;
			Client.Ready += Ready;

			SubscribeToAuditService();
		}

		private Task Log(LogMessage msg)
        {
			if (!msg.Message.Contains("OpCode"))
			{
				Console.WriteLine(msg);
			}

			return Task.CompletedTask;
        }

		private Task Ready()
		{
			RegisterUsers();
			return Task.CompletedTask;
		}

		private void RegisterUsers()
		{
			LogHandler.WriteLine(LogTarget.Console, "Starting user registration...");

			try
			{
				foreach (SocketGuild guild in Client.Guilds)
				{
					foreach (SocketGuildUser user in guild.Users)
					{
						if (!user.IsBot)
						{
							ConversionHandler.AddUser(user);
						}
					}
				}

			}
			catch (Exception e)
			{
				LogHandler.WriteLine(LogTarget.Console, e);
			}
			finally
			{
				LogHandler.WriteLine(LogTarget.Console, ConversionHandler.UsersAdded > 0 ? $"Done, added {ConversionHandler.UsersAdded} user(s)" : "Done, no new users were added");
			}
		}

		private void SubscribeToAuditService()
		{
			Client.ChannelCreated += (channel) => EventService.ChannelCreated(channel);
			Client.ChannelDestroyed += (channel) => EventService.ChannelDestroyed(channel);
			Client.ChannelUpdated += (before, after) => EventService.ChannelUpdated(before, after);

			Client.GuildMemberUpdated += (before, after) => EventService.GuildMemberUpdated(before, after);
			Client.GuildUpdated += (before, after) => EventService.GuildUpdated(before, after);

			Client.RoleCreated += (role) => EventService.RoleCreated(role);
			Client.RoleDeleted += (role) => EventService.RoleDeleted(role);
			Client.RoleUpdated += (before, after) => EventService.RoleUpdated(before, after);

			Client.UserBanned += (user, guild) => EventService.UserBanned(user, guild);
			Client.UserJoined += (user) => EventService.UserJoined(user);
			Client.UserLeft += (user) => EventService.UserLeft(user);
			Client.UserUnbanned += (user, guild) => EventService.UserUnbanned(user, guild);
			Client.UserUpdated += (before, after) => EventService.UserUpdated(before, after);
		}
	}
}
