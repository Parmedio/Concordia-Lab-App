using PersistentLayer.Configurations;
using PersistentLayer.Repositories.Concrete;
using PersistentLayerTest;

namespace PersistentLayer.Tests
{
    public class ScientistRepositoryTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly ScientistRepository _sut;
        private readonly ConcordiaDbContext _dbContext;

        public ScientistRepositoryTests(TestDatabaseFixture database)
        {
            _dbContext = database.CreateContext();
            _sut = new ScientistRepository(_dbContext);
        }

        [Fact]
        public void Should_Retrun_LocalId_By_TrelloId()
        {
            var scientistId = _sut.GetLocalIdByTrelloId("3434fv");
            Assert.Equal(1, scientistId);
        }

        [Fact]
        public void Should_Retrun_Null_By_TrelloId_Not_Existing()
        {
            var scientistId = _sut.GetLocalIdByTrelloId("trelloIdNotExisting");
            Assert.Null(scientistId);
        }
    }
}
