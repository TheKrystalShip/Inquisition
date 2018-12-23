using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Database
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany(x => x.Activities)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
