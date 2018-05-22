using Inquisition.Services.Core;

namespace Inquisition.Services
{
	public class DealService : Service
	{
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

		//private List<Deal> GetDeals()
		//{
		//	DatabaseContext db = new DatabaseContext();
		//	return db.Deals
		//		.Include(x => x.User)
		//		.ToList() ?? new List<Deal>();
		//}
	}
}
