using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Database.Core;
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
        private IServiceProvider ServiceCollection;

        public CommandHandler(DiscordSocketClient discordClient)
        {
            Client = discordClient;

            CommandService = new CommandService();
            CommandService.AddModulesAsync(Assembly.GetEntryAssembly());

			ServiceCollection = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .AddSingleton(new AudioService())
                .AddSingleton(new ReportService())
				.AddSingleton(new ReminderService(Client))
				.AddSingleton(new DealService())
				.AddDbContext<DatabaseContext>()
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
				LogHandler.WriteLine(e);
				return BotInfo.DefaultPrefix;
			}
		}
    }
}
