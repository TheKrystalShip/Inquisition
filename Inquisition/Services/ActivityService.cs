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
	public class ActivityService : IService
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
			//List<Activity> Activities = GetActivityList();

			Tick?.Invoke(this, EventArgs.Empty);
		}

		public void StopLoop()
		{
			Timer.Dispose();
			Stop?.Invoke(this, EventArgs.Empty);
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
