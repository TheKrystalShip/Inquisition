using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Inquisition.Data;
using System.Linq;
using Discord;

namespace Inquisition.Modules
{
    [Group("set")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class SetModule : ModuleBase<SocketCommandContext>
    {
        [Group("game")]
        public class SetGame : ModuleBase<SocketCommandContext>
        {

            [Command("name")]
            public async Task SetGameNameAsync(string name, string newName)
            {

            }

            [Command("port")]
            public async Task SetGamePortAsync(string name, string port)
            {

            }

            [Command("version")]
            public async Task SetGameVersionAsync(string name, string version)
            {

            }

            [Command("exe")]
            public async Task SetGameExeAsync(string name, string path)
            {

            }
        }
    }
}
