using Discord;
using Discord.WebSocket;

using Inquisition.Properties;

using System;
using System.Threading.Tasks;

namespace Inquisition.Handlers
{
	public class EventHandler
    {
        private DiscordSocketClient Client;
        private SocketTextChannel MembersLogChannel;
		private ulong ChannelId;

        public EventHandler(DiscordSocketClient client)
        {
			ChannelId = Convert.ToUInt64(BotInfo.MembersLogChannel);
			Client = client;

			Client.Ready += Ready;
            Client.Log += Log;

			MembersLogChannel = Client.GetChannel(ChannelId) as SocketTextChannel;
        }

		private Task Ready()
		{
			RegisterUsers();
			return Task.CompletedTask;
		}

		private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }

		private void RegisterUsers()
		{
			try
			{
				foreach (SocketGuild guild in Client.Guilds)
				foreach (SocketGuildUser user in guild.Users)
				{
					if (!user.IsBot)
					{
						ConversionHandler.AddUser(user);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
