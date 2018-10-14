using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Services
{
    public class ActivityService : Service
    {
        public override Timer Timer { get; protected set; }
        public override event Action<Service> Start;
        public override event Action<Service> Stop;
        public override event Action<Service> Tick;

        public ActivityService(Tools tools) : base(tools)
        {

        }

        public override void Init(int startDelay = 0, int interval = 1000)
        {
            Timer = new Timer(Loop, null, startDelay, interval);
            Start?.Invoke(this);
        }

        protected override void Loop(object state)
        {

            Tick?.Invoke(this);
        }

        public override void Dispose()
        {
            Stop?.Invoke(this);
        }

        public List<Activity> GetActivityList()
        {
            return Tools.Database.Activities
                .Include(x => x.Guild)
                .Include(x => x.User)
                .ToList() ?? new List<Activity>();
        }
    }
}
