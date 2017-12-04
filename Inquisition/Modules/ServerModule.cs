using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    [Group("server")]
    public class ServerModule : ModuleBase<SocketCommandContext>
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
