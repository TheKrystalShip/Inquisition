using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Database;
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
        private readonly IServiceProvider ServiceCollection;

		public CommandHandler(DiscordSocketClient client)
        {
            Client = client;

            CommandService = new CommandService(new CommandServiceConfig()
                {
                    DefaultRunMode = RunMode.Async,
                    LogLevel = LogSeverity.Debug,
                    CaseSensitiveCommands = false
                }
            );

            CommandService.AddModulesAsync(Assembly.GetEntryAssembly());

            AudioService audioService = new AudioService();
            ReportHandler reportHandler = new ReportHandler();
            ReminderService reminderService = new ReminderService(Client);
            //DealService dealService = new DealService();
            ActivityService activityService = new ActivityService();

            ServiceCollection = new ServiceCollection()
                .AddDbContext<DatabaseContext>()
                .AddLogging()
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .AddSingleton(audioService)
                .AddSingleton(reportHandler)
                .AddSingleton(reminderService)
                //.AddSingleton(dealService)
                .AddSingleton(activityService)
                .BuildServiceProvider();

            Console.Title = "Inquisition";

            Client.MessageReceived += HandleCommands;
        }

        private async Task HandleCommands(SocketMessage msg)
        {
			SocketUserMessage message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

			//string prefix = GetGuildPrefix(message) ?? BotInfo.DefaultPrefix;
			int argPos = 0;

			bool messageHasMention = message.HasMentionPrefix(Client.CurrentUser, ref argPos);
			//bool messageHasPrefix = message.HasStringPrefix(prefix, ref argPos);

			if (!messageHasMention)
				return;

            SocketCommandContext context = new SocketCommandContext(Client, message);
            IResult result = await CommandService.ExecuteAsync(context, argPos, ServiceCollection);

            if (!result.IsSuccess)
                await ReportHandler.Report(result.ErrorReason, message);
        }

		private string GetGuildPrefix(SocketUserMessage message)
		{
			var guildChannel = message.Channel as SocketGuildChannel;
			string socketGuildId = guildChannel.Guild.Id.ToString();
			return PrefixHandler.GetPrefix(socketGuildId);
		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
