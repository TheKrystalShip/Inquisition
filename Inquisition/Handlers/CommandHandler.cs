using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Database;
using Inquisition.Extensions;
using Inquisition.Logging;
using Inquisition.Logging.Extensions;
using Inquisition.Managers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CommandHandler> _logger;

		public CommandHandler(ref DiscordSocketClient client, ref IConfiguration config)
        {
            _client = client;

            _config = config;

            _commandService = new CommandService(new CommandServiceConfig()
                {
                    LogLevel = LogSeverity.Debug,
                    CaseSensitiveCommands = false,
                    DefaultRunMode = RunMode.Async
                }
            );

            _commandService.AddModulesAsync(Assembly.GetEntryAssembly()).Wait();

            _serviceProvider = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commandService)
                .AddSingleton(_config)
                .AddHandlers()
                .AddServices()
                .AddManagers()
                .AddLogger()
                .AddDbContext<DatabaseContext>(x =>
                    {
                        x.UseSqlite(_config.GetConnectionString("SQLite"));
                    }
                )
                .BuildServiceProvider();

            _serviceProvider.GetService<DatabaseContext>().Migrate();
            _serviceProvider.GetService<EventManager>();

            _reportHandler = _serviceProvider.GetService<ReportHandler>();
            _logger = _serviceProvider.GetService<ILogger<CommandHandler>>();

            _commandService.Log += CommandServiceLog;
            _client.MessageReceived += HandleCommands;
        }

        private Task CommandServiceLog(LogMessage logMessage)
        {
            _logger.LogInformation(GetType().FullName + $" ({logMessage.Source})", logMessage.Message);
            return Task.CompletedTask;
        }

        private async Task HandleCommands(SocketMessage msg)
        {
			SocketUserMessage message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

			//string prefix = GetGuildPrefix(message) ?? Configuration.Get("Bot", "DefaultPrefix");
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
            SocketGuildChannel guildChannel = message.Channel as SocketGuildChannel;
			string socketGuildId = guildChannel?.Guild.Id.ToString();

            PrefixHandler prefixHandler = _serviceProvider.GetService<PrefixHandler>();

			return prefixHandler.GetPrefix(socketGuildId);
		}
	}
}
