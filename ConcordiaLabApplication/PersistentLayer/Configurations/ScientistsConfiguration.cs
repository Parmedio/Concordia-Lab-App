using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PersistentLayer.Models;

namespace PersistentLayer.Configurations;

public class ScientistsConfiguration : IEntityTypeConfiguration<Scientist>
{
    public void Configure(EntityTypeBuilder<Scientist> entity)
    {
        entity.HasData(
            new Scientist(TrelloToken: "ATTA5c0a0bf47c1be3f495ebb81c42316684ff55e1134be71c0eba2cbecdd0614558CDCC81F8", Name: "alessandroferlugaubaldini", TrelloMemberId: "5bf9f901921c336b20b29d25"),
            new Scientist(TrelloToken: ""

            )
    }
}
