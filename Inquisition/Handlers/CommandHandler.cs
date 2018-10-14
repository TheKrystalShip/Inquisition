using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Reflection;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Extensions;
using TheKrystalShip.Inquisition.Managers;
using TheKrystalShip.Inquisition.Properties;
using TheKrystalShip.Logging;
using TheKrystalShip.Logging.Extensions;

namespace TheKrystalShip.Inquisition.Handlers
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly ReportHandler _reportHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(ref DiscordSocketClient client)
        {
            _client = client;

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
                .AddHandlers()
                .AddServices()
                .AddManagers()
                .AddLogger()
                .AddTools()
                .AddDbContext<SQLiteContext>(x =>
                {
                    x.UseSqlite(Settings.GetConnectionString("SQLite"));
                })
                .BuildServiceProvider();

            _serviceProvider.GetService<SQLiteContext>().Migrate();
            _serviceProvider.GetService<EventManager>();

            _reportHandler = _serviceProvider.GetService<ReportHandler>();
            _logger = _serviceProvider.GetService<ILogger<CommandHandler>>();

            _commandService.Log += CommandServiceLog;
            _client.MessageReceived += HandleCommands;
        }

        private Task CommandServiceLog(LogMessage logMessage)
        {
            if (logMessage.Message != null)
            {
                _logger.LogInformation(GetType().FullName + $" ({logMessage.Source})", logMessage.Message);
            }
            else if (logMessage.Exception != null)
            {
                _logger.LogError(logMessage.Exception);
            }

            return Task.CompletedTask;
        }

        private async Task HandleCommands(SocketMessage msg)
        {
            SocketUserMessage message = msg as SocketUserMessage;

            if (message is null | message.Author.IsBot)
                return;
            
            int argPos = 0;

            bool messageHasMention = message.HasMentionPrefix(_client.CurrentUser, ref argPos);

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
    }
}
