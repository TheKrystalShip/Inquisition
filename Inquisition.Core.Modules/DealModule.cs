using Discord;
using Discord.Commands;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Tools;

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

            Embed embed = EmbedFactory.Create(ResultType.Success, builder => {
                builder.WithTitle((messages.First() as IUserMessage).Content);
            });

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

            if (dealList.Count is 0)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "No deals were found");
            }

            Embed embed = EmbedFactory.Create(ResultType.Success, builder => {
                builder.WithTitle("Here's the deals");

                foreach (Deal deal in dealList)
                {
                    builder.AddField($"{deal.Url}", $"{deal.ExpireDate}");
                }
            });

            return new SuccessResult("Success", embed);
        }
    }
}
