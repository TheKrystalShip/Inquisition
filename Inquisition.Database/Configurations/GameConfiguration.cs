using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Database
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {

        }
    }
}
