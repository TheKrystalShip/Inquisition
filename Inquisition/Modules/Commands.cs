using Discord;
using Discord.Commands;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("servers")]
        [Summary("Displays all the Game Servers with the corresponding port")]
        public async Task ServersAsync()
        {
            List<Game> Games = new List<Game>
            {
                new Game { Name = "Space Engineers", Port = "3080", Version = "?" },
                new Game { Name = "StarMade", Port = "3070", Version = ".654" },
                new Game { Name = "Project Zomboid", Port = "3050", Version = "37.14" },
                new Game { Name = "Starbound", Port = "3040", Version = "1.2.2" },
                new Game { Name = "Terraria", Port = "3030", Version = "1.3.5.3" },
                new Game { Name = "Factorio", Port = "3020", Version = "15.18" },
                new Game { Name = "7 Days to die", Port = "3010", Version = "16 b138" },
                new Game { Name = "GMod - Sandbox", Port = "3003", Version = "?" },
                new Game { Name = "GMod - Murder", Port = "3000", Version = "?" }
            };

            EmbedBuilder builder = new EmbedBuilder();

            foreach (Game game in Games)
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

            await this.Context.Channel.DeleteMessagesAsync(messages);
            const int delay = 5000;
            var m = await ReplyAsync($"Deleted {amount} messages. _This message will be deleted in {delay / 1000} seconds._");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }
    }
}
