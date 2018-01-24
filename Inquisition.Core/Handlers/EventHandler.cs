using Discord;
using Discord.WebSocket;

using Inquisition.Core.Properties;

using Inquisition.Database.Commands;
using Inquisition.Database.Handlers;
using Inquisition.Database.Models;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inquisition.Core.Handlers
{
	public class EventHandler
    {
        private DiscordSocketClient Client;
        private SocketTextChannel MembersLogChannel;
		private ulong ChannelId;

        public EventHandler(DiscordSocketClient client)
        {
			ChannelId = Convert.ToUInt64(Resources.MembersLogChannel);
			Client = client;

			Client.Ready += Ready;
            Client.Log += Log;

			//new Thread(ReminderLoop){ IsBackground = true }.Start();

			MembersLogChannel = Client.GetChannel(ChannelId) as SocketTextChannel;
        }

		private Task Ready()
		{
			using (CrudHandler ch = new CrudHandler())
			{
				foreach (SocketGuild guild in Client.Guilds)
				foreach (SocketGuildUser user in guild.Users)
				{
					if (!user.IsBot)
					{
						User temp = ConversionHandler.ToUser(user);
						if (ch.Insert(temp) == Result.Successful)
						{
							Console.WriteLine($"Added {user.Username} from {guild}");
						}
					}
				}
			}
			
			return Task.CompletedTask;
		}

		private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }

		public void ReminderLoop()
		{
			while (true)
			{
				List<Reminder> RemindersList = Select.ReminderList(10);

				int sleepTime = RemindersList.Count > 0 ? 1000 : 10000;

				foreach (Reminder r in RemindersList)
				{
					Client.GetUser(Convert.ToUInt64(r.User.Id)).SendMessageAsync($"Reminder: {r.Message}");
				}

				Result result;
				using (CrudHandler c = new CrudHandler())
				{
					result = c.Delete(RemindersList);
				}

				Thread.Sleep(sleepTime);
			}
		}
	}
}
