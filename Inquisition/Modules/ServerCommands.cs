using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    [Group("server")]
    public class ServerCommands : ModuleBase<SocketCommandContext>
    {
        [Command("prune")]
        [Summary("Prunes all inactive members from the server")]
        public async Task PruneMembersAsync(int d)
        {
            var n = await Context.Guild.PruneUsersAsync(d);
            await ReplyAsync($"{n} users were pruned for inactivity in the last {d} days");
        }

        [Command("ban")]
        [Summary("Bans a user from the server")]
        public async Task KickMemberAsync(SocketUser user)
        {
            await Context.Guild.AddBanAsync(user);
            await ReplyAsync($"{user.Username} has been banned");
        }

        [Command("unban")]
        [Summary("Unbans a user")]
        public async Task UnbanMemberAsync(SocketUser user)
        {
            await Context.Guild.RemoveBanAsync(user);
            await ReplyAsync($"{user.Username} has been unbanned");
        }
    }
}
