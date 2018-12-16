using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class SettingsModule : Module
    {
        public SettingsModule()
        {

        }

        [Command("audit channel")]
        [Alias("default audit channel")]
        public async Task ShowDefaultAuditChannelAsync()
        {
            SocketTextChannel channel = Context.Client.GetChannel(Guild.AuditChannelId) as SocketTextChannel;

            if (channel is null)
            {
                await ReplyAsync("No default audit channel set for this guild");
                return;
            }

            EmbedBuilder embed = new EmbedBuilder().Create(channel);

            await ReplyAsync(embed);
        }

        [Command("prefix")]
        public async Task ShowPrefixAsync()
        {
            string prefix = Prefix.Get(Context.Guild.Id);

            if (prefix is null)
            {
                await ReplyAsync("Your guild is not in the database for some reason...");
            }
            else
            {
                await ReplyAsync($"Prefix for this guild is: **{prefix}**");
            }
        }
    }

    [Group("set")]
    public class SetSettingsModule : Module
    {
        public SetSettingsModule()
        {

        }

        [Command("audit channel")]
        [Alias("default audit channel")]
        public async Task SetDefaultAuditChannelAsync(SocketGuildChannel channel)
        {
            Guild.AuditChannelId = channel.Id;
            await ReplyAsync("Success");
        }

        [Command("prefix")]
        public async Task SetDefaultPrefix(string newPrefix)
        {
            string currentPrefix = Guild.Prefix;

            Prefix.Set(Context.Guild.Id, newPrefix);

            await ReplyAsync($"Prefix was **{currentPrefix}**, now changed to **{newPrefix}**");
        }
    }
}
