using Discord;

using Inquisition.Data.Handlers;
using Inquisition.Data.Interfaces;
using Inquisition.Data.Models;
using Inquisition.Handlers;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Inquisition.Services
{
	public class OfferService : IThreadLoop
    {
		private DbHandler db;
		private Timer Timer;
		public event System.EventHandler LoopStarted;
		public event System.EventHandler LoopStopped;
		public event System.EventHandler OfferAdded;

		private static Dictionary<IUserMessage, Offer> OfferMessages = new Dictionary<IUserMessage, Offer>();

		public OfferService(DbHandler dbHandler)
		{
			db = dbHandler;
		}

		public void StartLoop()
		{
			Timer = new Timer(delegate {
				foreach (KeyValuePair<IUserMessage, Offer> msg in OfferMessages)
				{
					msg.Key.ModifyAsync(x => {
						x.Embed= EmbedHandler.Create(msg.Value).Build();
					});
				}
			}, null, 0, 1000);
			LoopStarted?.Invoke(this, EventArgs.Empty);
		}

		public void StopLoop()
		{
			Timer.Dispose();
			LoopStopped?.Invoke(this, EventArgs.Empty);
		}

		private List<Offer> GetOffers()
		{
			return db.Offers
				.Include(x => x.User)
				.ToList();
		}

		public void AddOfferMessage(IUserMessage msg, Offer offer)
		{
			OfferMessages.Add(msg, offer);
			OfferAdded?.Invoke(this, EventArgs.Empty);
		}
    }
}
