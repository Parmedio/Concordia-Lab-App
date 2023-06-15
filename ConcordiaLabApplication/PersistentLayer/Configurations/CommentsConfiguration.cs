using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PersistentLayer.Models;

namespace PersistentLayer.Configurations;

public class CommentsConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> entity)
    {
        entity
            .HasIndex(c => c.TrelloId)
            .IsUnique();
    }
}
