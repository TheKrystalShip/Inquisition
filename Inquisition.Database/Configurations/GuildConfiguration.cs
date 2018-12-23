using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Database
{
    public class GuildConfiguration : IEntityTypeConfiguration<Guild>
    {
        public void Configure(EntityTypeBuilder<Guild> builder)
        {
            builder.HasMany(x => x.Servers)
                .WithOne(x => x.Guild);

            builder.Property(x => x.Id)
                .HasConversion<string>();

            builder.Property(x => x.AuditChannelId)
                .HasConversion<string>();
        }
    }
}
