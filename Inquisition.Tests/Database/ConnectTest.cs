﻿using Inquisition.Database;

using Xunit;

namespace Inquisition.Tests.Database
{
	public class ConnectTest
    {
		private DatabaseContext db;

		public ConnectTest()
		{
			db = new DatabaseContext();
		}

		[Fact]
		public void DatabaseEnsureCreated()
		{
			
		}
    }
}