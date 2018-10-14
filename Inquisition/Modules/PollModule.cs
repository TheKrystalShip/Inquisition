using Discord;
using Discord.Commands;

using System.Collections.Generic;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Modules
{
    public class PollModule : Module
    {
        private readonly IList<IEmote> _reactions = 
            new List<IEmote>
            {
                new Emoji("👍🏻"),
                new Emoji("👎🏻"),
                new Emoji("🤷🏻")
            };

        public PollModule(Tools tools) : base(tools)
        {

        }

        [Command("poll")]
        [Alias("poll:")]
        [Summary("Create a poll")]
        public async Task CreatePollAsync([Remainder] string question = "")
        {
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(1).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);

            EmbedBuilder embed = new EmbedBuilder()
                .Create(Context.User)
                .WithTitle(question)
                .WithFooter($"Asked by {Context.User.Username}", Context.User.GetAvatarUrl() ?? null);

            IUserMessage message = await ReplyAsync("", false, embed.Build());

            foreach (Emoji e in _reactions)
            {
                await message.AddReactionAsync(e).ConfigureAwait(false);
                await Task.Delay(100);
            }
        }
    }
}
