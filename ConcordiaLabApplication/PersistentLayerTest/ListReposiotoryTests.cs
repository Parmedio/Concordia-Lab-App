using FluentAssertions;
using PersistentLayer.Configurations;
using PersistentLayer.Repositories.Concrete;
using PersistentLayerTest;

namespace PersistentLayer.Tests
{
    public class ListReposiotoryTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly ListRepository _sut;
        private readonly ConcordiaDbContext _dbContext;

        public ListReposiotoryTests(TestDatabaseFixture database)
        {
            _dbContext = database.CreateContext();
            _sut = new ListRepository(_dbContext);
        }

        [Fact]
        public void Should_Return_All_Lists()
        {
            var lists = _sut.GetAll();
            Assert.Equal(3, lists.Count());
            
            foreach ( var list in lists)
            {
                var e = _dbContext.Experiments;
                foreach (var experiment in list.Experiments!)
                {
                    
                    experiment.Scientists!.ToList().ForEach(scientist =>
                    {
                        scientist.Should().NotBeNull();
                    });

                    experiment.Comments!.ToList().ForEach(comment =>
                    {
                        comment.Should().NotBeNull();
                    });

                    experiment.Label!.VerifyAllPropertiesNotNull().Should().BeTrue();
                }
            }
        }

        [Fact]
        public void Should_Return_Lists_Of_A_Scientist_By_ScientisId()
        {
            var lists = _sut.GetByScientistId(1);

            Assert.NotNull(lists);

            Assert.Equal(1, lists.Count());
            foreach (var list in lists)
            {
                foreach (var experiment in list.Experiments!)
                {
                    experiment.Scientists.Should().BeNull();

                    experiment.Comments!.ToList().ForEach(comment =>
                    {
                        comment.Should().NotBeNull();
                    });

                    experiment.Label!.VerifyAllPropertiesNotNull().Should().BeTrue();
                }
            }
        }

        [Fact]
        public void Should_Return_Lists_Of_A_Scientist_By_ScientisId_Not_Existing()
        {
            var lists = _sut.GetByScientistId(0).ToList();
            Assert.Empty(lists);
        }
    }
}
