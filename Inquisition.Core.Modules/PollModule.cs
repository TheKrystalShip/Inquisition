using Discord;
using Discord.Commands;

using System.Collections.Generic;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Tools;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class PollModule : Module
    {
        private readonly IList<IEmote> _reactions = new List<IEmote> { new Emoji("👍🏻"), new Emoji("👎🏻"), new Emoji("🤷🏻") };

        [Command("poll")]
        [Alias("poll:")]
        [Summary("Create a poll")]
        public async Task CreatePollAsync([Remainder] string question)
        {
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(1).FlattenAsync();
            await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);

            Embed embed = EmbedFactory.Create(ResultType.Info, builder => {
                builder.WithTitle(question);
                builder.WithFooter($"Asked by {Context.User.Username}", Context.User.GetAvatarUrl() ?? null);
            });

            IUserMessage message = await ReplyAsync(embed);

            foreach (Emoji reaction in _reactions)
            {
                await message.AddReactionAsync(reaction);
                await Task.Delay(250);
            }
        }
    }
}
