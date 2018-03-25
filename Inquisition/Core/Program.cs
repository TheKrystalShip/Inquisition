using Discord;
using Discord.WebSocket;

using Inquisition.Handlers;
using Inquisition.Properties;

using System.Threading.Tasks;

namespace Inquisition
{
	public class Program
    {
		private string Token;
		private DiscordSocketClient Client;

		private CommandHandler CommandHandler;
		private EventHandler EventHandler;
		private ServiceHandler ServiceHandler;
		private PrefixHandler PrefixHandler;

		static void Main(string[] args)
			=> new Program().Init().Wait();

        private async Task Init()
        {
			Token = BotInfo.Token;

			Client = new DiscordSocketClient(new DiscordSocketConfig() {
				LogLevel = LogSeverity.Debug,
				DefaultRetryMode = RetryMode.AlwaysRetry,
				ConnectionTimeout = 5000,
				AlwaysDownloadUsers = true
			});

			CommandHandler = new CommandHandler(Client);
			EventHandler = new EventHandler(Client);
			ServiceHandler = new ServiceHandler(Client);
			PrefixHandler = new PrefixHandler();

			await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();
            await Client.SetGameAsync($"@Inquisition help");

            await Task.Delay(-1);
        }
    }
}
