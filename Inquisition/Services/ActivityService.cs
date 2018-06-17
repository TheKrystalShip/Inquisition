using Inquisition.Database;
using Inquisition.Database.Models;
using Inquisition.Database.Repositories;
using Inquisition.Logging;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Services
{
    public class ActivityService : Service
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<ActivityService> _logger;

        public ActivityService(IRepositoryWrapper repository, ILogger<ActivityService> logger)
        {
            _repository = repository;
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
