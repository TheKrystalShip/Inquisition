using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Database.SQLite
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {

        }
    }
}
