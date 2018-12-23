using Discord;
using Discord.Commands;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class DealModule : Module
    {
        [Command("add deal")]
        [Summary("Adds a deal")]
        public async Task AddDealAsync(string url, string timeLeft = null)
        {
            IEnumerable<IMessage> messages = await (Context.Channel as ITextChannel)?.GetMessagesAsync(1)?.FlattenAsync();
            await (Context.Channel as ITextChannel)?.DeleteMessagesAsync(messages);
            IUserMessage reply = await ReplyAsync("Adding your deal...");

            Deal deal = new Deal
            {
                Url = url,
                User = User
            };

            if (timeLeft != null)
            {
                TimeSpan.TryParse(timeLeft, out TimeSpan expires);
                deal.ExpireDate = DateTime.Now.Add(expires);
            }

            EmbedBuilder embed = new EmbedBuilder()
                .Create(deal, (messages.First() as IUserMessage));

            IUserMessage message = await ReplyAsync(embed);

            deal.MessageId = message.Id;

            Database.Deals.Add(deal);
        }

        [Command("deals")]
        [Summary("Returns all active deals")]
        public async Task<RuntimeResult> ShowDealsAsync()
        {
            List<Deal> dealList = await Database.Deals
                .ToListAsync();

            if (dealList is null)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "No deals were found");
            }

            EmbedBuilder embed = new EmbedBuilder().Create(dealList);

            return new SuccessResult("Success", embed);
        }
    }
}
