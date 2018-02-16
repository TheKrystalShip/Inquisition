using Inquisition.Data.Handlers;
using Inquisition.Data.Interfaces;
using Inquisition.Data.Models;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Inquisition.Services
{
	public class ActivityService : IThreadLoop
    {
		public string Name { get; set; } = "Activity service";
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
			//List<Activity> Activities = GetActivityList();

			LoopTick?.Invoke(Name, EventArgs.Empty);
		}

		public void StopLoop()
		{
			Timer.Dispose();
			LoopStopped?.Invoke(Name, EventArgs.Empty);
		}

		public List<Activity> GetActivityList()
		{
			DbHandler db = new DbHandler();
			return db.Activities
				.Include(x => x.Guild)
				.Include(x => x.User)
				.ToList() ?? new List<Activity>();
		}
	}
}
