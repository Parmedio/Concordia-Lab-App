using FluentAssertions;
using PersistentLayer.Configurations;
using PersistentLayer.Repositories.Concrete;

namespace ConcordiaAppTestLayer.PersistentLayerTests;

public class ColumnReposiotoryTests
{
    private readonly ColumnRepository _sut;
    private readonly ConcordiaDbContext _dbContext;

    public ColumnReposiotoryTests()
    {
        _dbContext = new TestDatabaseFixture().CreateContext();
        _sut = new ColumnRepository(_dbContext);
    }

    [Fact]
    public void Should_Return_All_Columns()
    {
        var lists = _sut.GetAll();
        Assert.Equal(3, lists.Count());

        foreach (var list in lists)
        {
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
    public void Should_Return_Columns_Of_A_Scientist_By_ScientisId()
    {
        var lists = _sut.GetByScientistId(1);

        Assert.NotNull(lists);

        Assert.Equal(3, lists.Count());
        foreach (var list in lists)
        {
            foreach (var experiment in list.Experiments!)
            {
                experiment.Scientists!.Select(s => s.Id).Contains(4).Should().BeTrue();

                experiment.Comments!.ToList().ForEach(comment =>
                {
                    comment.Should().NotBeNull();
                });

                experiment.Label!.VerifyAllPropertiesNotNull().Should().BeTrue();
            }
        }
    }

    [Fact]
    public void Should_Return_Empty_List_By_ScientistId_Not_Existing()
    {
        var columns = _sut.GetByScientistId(0);
        Assert.Empty(columns);
    }

    [Fact]
    public void Should_Return_All_Columns_Simple()
    {
        var list = _sut.GetAllSimple();
        Assert.Equal(3, list.Count());
    }
}
