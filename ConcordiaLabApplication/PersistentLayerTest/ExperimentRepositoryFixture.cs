using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayerTest
{
    public class ExperimentRepositoryFixture : IDisposable
    {
        public ExperimentRepository ExperimentRepository { get; }
        public ConcordiaDbContext DbContext { get; }

        public ExperimentRepositoryFixture()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ConcordiaDbContext>()
                .UseInMemoryDatabase("ConcordiaLab")
                .Options;

            DbContext = new ConcordiaDbContext(dbContextOptions);
            ExperimentRepository = new ExperimentRepository(DbContext);
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
