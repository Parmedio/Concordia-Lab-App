using PersistentLayer.Configurations;
using PersistentLayer.Repositories.Concrete;

namespace PersistentLayerTest
{
    public class ListReposiotoryTests
    {
        private readonly ListRepository _sut;
        private readonly ConcordiaDbContext _dbContext;

        public ListReposiotoryTests()
        {
            _dbContext = new TestDatabaseFixture().CreateContext();
            _sut = new ListRepository(_dbContext);
        }

        [Fact]
        public void Should_Return_All_Lists()
        {
            var lists = _sut.GetAll();
            Assert.Equal(3, lists.Count());
            Assert.Equal("to do", lists.First().Title);
            Assert.True(lists.All(l => l.Experiments.Any()));
            Assert.True(lists.All(l => l.Experiments.All(e => e.Scientists.Any())));
        }

        [Fact]
        public void Shoul_Return_Lists_Of_A_Scientist_By_ScientisId()
        {
            var lists = _sut.GetByScientistId(1);

            Assert.NotNull(lists);

            Assert.Equal(3, lists.Count());
            Assert.True(lists.All(l => l.Experiments.Any(e => e.Scientists.Any(s => s.Id == 1))));
        }     
    }
}
