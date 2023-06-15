using PersistentLayer.Configurations;
using PersistentLayer.Repositories.Concrete;

namespace PersistentLayerTest
{
    public class ScientistRepositoryTests
    {
        private readonly ScientistRepository _sut;
        private readonly ConcordiaDbContext _dbContext;

        public ScientistRepositoryTests()
        {
            _dbContext = new TestDatabaseFixture().CreateContext();
            _sut = new ScientistRepository(_dbContext);
        }

        [Fact]
        public void Should_Retrun_LocalId_By_TrelloId()
        {
            var scientistId = _sut.GetLocalIdByTrelloId("3434fv");
            Assert.Equal(4, scientistId);
        }

        [Fact]
        public void Should_Retrun_Null_By_TrelloId_Not_Existing()
        {
            var scientistId = _sut.GetLocalIdByTrelloId("trelloIdNotExisting");
            Assert.Null(scientistId);
        }
    }
}
