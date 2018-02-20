using Discord;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Inquisition.Handlers
{
	public class EventHandler
    {
        private DiscordSocketClient Client;

        public EventHandler(DiscordSocketClient client)
        {
			Client = client;
            Client.Log += Log;
			Client.Ready += Ready;
        }

		private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }

		private Task Ready()
		{
			RegisterUsers();
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
				LogHandler.WriteLine(e.Message);
			}
		}
	}
}
