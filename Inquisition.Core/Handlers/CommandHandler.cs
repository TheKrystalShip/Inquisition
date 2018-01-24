using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Core.Properties;
using Inquisition.Core.Services;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Inquisition.Core.Handlers
{
	public class CommandHandler
    {
        private DiscordSocketClient DiscordClient;
        private CommandService CommandService;
        private IServiceProvider ServiceCollection;

        public CommandHandler(DiscordSocketClient discordClient)
        {
            DiscordClient = discordClient;

            CommandService = new CommandService();

            ServiceCollection = new ServiceCollection()
                .AddSingleton(DiscordClient)
                .AddSingleton(CommandService)
                .AddSingleton(new AudioService())
                .AddSingleton(new ReportService())
                .BuildServiceProvider();

            CommandService.AddModulesAsync(Assembly.GetEntryAssembly());

            DiscordClient.MessageReceived += HandleCommands;
        }

        private async Task HandleCommands(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

			string prefix = Resources.Prefix;
            int argPos = 0;

            if (message.HasMentionPrefix(DiscordClient.CurrentUser, ref argPos) ||
                message.HasStringPrefix(prefix, ref argPos))
            {
                SocketCommandContext context = new SocketCommandContext(DiscordClient, message);
                IResult result = await CommandService.ExecuteAsync(context, argPos, ServiceCollection);

                if (!result.IsSuccess)
                {
                    await ReportService.Report(result.ErrorReason, msg);
                    Console.WriteLine($"{DateTimeOffset.UtcNow} - {result.ErrorReason}");
                }
            }
        }
    }
}
