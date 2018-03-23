using Inquisition.Database.Core;

using Xunit;

namespace Inquisition.Tests.Database
{
	public class SelectTest
    {
		private DatabaseContext db;

		public SelectTest()
		{
			db = new DatabaseContext();
		}

		[Theory]
		[InlineData("245717107596197888")]
		[InlineData("143792810368303104")]
		[InlineData("241980504344100885")]
		public void SelectUser(string id)
		{
			Assert.NotNull(db.Users.Find(id));
		}
    }
}
