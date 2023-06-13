using Microsoft.EntityFrameworkCore;

using PersistentLayer.Models;

namespace PersistentLayer.Configurations;

public class ConcordiaDbContext : DbContext
{
    public DbSet<Scientist> Scientists { get; set; } = null!;
    public DbSet<Experiment> Experiments { get; set; } = null!;
    public DbSet<ListEntity> EntityLists { get; set; } = null!;
    public DbSet<Label> Labels { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    public ConcordiaDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LabelsConfiguration());

        modelBuilder.Entity<Comment>()
            .HasIndex(c => c.TrelloId)
            .IsUnique();
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ListEntity>().HasData(
                   new ListEntity { Id = 1, TrelloId = "64760804e47275c707e05d38", Title = "to do" },
                   new ListEntity { Id = 2, TrelloId = "64760804e47275c707e05d39", Title = "in progress" },
                   new ListEntity { Id = 3, TrelloId = "64760804e47275c707e05d3a", Title = "completed" }
                   );
    }
}
