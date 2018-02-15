using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Data.Handlers;
using Inquisition.Data.Models;
using Inquisition.Handlers;

using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	public class SettingsModule : ModuleBase<SocketCommandContext>
    {
		private DbHandler db;
		private DiscordSocketClient Client;

		public SettingsModule(DbHandler dbHandler, DiscordSocketClient socketClient)
		{
			db = dbHandler;
			Client = socketClient;
		}

		[Command("default channel")]
		[Alias("log channel", "default log channel")]
		public async Task ShowDefaultLogChannel()
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

				string defaultChannelId = guild.MemberAuditChannelId;
				SocketTextChannel channel = Client.GetChannel(Convert.ToUInt64(defaultChannelId)) as SocketTextChannel;

				EmbedBuilder embed = EmbedHandler.Create(channel);

				await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				Console.WriteLine(e);
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
		private DbHandler db;
		public SetSettingsModule(DbHandler dbHandler) => db = dbHandler;

		[Command("log channel")]
		[Alias("default channel", "default log channel")]
		public async Task SetDefaultLogChannel(SocketGuildChannel channel)
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

				guild.MemberAuditChannelId = channel.Id.ToString();
				db.SaveChanges();
				await ReplyAsync(ReplyHandler.Context(Result.Successful));
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				Console.WriteLine(e);
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

				PrefixHandler.PrefixDictionary[contextGuildId] = newPrefix;

				await ReplyAsync($"Prefix was **{currentPrefix}**, now changed to **{newPrefix}**");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
