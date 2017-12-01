using Discord;
using Discord.Commands;
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

    public class ServersCommand : ModuleBase<SocketCommandContext>
    {
        [Command("servers")]
        public async Task ServersAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder
                .AddInlineField("Space Engineers", "3080")
                .AddInlineField("StarMade", "3070")
                .AddInlineField("Project Zomboid", "3050")
                .AddInlineField("Starbound", "3040")
                .AddInlineField("Terraria", "3030")
                .AddInlineField("Factorio", "3020")
                .AddInlineField("7 Days to die", "3010")
                .AddInlineField("GMod Sandbox", "3003")
                .AddInlineField("GMod Murder", "3000");

            await ReplyAsync("Domain: **ffs.game-host.org**", false, builder.Build());
        }
    }
}
