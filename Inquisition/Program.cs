using Discord;
using Discord.WebSocket;
using Inquisition.Data;
using Inquisition.Handlers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Inquisition.Properties;

namespace Inquisition
{
    class Program
    {
        private DiscordSocketClient Client;
        private CommandHandler CommandHandler;
        private EventHandler EventHandler;
        private string Token = Res.Token;

        static void Main(string[] args) 
            => new Program().Run().GetAwaiter().GetResult();

        public async Task Run()
        {
            using (DbContext db = new DatabaseContext())
            {
                db.Database.Migrate();
            }

            Client = new DiscordSocketClient();
            CommandHandler = new CommandHandler(Client);
            EventHandler = new EventHandler(Client);

            try
            {
                await Client.SetGameAsync($"@Inquisition help");

                await Client.LoginAsync(TokenType.Bot, Token);
                await Client.StartAsync();
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }

            await Task.Delay(-1);
        }
    }
}
