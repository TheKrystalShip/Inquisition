using Discord;
using Discord.WebSocket;
using Inquisition.Data;
using Inquisition.Handlers;
using Inquisition.Properties;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Inquisition
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
            using (DbContext db = new DatabaseContext())
            {
                db.Database.Migrate();
            }

            Client = new DiscordSocketClient();
            CommandHandler = new CommandHandler(Client);
            EventHandler = new EventHandler(Client);

            await Client.SetGameAsync($"@Inquisition help");

            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
