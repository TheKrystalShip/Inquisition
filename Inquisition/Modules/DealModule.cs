using Discord;
using Discord.Commands;

using Inquisition.Data.Models;
using Inquisition.Database.Core;
using Inquisition.Database.Models;
using Inquisition.Handlers;
using Inquisition.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	public class DealModule : ModuleBase<SocketCommandContext>
    {
		private DatabaseContext db;

		public DealModule(DatabaseContext dbHandler) => db = dbHandler;

		[Command("add deal")]
		[Summary("Adds a deal")]
		public async Task AddDealAsync(string url, string timeLeft = null)
		{
			try
			{
				var messages = await Context.Channel.GetMessagesAsync(1).Flatten();
				await Context.Channel.DeleteMessagesAsync(messages);
				var reply = await ReplyAsync("Adding your deal...");

				User localUser = db.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());

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

				var msg = await ReplyAsync(ReplyHandler.Context(Result.Successful), false, embed.Build());

				deal.MessageId = msg.Id.ToString();

				db.Deals.Add(deal);
				db.SaveChanges();

			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				LogHandler.WriteLine(e);
				ReportService.Report(Context, e);
			}
		}

		[Command("deals", RunMode = RunMode.Async)]
		[Summary("Returns all active deals")]
		public async Task ShowDealsAsync()
		{
			List<Deal> dealList = db.Deals.ToList();

			EmbedBuilder embed = EmbedHandler.Create(dealList);

			await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
		}
    }
}
