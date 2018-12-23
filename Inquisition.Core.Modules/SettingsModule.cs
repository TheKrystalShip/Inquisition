using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Tools;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class SettingsModule : Module
    {
        [Command("audit channel")]
        [Alias("default audit channel")]
        public async Task<RuntimeResult> ShowDefaultAuditChannelAsync()
        {
            SocketTextChannel channel = Context.Client.GetChannel(Guild.AuditChannelId) as SocketTextChannel;

            if (channel is null)
            {
                return new ErrorResult(CommandError.UnmetPrecondition, "No default audit channel set for this guild");
            }

            Embed embed = EmbedFactory.Create(ResultType.Info, builder => {
                builder.WithTitle("Here's the audit channel");
                builder.WithDescription(channel.Mention);
            });

            return new InfoResult(embed);
        }

        [Command("prefix")]
        public async Task<RuntimeResult> ShowPrefixAsync()
        {
            string prefix = Prefix.Get(Context.Guild.Id);

            if (prefix is null)
            {
                return new ErrorResult(CommandError.UnmetPrecondition, "Your guild is not in the database for some reason...");
            }
            else
            {
                return new InfoResult($"Prefix for this guild is: **{prefix}**");
            }
        }
    }

    [Group("set")]
    public class SetSettingsModule : Module
    {
        [Command("audit channel")]
        [Alias("default audit channel")]
        public async Task<RuntimeResult> SetDefaultAuditChannelAsync(SocketGuildChannel channel)
        {
            Guild.AuditChannelId = channel.Id;
            return new SuccessResult();
        }

        [Command("prefix")]
        public async Task<RuntimeResult> SetDefaultPrefix(string newPrefix)
        {
            string currentPrefix = Guild.Prefix;

            Prefix.Set(Context.Guild.Id, newPrefix);

            return new InfoResult($"Prefix was **{currentPrefix}**, now changed to **{newPrefix}**");
        }
    }
}
