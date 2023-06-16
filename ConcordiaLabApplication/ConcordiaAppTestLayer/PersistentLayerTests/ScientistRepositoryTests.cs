using PersistentLayer.Configurations;
using PersistentLayer.Repositories.Concrete;

namespace ConcordiaAppTestLayer.PersistentLayerTests;

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
        var scientistId = _sut.GetLocalIdByTrelloId("639c692ed850f6055714fd55");
        Assert.Equal(2, scientistId);
    }

    [Fact]
    public void Should_Retrun_Null_By_TrelloId_Not_Existing()
    {
        var scientistId = _sut.GetLocalIdByTrelloId("trelloIdNotExisting");
        Assert.Null(scientistId);
    }
}
