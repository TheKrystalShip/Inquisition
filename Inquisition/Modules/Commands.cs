using Discord;
using Discord.Commands;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inquisition.Modules
{

    /*
    |===========================================|
    | Game Server     | Port | Version | Modded |
    |===========================================|
    | Space Engineers | 3080 |         | NO     |
    | StarMade        | 3070 | .654    | NO     |
    | Project Zomboid | 3050 | 37.14   | YES    |
    | Starbound       | 3040 | 1.2.2   | NO     |
    | Terraria        | 3030 | 1.3.5.3 | NO     |
    | Factorio        | 3020 | 15.18   | NO     |
    | 7 Days To Die   | 3010 | 16 b138 | NO     |
    | GMod Sandbox    | 3003 |         | NO     |
    | GMod Murder     | 3000 |         | NO     |
    |===========================================|
    */

    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("servers")]
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
        public async Task WipeChannelAsync()
        {

        }
    }
}
