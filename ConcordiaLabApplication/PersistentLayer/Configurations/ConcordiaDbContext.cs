using Microsoft.EntityFrameworkCore;

using PersistentLayer.Models;

namespace PersistentLayer.Configurations;

public class ConcordiaDbContext : DbContext
{
    public DbSet<Scientist> Scientists { get; set; } = null!;
    public DbSet<Experiment> Experiments { get; set; } = null!;
    public DbSet<Column> EntityLists { get; set; } = null!;
    public DbSet<Label> Labels { get; set; } = null!;
    public DbSet<Column> Columns { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    public ConcordiaDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LabelsConfiguration());
        modelBuilder.ApplyConfiguration(new ScientistsConfiguration());

        modelBuilder.Entity<Comment>()
            .HasIndex(c => c.TrelloId)
            .IsUnique();
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Column>().HasData(
                   new Column { Id = 1, TrelloId = "64760804e47275c707e05d38", Title = "to do" },
                   new Column { Id = 2, TrelloId = "64760804e47275c707e05d39", Title = "in progress" },
                   new Column { Id = 3, TrelloId = "64760804e47275c707e05d3a", Title = "completed" }
                   );

        modelBuilder.Entity<Experiment>(entity =>
        {
            entity.HasOne(e => e.Label)
                .WithMany()
                .HasForeignKey(e => e.LabelId)
                .IsRequired(false);
        });
    }
}
