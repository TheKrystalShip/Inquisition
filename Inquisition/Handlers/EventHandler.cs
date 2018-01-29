using Discord;
using Discord.WebSocket;

using Inquisition.Data.Handlers;
using Inquisition.Data.Models;
using Inquisition.Properties;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inquisition.Handlers
{
	public class EventHandler
    {
		private DbHandler db;
        private DiscordSocketClient Client;
        private SocketTextChannel MembersLogChannel;
		private ulong ChannelId;

        public EventHandler(DiscordSocketClient client)
        {
			ChannelId = Convert.ToUInt64(Resources.MembersLogChannel);
			Client = client;

			db = new DbHandler();

			Client.Ready += Ready;
            Client.Log += Log;

			new Thread(ReminderLoop) { IsBackground = true }.Start();

			MembersLogChannel = Client.GetChannel(ChannelId) as SocketTextChannel;
        }

		private Task Ready()
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

				return Task.CompletedTask;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return Task.CompletedTask;
			}
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
				List<Reminder> GetReminderList()
				{
					return db.Reminders
						.Where(x => x.DueDate <= DateTimeOffset.UtcNow)
						.Take(10)
						.Include(x => x.User)
						.ToList();
				}

				List<Reminder> RemindersList = GetReminderList();
				int sleepTime = 10000;

				if (RemindersList.Count > 0)
				{
					foreach (Reminder r in RemindersList)
					{
						Client.GetUser(Convert.ToUInt64(r.User.Id)).SendMessageAsync($"Reminder: {r.Message}");
						db.Reminders.Remove(r);
					}
					sleepTime = 1000;
					db.SaveChanges();
				}
				Thread.Sleep(sleepTime);
			}
		}
	}
}
