using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Database.SQLite
{
    public class ServerConfiguration : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany(x => x.Servers);

            builder.HasOne(x => x.Guild)
                .WithMany(x => x.Servers);
        }
    }
}
