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

namespace Inquisition
{
    public class CommandHandler
    {
        private DiscordSocketClient Client;
        private CommandService Commands;
        private IServiceProvider Services;

        string logFilePath;

        public CommandHandler(DiscordSocketClient c)
        {
            Client = c;
            
            Commands = new CommandService();

            Services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(Commands)
                .AddSingleton(new AudioService())
                .BuildServiceProvider();

            Commands.AddModulesAsync(Assembly.GetEntryAssembly());

            logFilePath = String.Format("Data/Logs/{0:yyyy-MM-dd}.log", DateTime.Now);

            if (!File.Exists(logFilePath))
            {
                Console.WriteLine($"Creating log file {logFilePath}...");
                File.Create(logFilePath);
                Task.Delay(2000);
                Console.WriteLine("Done.");
            }

            HelpModule.Create(Commands);

            Client.MessageReceived += HandleCommands;
        }

        private async Task HandleCommands(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

            User localUser = DbHandler.GetFromDb(msg.Author);

            if (localUser is null)
            {
                DbHandler.AddToDb(localUser);
            }

            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                sw.WriteLine($"{msg.Channel} | {msg.Author}: {msg.Content}");
            }

            string prefix = "rip ";

            int argPos = 0;

            if (message.HasMentionPrefix(Client.CurrentUser, ref argPos) ||
                message.HasStringPrefix(prefix, ref argPos) ||
                message.Channel.GetType() == typeof(SocketDMChannel))
            {
                SocketCommandContext context = new SocketCommandContext(Client, message);
                IResult result = await Commands.ExecuteAsync(context, argPos, Services);

                if (!result.IsSuccess)
                {
                    Console.WriteLine($"{DateTimeOffset.UtcNow} - {result.ErrorReason}");
                }
            }
        }
    }
}
