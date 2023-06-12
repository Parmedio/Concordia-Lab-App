using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Repositories.Concrete;

namespace PersistentLayerTest
{
    public class ScientistRepositoryTests
    {
        private readonly ScientistRepository _sut;

        public ScientistRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ConcordiaDbContext>()
                .UseSqlServer("Data Source=DESKTOP-476F63V\\SQLEXPRESS;Initial Catalog=ConcordiaLab;Integrated Security=true;TrustServerCertificate=True;")
                .Options;

            var _dbContext = new ConcordiaDbContext(dbContextOptions);
            _sut = new ScientistRepository(_dbContext);
        }

        [Fact]
        public void Should_Retrun_LocalId_By_TrelloId()
        {
            var result = _sut.GetLocalIdByTrelloId("dferfre333");
            Assert.Equal(1, result);
        }
    }
}
