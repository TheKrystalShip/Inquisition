using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Inquisition.Handlers;
using Microsoft.EntityFrameworkCore;
using Inquisition.Properties;
using Inquisition.Data;

namespace Inquisition
{
    class Program
    {
        private DiscordSocketClient Client;
        private CommandHandler Commands;
        private EventHandler EventHandler;
        private string Token = Resources.Token;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using (DbContext context = new InquisitionContext())
            {
                context.Database.Migrate();
            }

            Client = new DiscordSocketClient();
            Commands = new CommandHandler(Client);
            EventHandler = new EventHandler(Client);

            await Client.SetGameAsync($"@Inquisition help");
            
            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }
    }
}