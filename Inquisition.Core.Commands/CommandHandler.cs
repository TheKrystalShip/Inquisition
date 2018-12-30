using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System.Reflection;
using System.Threading.Tasks;

using TheKrystalShip.DependencyInjection;
using TheKrystalShip.Inquisition.Extensions;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Core.Commands
{
    public class CommandHandler : CommandService, ICommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(DiscordSocketClient client, CommandServiceConfig config) : base(config)
        {
            _client = client;
            _logger = new Logger<CommandHandler>();
        }

        public async Task LoadModulesAsync()
        {
            await AddModulesAsync(Assembly.GetAssembly(typeof(Modules.Module)), Container.GetServiceProvider());
        }

        public Task OnLog(LogMessage logMessage)
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

        public async Task OnClientMessageRecievedAsync(SocketMessage socketMessage)
        {
            SocketUserMessage socketUserMessage = socketMessage as SocketUserMessage;

            if (socketUserMessage is null | socketUserMessage.Author.IsBot)
                return;

            if (socketUserMessage.IsValid(_client.CurrentUser, out int argPos))
            {
                SocketCommandContext context = new SocketCommandContext(_client, socketUserMessage);
                IResult result = await ExecuteAsync(context, argPos, Container.GetServiceProvider());
            }
        }
    }
}
