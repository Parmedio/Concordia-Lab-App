using Microsoft.EntityFrameworkCore;
using PersistentLayer.Models;

namespace PersistentLayer.Configurations;

public class ConcordiaDbContext : DbContext
{
    public DbSet<Scientist> Scientists { get; set; } = null!;
    public DbSet<Experiment> Experiments { get; set; } = null!;
    public DbSet<Column> Columns { get; set; } = null!;
    public DbSet<Label> Labels { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    public ConcordiaDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CommentsConfiguration());
        modelBuilder.ApplyConfiguration(new ExperimentsConfiguration());
        modelBuilder.ApplyConfiguration(new LabelsConfiguration());
        modelBuilder.ApplyConfiguration(new ScientistsConfiguration());
        modelBuilder.ApplyConfiguration(new ColumnsConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
