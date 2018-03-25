using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Data.Models;
using Inquisition.Database;
using Inquisition.Database.Models;
using Inquisition.Handlers;
using Inquisition.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	public class SettingsModule : ModuleBase<SocketCommandContext>
    {
		private DatabaseContext db;
		private DiscordSocketClient Client;

		public SettingsModule(DatabaseContext dbHandler, DiscordSocketClient socketClient)
		{
			db = dbHandler;
			Client = socketClient;
		}

		[Command("audit channel")]
		[Alias("default audit channel")]
		public async Task ShowDefaultAuditChannelAsync()
		{
			try
			{
				string contextGuildId = Context.Guild.Id.ToString();
				Guild guild = db.Guilds.FirstOrDefault(x => x.Id == contextGuildId);

				if (guild is null)
				{
					await ReplyAsync("Your guild is not in the database for some reason...");
					return;
				}

				string defaultChannelId = guild.AuditChannelId;
				SocketTextChannel channel = Client.GetChannel(Convert.ToUInt64(defaultChannelId)) as SocketTextChannel;

				EmbedBuilder embed = EmbedHandler.Create(channel);

				await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				LogHandler.WriteLine(LogTarget.Console, e);
			}
		}

		[Command("prefix")]
		public async Task ShowPrefixAsync()
		{
			string contextGuildId = Context.Guild.Id.ToString();

			Guild guild = db.Guilds.FirstOrDefault(x => x.Id == contextGuildId);
			if (guild is null)
			{
				await ReplyAsync("Your guild is not in the database for some reason...");
				return;
			}

			await ReplyAsync($"Prefix for this guild is: **{guild.Prefix}**");
		}
    }

	[Group("set")]
	public class SetSettingsModule : ModuleBase<SocketCommandContext>
	{
		private DatabaseContext db;
		public SetSettingsModule(DatabaseContext dbHandler) => db = dbHandler;

		[Command("audit channel")]
		[Alias("default audit channel")]
		public async Task SetDefaultAuditChannelAsync(SocketGuildChannel channel)
		{
			try
			{
				string ContextGuildId = Context.Guild.Id.ToString();
				Guild guild = db.Guilds.FirstOrDefault(x => x.Id == ContextGuildId);

				if (guild is null)
				{
					await ReplyAsync("Your guild is not in the database for some reason...");
					return;
				}

				guild.AuditChannelId = channel.Id.ToString();
				db.SaveChanges();
				await ReplyAsync(ReplyHandler.Context(Result.Successful));
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				LogHandler.WriteLine(LogTarget.Console, e);
			}
		}

		[Command("prefix")]
		public async Task SetDefaultPrefix(string newPrefix)
		{
			try
			{
				string contextGuildId = Context.Guild.Id.ToString();

				Guild guild = db.Guilds.FirstOrDefault(x => x.Id == contextGuildId);

				if (guild is null)
				{
					await ReplyAsync("Your guild is not in the database for some reason...");
					return;
				}

				string currentPrefix = guild.Prefix;
				guild.Prefix = newPrefix.Trim();
				db.Guilds.Update(guild);
				db.SaveChanges();
				
				PrefixHandler.SetPrefix(contextGuildId, newPrefix);

				await ReplyAsync($"Prefix was **{currentPrefix}**, now changed to **{newPrefix}**");
			}
			catch (Exception e)
			{
				LogHandler.WriteLine(LogTarget.Console, e);
			}
		}
	}
}
