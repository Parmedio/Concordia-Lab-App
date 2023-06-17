using PersistentLayer.Configurations;
using PersistentLayer.Models;
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
    public void Should_Return_All_Scientists()
    {
        var scientists = _sut.GetAll();
        Assert.Equal(3, scientists.Count());
    }

    [Fact]
    public void Should_Retrun_Scientist_LocalId_By_TrelloId()
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

    [Fact]
    public void Should_Return_Scientist_By_Id()
    {
        var scientist = _sut.GetById(1);
        Assert.NotNull(scientist);
        Assert.Equal(1, scientist.Id);
        Assert.Equal("ATTA5c0a0bf47c1be3f495ebb81c42316684ff55e1134be71c0eba2cbecdd0614558CDCC81F8", scientist.TrelloToken);
        Assert.Equal("Alessandro Ferluga", scientist.Name);
        Assert.Equal("5bf9f901921c336b20b29d25", scientist.TrelloMemberId);
    }

    [Fact]  
    public void Should_Return_Null_By_Id_Not_Exisiting()
    {
        var scientist = _sut.GetById(0);
        Assert.Null(scientist);
    }
}