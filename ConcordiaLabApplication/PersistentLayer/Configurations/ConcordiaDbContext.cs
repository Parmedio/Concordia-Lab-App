using Microsoft.EntityFrameworkCore;

namespace PersistentLayer.Configurations;

public class ConcordiaDbContext : DbContext
{
    public ConcordiaDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

}
