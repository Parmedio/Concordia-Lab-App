using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PersistentLayer.Models;

namespace PersistentLayer.Configurations;

public class ExperimentsConfiguration : IEntityTypeConfiguration<Experiment>
{
    public void Configure(EntityTypeBuilder<Experiment> entity)
    {
        entity
            .HasIndex(p => p.TrelloId)
            .IsUnique(true);
    }
}
