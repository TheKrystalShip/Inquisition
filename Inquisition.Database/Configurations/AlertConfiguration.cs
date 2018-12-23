using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Database
{
    public class AlertConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany(x => x.Alerts)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.TargetUser)
                .WithMany(x => x.TargetAlerts);
        }
    }
}
