using Microsoft.EntityFrameworkCore;
using PersistentLayer.Models;

namespace PersistentLayer.Configurations;

public class ConcordiaDbContext : DbContext
{
    public DbSet<Scientist> Scientist { get; set; } = null!;
    public DbSet<Experiment> Experiments { get; set; } = null!;
    public DbSet<ListEntity> Lists { get; set; } = null!;
    public DbSet<Label> Labels { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    public ConcordiaDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
