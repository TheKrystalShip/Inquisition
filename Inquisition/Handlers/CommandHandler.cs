using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Data;
using Inquisition.Properties;
using Inquisition.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Inquisition.Handlers
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
                .AddSingleton(new GameService())
                .AddSingleton(new LoggingService())
                .AddSingleton(new ExceptionService(DiscordClient))
                .BuildServiceProvider();

            CommandService.AddModulesAsync(Assembly.GetEntryAssembly());

            DiscordClient.MessageReceived += HandleCommands;
        }

        private async Task HandleCommands(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

            try
            {
                User localUser = DbHandler.Select.User(msg.Author);

                if (localUser is null)
                {
                    DbHandler.Insert.User(message.Author as SocketGuildUser);
                }
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(e);
            }

            LoggingService.Log(msg);

            string prefix = Resources.Prefix;
            int argPos = 0;

            if (message.HasMentionPrefix(DiscordClient.CurrentUser, ref argPos) ||
                message.HasStringPrefix(prefix, ref argPos) ||
                message.Channel.GetType() == typeof(SocketDMChannel))
            {
                SocketCommandContext context = new SocketCommandContext(DiscordClient, message);
                IResult result = await CommandService.ExecuteAsync(context, argPos, ServiceCollection);

                if (!result.IsSuccess)
                {
                    await ExceptionService.SendErrorAsync(result.ErrorReason, msg);
                    Console.WriteLine($"{DateTimeOffset.UtcNow} - {result.ErrorReason}");
                }
            }
        }
    }
}
