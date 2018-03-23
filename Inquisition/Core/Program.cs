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
		private string Token;
		
		private CommandHandler CommandHandler;
		private EventHandler EventHandler;
		private ServiceHandler ServiceHandler;
		private PrefixHandler PrefixHandler;

		static void Main(string[] args)
			=> new Program().Run();

        private async void Run()
        {
            try
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
			}
            catch (System.Exception e)
            {
				ReportService.Report(e);
            }

            await Task.Delay(-1);
        }
    }
}
