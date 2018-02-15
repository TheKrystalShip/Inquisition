using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Data.Handlers;
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
        private DiscordSocketClient Client;
        private CommandService CommandService;
		private DbHandler db;
        private IServiceProvider ServiceCollection;

        public CommandHandler(DiscordSocketClient discordClient)
        {
            Client = discordClient;
			db = new DbHandler();
            CommandService = new CommandService();
            CommandService.AddModulesAsync(Assembly.GetEntryAssembly());

			ServiceCollection = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .AddSingleton(new AudioService())
                .AddSingleton(new ReportService(db))
				.AddSingleton(new DbHandler())
				.AddSingleton(new ReminderService(Client, db))
				.AddSingleton(new DealService(db))
				.AddDbContext<DbHandler>()
                .BuildServiceProvider();

            Client.MessageReceived += HandleCommands;
        }

        private async Task HandleCommands(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

			string prefix = GetGuildPrefix(message) ?? BotInfo.DefaultPrefix;
			
			int argPos = 0;

            if (message.HasMentionPrefix(Client.CurrentUser, ref argPos) ||
                message.HasStringPrefix(prefix, ref argPos))
            {
                SocketCommandContext context = new SocketCommandContext(Client, message);
                IResult result = await CommandService.ExecuteAsync(context, argPos, ServiceCollection);

                if (!result.IsSuccess)
                {
                    await ReportService.Report(result.ErrorReason, msg);
                    Console.WriteLine($"{DateTimeOffset.UtcNow} - {result.ErrorReason}");
                }
            }
        }

		private string GetGuildPrefix(SocketUserMessage message)
		{
			try
			{
				var guildChannel = message.Channel as SocketGuildChannel;
				string socketGuildId = guildChannel.Guild.Id.ToString();
				return PrefixHandler.PrefixDictionary[socketGuildId];
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}
    }
}
