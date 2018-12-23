using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace TheKrystalShip.Inquisition.Core
{
    public class Bot : DiscordSocketClient
    {
        public Bot(DiscordSocketConfig config) : base(config)
        {

        }

        public async Task InitAsync(string token)
        {
            await LoginAsync(TokenType.Bot, token);
            await StartAsync();
            await SetGameAsync("God");
            await SetStatusAsync(UserStatus.Online);
        }
    }
}
