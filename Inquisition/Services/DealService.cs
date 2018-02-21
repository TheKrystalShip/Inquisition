using Inquisition.Data.Models;
using Inquisition.Database.Core;
using Inquisition.Database.Models;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Inquisition.Services
{
	public class DealService : IService
    {
		public Timer Timer { get; set; }
		public event EventHandler Start;
		public event EventHandler Stop;
		public event EventHandler Tick;

		public void StartLoop()
		{
			Timer = new Timer(Loop, null, 0, 1000);
			Start?.Invoke(this, EventArgs.Empty);
		}

		public void Loop(object state)
		{
			Tick?.Invoke(this, EventArgs.Empty);
		}

		public void StopLoop()
		{
			Timer.Dispose();
			Stop?.Invoke(this, EventArgs.Empty);
		}

		private List<Deal> GetDeals()
		{
			DatabaseContext db = new DatabaseContext();
			return db.Deals
				.Include(x => x.User)
				.ToList() ?? new List<Deal>();
		}

		public override string ToString()
		{
			return "Deal service";
		}
	}
}
