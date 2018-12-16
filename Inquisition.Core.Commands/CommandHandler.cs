using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System.Reflection;
using System.Threading.Tasks;

using TheKrystalShip.DependencyInjection;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Core.Commands
{
    public class CommandHandler : CommandService
    {
        private DiscordSocketClient _client;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler() : this(new CommandServiceConfig() { CaseSensitiveCommands = false, DefaultRunMode = RunMode.Async })
        {

        }

        public CommandHandler(CommandServiceConfig config) : base(config)
        {
            AddModulesAsync(Assembly.GetAssembly(typeof(Modules.Module))).Wait();
            Log += OnLog;
            CommandExecuted += OnCommandExecutedAsync;
            _logger = new Logger<CommandHandler>();
        }

        public void SetClient(DiscordSocketClient client)
        {
            _client = client;
            _client.Log += OnLog;
            _client.MessageReceived += OnClientMessageRecievedAsync;
        }

        private Task OnLog(LogMessage logMessage)
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

        private async Task OnClientMessageRecievedAsync(SocketMessage msg)
        {
            SocketUserMessage message = msg as SocketUserMessage;

            if (message is null | message.Author.IsBot)
                return;
            
            int argPos = 0;

            bool messageHasMention = message.HasMentionPrefix(_client.CurrentUser, ref argPos);

            if (!messageHasMention)
                return;

            SocketCommandContext context = new SocketCommandContext(_client, message);
            IResult result = await ExecuteAsync(context, argPos, Container.GetServiceProvider());
        }

        private Task OnCommandExecutedAsync(CommandInfo command, ICommandContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                _logger.LogError(result.ErrorReason);
            }

            return Task.CompletedTask;
        }
    }
}
