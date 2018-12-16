using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using TheKrystalShip.Inquisition.Database.SQLite;
using TheKrystalShip.Inquisition.Tools;

namespace TheKrystalShip.Inquisition.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SQLiteContext>
    {
        public SQLiteContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<SQLiteContext> builder = new DbContextOptionsBuilder<SQLiteContext>();
            builder.UseSqlite(Configuration.GetConnectionString("SQLite"));
            return new SQLiteContext(builder.Options);
        }
    }
}
