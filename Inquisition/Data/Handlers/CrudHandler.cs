using System;

namespace Inquisition.Data.Handlers
{
	[Obsolete("Still needs testing, don't use this for now", true)]
	public class CrudHandler : IDisposable
    {
		private DbHandler db;
		public CrudHandler(DbHandler dbHandler) => db = dbHandler;

		public void Insert<T>(T data) where T: class
		{
			db.Set<T>().Add(data);
		}

		public T Select<T>(T data) where T: class
		{
			return db.Set<T>().Find(data);
		}

		public void Update<T>(T data) where T: class
		{
			db.Set<T>().Update(data);
		}

		public void Delete<T>(T data) where T: class
		{
			db.Set<T>().Remove(data);
		}

		public void Dispose()
		{
			db.SaveChanges();
		}
	}
}
