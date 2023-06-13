using Microsoft.EntityFrameworkCore;

using PersistentLayer.Configurations;

namespace ConcordiaLab
{
    public static class BootStrapper
    {
        public static async Task MigrateAsync(this WebApplication app)
        {
            var provider = app.Services.CreateScope();
            var context = provider.ServiceProvider.GetRequiredService<ConcordiaDbContext>();
            //await context.Database.MigrateAsync();
        }
    }
}
