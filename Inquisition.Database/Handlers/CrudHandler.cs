using Inquisition.Database.Models;

using System;
using System.Linq;

namespace Inquisition.Database.Handlers
{
	public class CrudHandler : IDisposable
    {
		private DbHandler db;

		public CrudHandler() => db = new DbHandler();
		public void Dispose() => db.Dispose();

		public bool Exists<T>(T data) where T : class
		{
			if (db.Set<T>().FirstOrDefault(x => x == data) is null)
			{
				return false;
			}
			else return true;
		}

		// TODO: Select

		public Result Insert<T>(T data) where T : class
		{
			try
			{
				if (!Exists(data))
				{
					db.Set<T>().Add(data);
					db.SaveChanges();
					return Result.Successful;
				}

				return Result.Exists;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return Result.Failed;
				throw e;
			}
		}
		
		public Result Update<T>(T data) where T : class
		{
			try
			{
				db.Set<T>().Update(data);
				db.SaveChanges();

				return Result.Successful;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return Result.Failed;
				throw e;
			}
		}
		
		public Result Delete<T>(T data) where T : class
		{
			try
			{
				db.Set<T>().Remove(data);
				db.SaveChanges();

				return Result.Successful;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return Result.Failed;
				throw e;
			}
		}
	}
}
