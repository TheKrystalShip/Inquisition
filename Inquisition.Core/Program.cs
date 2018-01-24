using Discord;
using Discord.WebSocket;

using Inquisition.Core.Handlers;
using Inquisition.Core.Properties;
using Inquisition.Core.Services;
using Inquisition.Data.Services;
using Inquisition.Database.Handlers;

using System.Threading.Tasks;

namespace Inquisition.Core
{
	class Program
    {
        private DiscordSocketClient Client;
        private CommandHandler CommandHandler;
        private EventHandler EventHandler;
        private string Token = Resources.Token;

        static void Main(string[] args) 
            => new Program().Run().GetAwaiter().GetResult();

        public async Task Run()
        {
            try
            {
				using (Benchmark benchmark = new Benchmark())
				using (DbHandler db = new DbHandler())
				{
					db.MigrateDatabase();
				}

				Client = new DiscordSocketClient();
				CommandHandler = new CommandHandler(Client);
				EventHandler = new EventHandler(Client);

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
