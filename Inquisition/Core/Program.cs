using Discord;
using Discord.WebSocket;

using Inquisition.Handlers;
using Inquisition.Properties;
using Inquisition.Services;

using System.Threading.Tasks;

namespace Inquisition.Core
{
	public class Program
    {
		private DiscordSocketClient Client;
		
		private CommandHandler CommandHandler;
		private EventHandler EventHandler;
		private ThreadHandler ThreadHandler;
		private PrefixHandler PrefixHandler;
        private string Token = BotInfo.Token;

		static void Main(string[] args) 
            => new Program().Run().GetAwaiter().GetResult();

        public async Task Run()
        {
            try
            {
				Client = new DiscordSocketClient();

				CommandHandler = new CommandHandler(Client);
				EventHandler = new EventHandler(Client);
				ThreadHandler = new ThreadHandler(Client);
				PrefixHandler = new PrefixHandler();

				await Client.LoginAsync(TokenType.Bot, Token);
                await Client.StartAsync();
                await Client.SetGameAsync($"@Inquisition help");
			}
            catch (System.Exception e)
            {
				ReportService.Report(e);
            }

            await Task.Delay(-1);
        }
    }
}
