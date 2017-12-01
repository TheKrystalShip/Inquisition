using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inquisition.Data;
using System.Linq;

namespace Inquisition.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("servers")]
        [Summary("Displays all the Game Servers with the corresponding port")]
        public async Task ServersAsync()
        {
            InquisitionContext db = new InquisitionContext();
            List<Data.Game> Games = db.Games.ToList();
            EmbedBuilder builder = new EmbedBuilder();

            foreach (Data.Game game in Games)
            {
                builder.AddInlineField(game.Name, game.Port);
            }

            await ReplyAsync("Domain: **ffs.game-host.org**", false, builder.Build());
        }

        [Command("wipe")]
        [Summary("Wipes a text channel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task WipeChannelAsync(uint amount = 1)
        {
            var messages = await Context.Channel.GetMessagesAsync((int)amount + 1).Flatten();

            await Context.Channel.DeleteMessagesAsync(messages);
            const int delay = 5000;
            var m = await ReplyAsync($"Deleted {amount} messages. _This message will be deleted in {delay / 1000} seconds._");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }
    }
}
