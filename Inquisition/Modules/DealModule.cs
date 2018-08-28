using Discord;
using Discord.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Data.Models;
using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Database.Models;
using TheKrystalShip.Inquisition.Handlers;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Modules
{
    public class DealModule : ModuleBase<SocketCommandContext>
    {
        private readonly DatabaseContext _dbContext;
        private readonly ReportHandler _reportHandler;
        private readonly ILogger<DealModule> _logger;

        public DealModule(DatabaseContext dbContext, ReportHandler reportHandler, ILogger<DealModule> logger)
        {
            _dbContext = dbContext;
            _reportHandler = reportHandler;
            _logger = logger;
        }

        [Command("add deal")]
        [Summary("Adds a deal")]
        public async Task AddDealAsync(string url, string timeLeft = null)
        {
            try
            {
                IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(1).Flatten();
                await Context.Channel.DeleteMessagesAsync(messages);
                IUserMessage reply = await ReplyAsync("Adding your deal...");

                User localUser = _dbContext.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());

                Deal deal = new Deal
                {
                    Url = url,
                    User = localUser
                };

                if (timeLeft != null)
                {
                    TimeSpan.TryParse(timeLeft, out TimeSpan expires);
                    deal.ExpireDate = DateTime.Now.Add(expires);
                }

                EmbedBuilder embed = EmbedHandler.Create(deal, (messages.First() as IUserMessage));

                IUserMessage msg = await ReplyAsync(ReplyHandler.Context(Result.Successful), false, embed.Build());

                deal.MessageId = msg.Id.ToString();

                _dbContext.Deals.Add(deal);
                _dbContext.SaveChanges();

            }
            catch (Exception e)
            {
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
            }
        }

        [Command("deals")]
        [Summary("Returns all active deals")]
        public async Task ShowDealsAsync()
        {
            List<Deal> dealList = _dbContext.Deals.ToList();

            EmbedBuilder embed = EmbedHandler.Create(dealList);

            await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
        }
    }
}
