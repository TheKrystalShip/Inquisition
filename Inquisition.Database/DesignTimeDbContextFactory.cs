using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TheKrystalShip.Inquisition.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SQLiteContext>
    {
        public SQLiteContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<SQLiteContext> builder = new DbContextOptionsBuilder<SQLiteContext>();
            builder.UseSqlite("Data source=.\\Properties\\Inquisition.db");
            return new SQLiteContext(builder.Options);
        }
    }
}
