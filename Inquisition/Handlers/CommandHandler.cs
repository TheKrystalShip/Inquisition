using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Database;
using Inquisition.Exceptions;
using Inquisition.Logging;
using Inquisition.Properties;
using Inquisition.Services;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Inquisition.Handlers
{
	public class CommandHandler : Handler
    {
        private DiscordSocketClient Client;
        private CommandService CommandService;
        private IServiceProvider ServiceCollection;

		public CommandHandler(DiscordSocketClient client)
			=> Init(client).Wait();

		private async Task Init(DiscordSocketClient client)
		{
			Client = client;

			CommandService = new CommandService(new CommandServiceConfig() {
				DefaultRunMode = RunMode.Async,
				LogLevel = LogSeverity.Debug,
				CaseSensitiveCommands = false
			});

			await CommandService.AddModulesAsync(Assembly.GetEntryAssembly());

			ServiceCollection = new ServiceCollection()
				.AddLogging()
				.AddSingleton(Client)
				.AddSingleton(CommandService)
				.AddSingleton(new AudioService())
				.AddSingleton(new ReportHandler())
				.AddSingleton(new ReminderService(Client))
				.AddSingleton(new DealService())
				.AddDbContext<DatabaseContext>()
				.BuildServiceProvider();

			Console.Title = "Inquisition";

			Client.MessageReceived += HandleCommands;
		}

        private async Task HandleCommands(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

			string prefix = GetGuildPrefix(message) ?? BotInfo.DefaultPrefix;
			int argPos = 0;

			bool messageHasMention = message.HasMentionPrefix(Client.CurrentUser, ref argPos);
			bool messageHasPrefix = message.HasStringPrefix(prefix, ref argPos);

			if (messageHasMention || messageHasPrefix)
            {
                SocketCommandContext context = new SocketCommandContext(Client, message);
                IResult result = await CommandService.ExecuteAsync(context, argPos, ServiceCollection);

                if (!result.IsSuccess)
                {
                    await ReportHandler.Report(result.ErrorReason, msg);
                }
            }
        }

		private string GetGuildPrefix(SocketUserMessage message)
		{
			try
			{
				var guildChannel = message.Channel as SocketGuildChannel;
				string socketGuildId = guildChannel.Guild.Id.ToString();
				return PrefixHandler.GetPrefix(socketGuildId);
			}
			catch (InquisitionNotFoundException e)
			{
				LogHandler.WriteLine(LogTarget.Console, e);
				return null;
			}
		}
	}
}
