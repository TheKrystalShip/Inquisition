using Discord;
using Discord.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Data.Models;
using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Extensions;
using TheKrystalShip.Inquisition.Handlers;

namespace TheKrystalShip.Inquisition.Modules
{
    public class DealModule : Module
    {
        public DealModule(Tools tools) : base(tools)
        {

        }

        [Command("add deal")]
        [Summary("Adds a deal")]
        public async Task AddDealAsync(string url, string timeLeft = null)
        {
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(1).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);
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

            IUserMessage message = await ReplyAsync(ReplyHandler.Context(Result.Successful), false, embed.Build());

            deal.MessageId = message.Id;

            Database.Deals.Add(deal);
        }

        [Command("deals")]
        [Summary("Returns all active deals")]
        public async Task ShowDealsAsync()
        {
            List<Deal> dealList = Database.Deals.ToList();

            EmbedBuilder embed = new EmbedBuilder().Create(dealList);

            await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
        }
    }
}
