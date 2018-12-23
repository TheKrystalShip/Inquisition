﻿using Discord;
using Discord.Commands;

using System.Collections.Generic;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Core.Modules
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

        public PollModule()
        {

        }

        [Command("poll")]
        [Alias("poll:")]
        [Summary("Create a poll")]
        public async Task CreatePollAsync([Remainder] string question = "")
        {
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(1).FlattenAsync();
            await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);

            EmbedBuilder embedBuilder = new EmbedBuilder()
                .Create(Context.User)
                .WithTitle(question)
                .WithFooter($"Asked by {Context.User.Username}", Context.User.GetAvatarUrl() ?? null);

            IUserMessage message = await ReplyAsync(embedBuilder);

            foreach (Emoji reaction in _reactions)
            {
                await message.AddReactionAsync(reaction);
                await Task.Delay(250);
            }
        }
    }
}
