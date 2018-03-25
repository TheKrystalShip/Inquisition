using Inquisition.Database.Core;
using Inquisition.Database.Models;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Services
{
	public class ActivityService : BaseService
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

		public List<Activity> GetActivityList()
		{
			DatabaseContext db = new DatabaseContext();
			return db.Activities
				.Include(x => x.Guild)
				.Include(x => x.User)
				.ToList() ?? new List<Activity>();
		}

		public override string ToString()
		{
			return "Activity service";
		}
	}
}
