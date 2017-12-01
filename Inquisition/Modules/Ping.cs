using Discord.Commands;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Yup, this works fine");
        }
    }
}
