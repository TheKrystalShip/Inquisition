using Inquisition.Data.Models;
using Inquisition.Database.Core;
using Inquisition.Database.Models;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Services
{
	public class DealService : BaseService
    {
		public override void StartLoop()
		{
			base.StartLoop();
		}

		public override void Loop(object state)
		{
			base.Loop(state);
		}

		public override void StopLoop()
		{
			base.StopLoop();
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
