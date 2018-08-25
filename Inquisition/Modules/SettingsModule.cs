using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Data.Models;
using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Database.Models;
using TheKrystalShip.Inquisition.Handlers;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Modules
{
    public class SettingsModule : ModuleBase<SocketCommandContext>
    {
		private readonly DatabaseContext _dbContext;
        private readonly ReportHandler _reportHandler;
		private readonly DiscordSocketClient _client;
        private readonly PrefixHandler _prefixHandler;
        private readonly ILogger<SettingsModule> _logger;

		public SettingsModule(
            DatabaseContext dbContext,
            ReportHandler reportHandler,
            DiscordSocketClient client,
            PrefixHandler prefixHandler,
            ILogger<SettingsModule> logger)
		{
			_dbContext = dbContext;
            _reportHandler = reportHandler;
			_client = client;
            _prefixHandler = prefixHandler;
            _logger = logger;
		}

		[Command("audit channel")]
		[Alias("default audit channel")]
		public async Task ShowDefaultAuditChannelAsync()
		{
			try
			{
				string contextGuildId = Context.Guild.Id.ToString();
				Guild guild = _dbContext.Guilds.FirstOrDefault(x => x.Id == contextGuildId);

				if (guild is null)
				{
					await ReplyAsync("Your guild is not in the database for some reason...");
					return;
				}

				string defaultChannelId = guild.AuditChannelId;
				SocketTextChannel channel = _client.GetChannel(Convert.ToUInt64(defaultChannelId)) as SocketTextChannel;

                if (channel is null)
                {
                    await ReplyAsync("No default audit channel set for this guild");
                    return;
                }

				EmbedBuilder embed = EmbedHandler.Create(channel);

				await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("prefix")]
		public async Task ShowPrefixAsync()
		{
			string contextGuildId = Context.Guild.Id.ToString();
			string prefix = _prefixHandler.GetPrefix(contextGuildId);

			if (prefix is null)
			{
				await ReplyAsync("Your guild is not in the database for some reason...");
			}
			else
			{
				await ReplyAsync($"Prefix for this guild is: **{prefix}**");
			}
		}
    }

	[Group("set")]
	public class SetSettingsModule : ModuleBase<SocketCommandContext>
	{
		private readonly DatabaseContext _dbContext;
        private readonly ReportHandler _reportHandler;
        private readonly PrefixHandler _prefixHandler;
        private readonly ILogger<SetSettingsModule> _logger;

		public SetSettingsModule(
            DatabaseContext dbContext,
            ReportHandler reportHandler,
            PrefixHandler prefixHandler,
            ILogger<SetSettingsModule> logger)
		{
            _dbContext = dbContext;
            _reportHandler = reportHandler;
            _prefixHandler = prefixHandler;
            _logger = logger;
		}

		[Command("audit channel")]
		[Alias("default audit channel")]
		public async Task SetDefaultAuditChannelAsync(SocketGuildChannel channel)
		{
			try
			{
				string ContextGuildId = Context.Guild.Id.ToString();
				Guild guild = _dbContext.Guilds.FirstOrDefault(x => x.Id == ContextGuildId);

				if (guild is null)
				{
					await ReplyAsync("Your guild is not in the database for some reason...");
					return;
				}

				guild.AuditChannelId = channel.Id.ToString();
				_dbContext.SaveChanges();
				await ReplyAsync(ReplyHandler.Context(Result.Successful));
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("prefix")]
		public async Task SetDefaultPrefix(string newPrefix)
		{
			try
			{
				string contextGuildId = Context.Guild.Id.ToString();

				Guild guild = _dbContext.Guilds.FirstOrDefault(x => x.Id == contextGuildId);

				if (guild is null)
				{
					await ReplyAsync("Your guild is not in the database for some reason...");
					return;
				}

				string currentPrefix = guild.Prefix;
				guild.Prefix = newPrefix.Trim();
				_dbContext.Guilds.Update(guild);
				_dbContext.SaveChanges();
				
				_prefixHandler.SetPrefix(contextGuildId, newPrefix);

				await ReplyAsync($"Prefix was **{currentPrefix}**, now changed to **{newPrefix}**");
			}
			catch (Exception e)
			{
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}
	}
}
