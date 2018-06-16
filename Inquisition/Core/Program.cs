using Discord;
using Discord.WebSocket;

using Inquisition.Database;
using Inquisition.Handlers;
using Inquisition.Logging;
using Inquisition.Properties;

using System.Threading.Tasks;

namespace Inquisition
{
    public class Program
    {
		private static string Token;
		private static DiscordSocketClient Client;

		private static CommandHandler CommandHandler;
		private static EventHandler EventHandler;
		private static ServiceHandler ServiceHandler;
		private static PrefixHandler PrefixHandler;

		private static DatabaseContext DatabaseContext;

        static async Task Main(string[] args)
		{
			Token = BotInfo.Token;

			Client = new DiscordSocketClient(new DiscordSocketConfig()
				{
					LogLevel = LogSeverity.Info,
					DefaultRetryMode = RetryMode.AlwaysRetry,
					ConnectionTimeout = 5000,
					AlwaysDownloadUsers = true
				}
			);

			CommandHandler = new CommandHandler(Client);
			EventHandler = new EventHandler(Client);
			ServiceHandler = new ServiceHandler();
			PrefixHandler = new PrefixHandler();

			try
			{
				DatabaseContext = new DatabaseContext();
				DatabaseContext.Migrate();
			}
			catch (System.Exception e)
			{
				ReportHandler.Report(e);
				LogHandler.WriteLine(LogTarget.Console, "Program", "Failed to migrate database");
			}

            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();
            await Client.SetGameAsync($"God");

            await Task.Delay(-1);
        }
    }
}
