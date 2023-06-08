using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;
using PersistentLayer.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayerTest
{
    public class ScientistRepositoryTests
    {
        private readonly ScientistRepository _scientistRepository;
        private readonly ConcordiaDbContext _dbContext;

        public ScientistRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ConcordiaDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ConcordiaDbContext(options);
            _scientistRepository = new ScientistRepository(_dbContext);
        }

        [Fact]
        public void Should_Retrun_LocalId_By_TrelloId()
        {
            var existingScientist = new Scientist {Name = "gabriele", TrelloToken = "evrevr4545bgrbt", TrelloMemberId = "ece57v3" };
            _dbContext.Scientists.Add(existingScientist);
            _dbContext.SaveChanges();

            var scientistId = _scientistRepository.GetLocalIdByTrelloId("ece57v3");
            Assert.NotNull(scientistId);
        }
    }
}
