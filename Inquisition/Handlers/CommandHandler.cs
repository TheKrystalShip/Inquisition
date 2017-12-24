using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Data;
using Inquisition.Modules;
using Inquisition.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Inquisition.Handlers
{
    public class CommandHandler
    {
        private DiscordSocketClient DiscordClient;
        private CommandService CommandService;
        private IServiceProvider ServiceCollection;
        private HelpModule HelpModule;
        private string logFilePath;

        public CommandHandler(DiscordSocketClient discordClient)
        {
            DiscordClient = discordClient;

            CommandService = new CommandService();

            ServiceCollection = new ServiceCollection()
                .AddSingleton(DiscordClient)
                .AddSingleton(CommandService)
                .AddSingleton(new GameService())
                .BuildServiceProvider();

            CommandService.AddModulesAsync(Assembly.GetEntryAssembly());

            Directory.CreateDirectory("Data/Logs");

            logFilePath = String.Format("Data/Logs/{0:yyyy-MM-dd}.log", DateTime.Now);

            if (!File.Exists(logFilePath))
            {
                Console.WriteLine($"Creating log file {logFilePath}...");
                File.Create(logFilePath);
                Console.WriteLine("Done.");
            }

            HelpModule = new HelpModule(CommandService);

            DiscordClient.MessageReceived += HandleCommands;
        }

        private async Task HandleCommands(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

            User localUser = DbHandler.Select.User(msg.Author);

            if (localUser is null)
            {
                DbHandler.Insert.User(localUser);
            }

            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                sw.WriteLine($"{msg.Channel} | {msg.Author}: {msg.Content}");
            }

            string prefix = "rip ";

            int argPos = 0;

            if (message.HasMentionPrefix(DiscordClient.CurrentUser, ref argPos) ||
                message.HasStringPrefix(prefix, ref argPos) ||
                message.Channel.GetType() == typeof(SocketDMChannel))
            {
                SocketCommandContext context = new SocketCommandContext(DiscordClient, message);
                IResult result = await CommandService.ExecuteAsync(context, argPos, ServiceCollection);

                if (!result.IsSuccess)
                {
                    Console.WriteLine($"{DateTimeOffset.UtcNow} - {result.ErrorReason}");
                }
            }
        }
    }
}
