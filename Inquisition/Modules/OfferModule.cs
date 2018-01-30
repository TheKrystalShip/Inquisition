using Discord;
using Discord.Commands;

using Inquisition.Data.Handlers;
using Inquisition.Data.Models;
using Inquisition.Handlers;
using Inquisition.Services;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	public class OfferModule : ModuleBase<SocketCommandContext>
    {
		private DbHandler db;
		private OfferService OfferService;

		public OfferModule(DbHandler dbHandler, OfferService offerService)
		{
			db = dbHandler;
			OfferService = offerService;
		}

		[Command("add offer")]
		[Summary("Adds an offer")]
		public async Task AddOfferAsync(string url, string timeLeft = null)
		{
			try
			{
				var messages = await Context.Channel.GetMessagesAsync(1).Flatten();
				await Context.Channel.DeleteMessagesAsync(messages);

				User localUser = db.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());

				Offer offer = new Offer
				{
					Url = url,
					User = localUser
				};

				if (timeLeft != null)
				{
					TimeSpan.TryParse(timeLeft, out TimeSpan expires);
					offer.ExpireDate = DateTime.Now.Add(expires);
				}

				//db.Offers.Add(offer);
				//db.SaveChanges();

				EmbedBuilder embed = EmbedHandler.Create(offer);

				var msg = await ReplyAsync(ReplyHandler.Context(Result.Successful), false, embed.Build());
				OfferService.AddOfferMessage(msg, offer);
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				Console.WriteLine(e);
				ReportService.Report(Context, e);
			}
		}
    }
}
