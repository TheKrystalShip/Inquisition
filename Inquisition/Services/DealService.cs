using Inquisition.Data.Interfaces;
using Inquisition.Database.Core;
using Inquisition.Database.Models;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Inquisition.Services
{
	public class DealService : IThreadLoop
    {
		public string Name { get; set; } = "Deal Service";
		public Timer Timer { get; set; }
		public event EventHandler LoopStarted;
		public event EventHandler LoopStopped;
		public event EventHandler LoopTick;

		public void StartLoop()
		{
			Timer = new Timer(Loop, null, 0, 1000);
			LoopStarted?.Invoke(Name, EventArgs.Empty);
		}

		public void Loop(object state)
		{
			LoopTick?.Invoke(Name, EventArgs.Empty);
		}

		public void StopLoop()
		{
			Timer.Dispose();
			LoopStopped?.Invoke(Name, EventArgs.Empty);
		}

		private List<Deal> GetDeals()
		{
			DatabaseContext db = new DatabaseContext();
			return db.Deals
				.Include(x => x.User)
				.ToList() ?? new List<Deal>();
		}
	}
}
