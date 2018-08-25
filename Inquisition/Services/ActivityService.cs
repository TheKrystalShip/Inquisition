using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Database.Models;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Services
{
    public class ActivityService : Service
    {
        private readonly ILogger<ActivityService> _logger;

        public ActivityService(ILogger<ActivityService> logger)
        {
            _logger = logger;
        }

		public override void Init(int startDelay = 0, int interval = 1000)
		{
			base.Init(startDelay, interval);
		}

		public override void Loop(object state)
		{
			base.Loop(state);
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public List<Activity> GetActivityList()
		{
			DatabaseContext db = new DatabaseContext();
			return db.Activities
				.Include(x => x.Guild)
				.Include(x => x.User)
				.ToList() ?? new List<Activity>();
		}
	}
}
