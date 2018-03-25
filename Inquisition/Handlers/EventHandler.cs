using Discord;
using Discord.WebSocket;

using Inquisition.Logging;
using Inquisition.Services;

using System;
using System.Threading.Tasks;

namespace Inquisition.Handlers
{
	public class EventHandler : BaseHandler
    {
        private DiscordSocketClient Client;
		private AuditService AuditService;

        public EventHandler(DiscordSocketClient client)
        {
			Client = client;
			AuditService = new AuditService(Client);

            Client.Log += Log;
			Client.Ready += Ready;

			SubscribeToAuditService();
        }

		private Task Log(LogMessage msg)
        {
			Console.WriteLine(msg);
			return Task.CompletedTask;
        }

		private async Task Ready()
		{
			await RegisterUsers();
			//ServiceHandler.StartAllLoops();
		}

		private async Task RegisterUsers()
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
			Client.ChannelCreated += (channel) => AuditService.ChannelCreated(channel);
			Client.ChannelDestroyed += (channel) => AuditService.ChannelDestroyed(channel);
			Client.ChannelUpdated += (before, after) => AuditService.ChannelUpdated(before, after);

			Client.GuildMemberUpdated += (before, after) => AuditService.GuildMemberUpdated(before, after);
			Client.GuildUpdated += (before, after) => AuditService.GuildUpdated(before, after);

			Client.RoleCreated += (role) => AuditService.RoleCreated(role);
			Client.RoleDeleted += (role) => AuditService.RoleDeleted(role);
			Client.RoleUpdated += (before, after) => AuditService.RoleUpdated(before, after);

			Client.UserBanned += (user, guild) => AuditService.UserBanned(user, guild);
			Client.UserJoined += (user) => AuditService.UserJoined(user);
			Client.UserLeft += (user) => AuditService.UserLeft(user);
			Client.UserUnbanned += (user, guild) => AuditService.UserUnbanned(user, guild);
			Client.UserUpdated += (before, after) => AuditService.UserUpdated(before, after);
		}
	}
}
