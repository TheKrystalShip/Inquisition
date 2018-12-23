using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Database
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasMany(x => x.Jokes)
                .WithOne(x => x.User);

            builder.HasMany(x => x.Reminders)
                .WithOne(x => x.User);

            builder.HasMany(x => x.Alerts)
                .WithOne(x => x.User);

            builder.HasMany(x => x.TargetAlerts)
                .WithOne(x => x.TargetUser);

            builder.HasMany(x => x.Deals)
                .WithOne(x => x.User);

            builder.HasMany(x => x.Servers)
                .WithOne(x => x.User);

            builder.HasMany(x => x.Activities)
                .WithOne(x => x.User);

            builder.Property(x => x.Id)
                .HasConversion<string>();
        }
    }
}
