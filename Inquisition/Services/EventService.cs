using Discord.WebSocket;

using Inquisition.Database;
using Inquisition.Database.Models;
using Inquisition.Logging;
using Inquisition.Managers;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Services
{
    public class EventService : Service
    {
		private readonly DiscordSocketClient _client;
		private readonly DatabaseContext _dbContext;
        private readonly UserManager _conversionHandler;
        private readonly ILogger<EventService> _logger;

		public EventService(
            DiscordSocketClient client,
            DatabaseContext dbContext,
            UserManager conversionHandler,
            ILogger<EventService> logger)
		{
			_client = client;
            _dbContext = dbContext;
            _conversionHandler = conversionHandler;
            _logger = logger;
		}

		public async Task ChannelCreated(SocketChannel socketChannel)
		{
			SocketGuildChannel channel = socketChannel as SocketGuildChannel;

			if (channel is SocketTextChannel)
			{
				SocketTextChannel textChannel = channel as SocketTextChannel;
				await SendAuditMessageAsync($"Channel created: {textChannel.Mention}", GetGuildAuditChannel(channel.Guild));
			}
			else
			{
				await SendAuditMessageAsync($"Channel created: {channel}", GetGuildAuditChannel(channel.Guild));
			}
		}

		public async Task ChannelUpdated(SocketChannel beforeSocketChannel, SocketChannel afterSocketChannel)
		{
			SocketGuildChannel before = beforeSocketChannel as SocketGuildChannel;
			SocketGuildChannel after = afterSocketChannel as SocketGuildChannel;

			if (before is SocketTextChannel)
			{
				SocketTextChannel beforeText = before as SocketTextChannel;
				SocketTextChannel afterText = after as SocketTextChannel;

				await SendAuditMessageAsync($"Channel updated: {beforeText.Mention}, {afterText.Mention}", GetGuildAuditChannel(before.Guild));
			}
			else
			{
				await SendAuditMessageAsync($"Channel updated: {before}, {after}", GetGuildAuditChannel(before.Guild));
			}
		}

		public async Task ChannelDestroyed(SocketChannel socketChannel)
		{
			SocketGuildChannel channel = socketChannel as SocketGuildChannel;
			await SendAuditMessageAsync($"Channel destroyed: {channel}", GetGuildAuditChannel(channel.Guild));
		}

		public async Task GuildMemberUpdated(SocketGuildUser before, SocketGuildUser after)
		{
			await SendAuditMessageAsync($"Member updated: {before}, {after}", GetGuildAuditChannel(before.Guild));
		}

		public async Task GuildUpdated(SocketGuild before, SocketGuild after)
		{
			await SendAuditMessageAsync($"Guild updated: {before}, {after}", GetGuildAuditChannel(before));
		}

		public async Task RoleCreated(SocketRole role)
		{
			await SendAuditMessageAsync($"Role created: {role}", GetGuildAuditChannel(role.Guild));
		}

		public async Task RoleDeleted(SocketRole role)
		{
			await SendAuditMessageAsync($"Role deleted: {role}", GetGuildAuditChannel(role.Guild));
		}

		public async Task RoleUpdated(SocketRole before, SocketRole after)
		{
			await SendAuditMessageAsync($"Role updated: {before}, {after}", GetGuildAuditChannel(before.Guild));
		}

		public async Task UserBanned(SocketUser user, SocketGuild guild)
		{
			await SendAuditMessageAsync($"User banned: {user.Mention}", GetGuildAuditChannel(guild));
		}

		public async Task UserUnbanned(SocketUser user, SocketGuild guild)
		{
			try
			{
				SocketTextChannel auditChannel = GetGuildAuditChannel(guild);
				await auditChannel.SendMessageAsync($"User unbanned: {user.Mention}");
			}
			catch (Exception e)
			{
                _logger.LogError(e);
			}
		}

		public async Task UserJoined(SocketGuildUser user)
		{
			await SendAuditMessageAsync($"User joined: {user.Mention}", GetGuildAuditChannel(user.Guild));
			_conversionHandler.AddUser(user);
		}

		public async Task UserLeft(SocketGuildUser user)
		{
			await SendAuditMessageAsync($"User left: {user.Mention}", GetGuildAuditChannel(user.Guild));
			_conversionHandler.RemoveUser(user);
		}

		public async Task UserUpdated(SocketUser before, SocketUser after)
		{
			SocketGuildUser user = before as SocketGuildUser;
			await SendAuditMessageAsync($"User updated: {before}, {after}", GetGuildAuditChannel(user.Guild));
		}

		private SocketTextChannel GetGuildAuditChannel(SocketChannel socketChannel)
		{
			SocketGuildChannel channel = (SocketGuildChannel)socketChannel;

			string channelGuildId = channel.Guild.Id.ToString();
			Guild guild = _dbContext.Guilds.FirstOrDefault(x => x.Id == channelGuildId);

			if (guild is null)
			{
				throw new InvalidOperationException("Failed to retrieve guild from database");
			}

			if (guild.AuditChannelId is null)
			{
				throw new InvalidOperationException("Audit log channel is not specified");
			}
			
			SocketGuildChannel auditChannel = (SocketGuildChannel)_client.GetChannel(Convert.ToUInt64(guild.AuditChannelId));
			return auditChannel as SocketTextChannel;
		}
		private SocketTextChannel GetGuildAuditChannel(SocketGuild socketGuild)
		{
			string channelGuildId = socketGuild.Id.ToString();
			Guild guild = _dbContext.Guilds.FirstOrDefault(x => x.Id == channelGuildId);

			if (guild is null)
			{
				throw new InvalidOperationException("Failed to retrieve guild from database");
			}

			if (guild.AuditChannelId is null)
			{
				throw new InvalidOperationException("Audit log channel is not specified");
			}

			SocketGuildChannel auditChannel = (SocketGuildChannel)_client.GetChannel(Convert.ToUInt64(guild.AuditChannelId));
			return auditChannel as SocketTextChannel;
		}
		private async Task SendAuditMessageAsync(string message, SocketTextChannel channel)
		{
			await channel.SendMessageAsync(message);
		}
	}
}
