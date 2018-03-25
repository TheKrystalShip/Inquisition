using Discord;
using Discord.WebSocket;

using Inquisition.Handlers;
using Inquisition.Properties;

using System.Threading.Tasks;

namespace Inquisition.Core
{
	public class Program
    {
		private DiscordSocketClient Client;
		private string Token;
		
		private CommandHandler CommandHandler;
		private EventHandler EventHandler;
		private ServiceHandler ServiceHandler;
		private PrefixHandler PrefixHandler;

		static void Main(string[] args)
			=> new Program().Run().Wait();

        private async Task Run()
        {
			Client = new DiscordSocketClient();
			Token = BotInfo.Token;

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
