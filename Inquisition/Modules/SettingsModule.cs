using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Data.Models;
using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Extensions;
using TheKrystalShip.Inquisition.Handlers;

namespace TheKrystalShip.Inquisition.Modules
{
    public class SettingsModule : Module
    {
        public SettingsModule(Tools tools) : base(tools)
        {

        }

        [Command("audit channel")]
        [Alias("default audit channel")]
        public async Task ShowDefaultAuditChannelAsync()
        {
            SocketTextChannel channel = Tools.Client.GetChannel(Guild.AuditChannelId) as SocketTextChannel;

            if (channel is null)
            {
                await ReplyAsync("No default audit channel set for this guild");
                return;
            }

            EmbedBuilder embed = new EmbedBuilder().Create(channel);

            await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
        }

        [Command("prefix")]
        public async Task ShowPrefixAsync()
        {
            string prefix = Tools.Prefix.Get(Context.Guild.Id);

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
        public SetSettingsModule(Tools tools) : base(tools)
        {

        }

        [Command("audit channel")]
        [Alias("default audit channel")]
        public async Task SetDefaultAuditChannelAsync(SocketGuildChannel channel)
        {
            Guild.AuditChannelId = channel.Id;
            await ReplyAsync(ReplyHandler.Context(Result.Successful));
        }

        [Command("prefix")]
        public async Task SetDefaultPrefix(string newPrefix)
        {
            string currentPrefix = Guild.Prefix;

            Tools.Prefix.Set(Context.Guild.Id, newPrefix);

            await ReplyAsync($"Prefix was **{currentPrefix}**, now changed to **{newPrefix}**");
        }
    }
}
