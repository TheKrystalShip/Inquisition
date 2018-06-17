using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Database;
using Inquisition.Extensions;
using Inquisition.Logging;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Inquisition.Handlers
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly ReportHandler _reportHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CommandHandler> _logger;

		public CommandHandler(DiscordSocketClient client)
        {
            _client = client;

            _logger = new Logger<CommandHandler>();

            _commandService = new CommandService(new CommandServiceConfig()
                {
                    DefaultRunMode = RunMode.Async,
                    LogLevel = LogSeverity.Verbose,
                    CaseSensitiveCommands = false
                }
            );

            _commandService.AddModulesAsync(Assembly.GetEntryAssembly()).Wait();

            _serviceProvider = new ServiceCollection()
                .AddDbContext<DatabaseContext>()
                .AddSingleton(_client)
                .AddSingleton(_commandService)
                .AddHandlers()
                .AddServices()
                .AddRepository()
                .AddLogger()
                .BuildServiceProvider();

            // Call some handlers/services to start them up
            _serviceProvider.GetService<EventHandler>();
            //_serviceProvider.GetService<ServiceHandler>(); // Not finished yet

            _reportHandler = _serviceProvider.GetService<ReportHandler>();

            _client.MessageReceived += HandleCommands;
        }

        private async Task HandleCommands(SocketMessage msg)
        {
			SocketUserMessage message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

			//string prefix = GetGuildPrefix(message) ?? BotInfo.DefaultPrefix;
			int argPos = 0;

			bool messageHasMention = message.HasMentionPrefix(_client.CurrentUser, ref argPos);
			//bool messageHasPrefix = message.HasStringPrefix(prefix, ref argPos);

			if (!messageHasMention)
				return;

            SocketCommandContext context = new SocketCommandContext(_client, message);
            IResult result = await _commandService.ExecuteAsync(context, argPos, _serviceProvider);

            if (!result.IsSuccess)
            {
                _reportHandler.ReportAsync(result.ErrorReason, message);
                _logger.LogError(result.ErrorReason);
            }
        }

		private string GetGuildPrefix(SocketUserMessage message)
		{
			var guildChannel = message.Channel as SocketGuildChannel;
			string socketGuildId = guildChannel.Guild.Id.ToString();

            PrefixHandler prefixHandler = _serviceProvider.GetService<PrefixHandler>();

			return prefixHandler.GetPrefix(socketGuildId);
		}
	}
}
