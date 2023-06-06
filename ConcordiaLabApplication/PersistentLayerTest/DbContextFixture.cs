using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayerTest
{
    public class DbContextFixture : IDisposable
    {
        public ConcordiaDbContext DbContext { get; }

        public DbContextFixture()
        {
            var options = new DbContextOptionsBuilder<ConcordiaDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            DbContext = new ConcordiaDbContext(options);
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
