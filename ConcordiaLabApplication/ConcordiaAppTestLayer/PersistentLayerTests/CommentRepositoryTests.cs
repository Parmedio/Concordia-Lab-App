using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Concrete;

namespace ConcordiaAppTestLayer.PersistentLayerTests;

public class CommentRepositoryTests
{
    private readonly CommentRepository _sut;
    private readonly ConcordiaDbContext _dbContext;

    public CommentRepositoryTests()
    {
        _dbContext = new TestDatabaseFixture().CreateContext();
        _sut = new CommentRepository(_dbContext);
    }

    [Fact]
    public void Add_Comment_Should_Return_Comment()
    {
        using var transaction = _dbContext.Database.BeginTransaction();

        var comment = new Comment { TrelloId = "rfgerre444f", Body = "Test Comment", ExperimentId = 1, ScientistId = 2 };
        var Returnedcomment = _sut.AddComment(comment);
        Assert.Equal(Returnedcomment, comment);

        var commentAdded = _sut.GetCommentByTrelloId("rfgerre444f");
        Assert.NotNull(commentAdded);
        Assert.Equal(2, commentAdded.ScientistId);
        Assert.Equal(1, commentAdded.ExperimentId);

        transaction.Rollback();
    }

    [Fact]
    public void GetCommentByTrelloId_Of_Scientist_Should_Return_Comment()
    {
        var comment = _sut.GetCommentByTrelloId("TrelloIdComment1");
        Assert.NotNull(comment);
        Assert.Equal("This is the first comment.", comment.Body);
        Assert.Equal("Alessandro", comment.CreatorName);
        Assert.Equal(1, comment.ExperimentId);
        Assert.Equal(1, comment.ScientistId);
        Assert.NotNull(comment.Scientist);
        Assert.Equal("Alessandro Ferluga", comment.Scientist.Name);
        Assert.Equal("5bf9f901921c336b20b29d25", comment.Scientist.TrelloMemberId);
        Assert.Equal("ATTA5c0a0bf47c1be3f495ebb81c42316684ff55e1134be71c0eba2cbecdd0614558CDCC81F8", comment.Scientist.TrelloToken);
    }

    [Fact]
    public void GetCommentByTrelloId_Of_Researcher_Should_Return_Comment()
    {
        var comment = _sut.GetCommentByTrelloId("TrelloIdComment3");
        Assert.NotNull(comment);
        Assert.Equal("This is the third comment.", comment.Body);
        Assert.Equal("Mike", comment.CreatorName);
        Assert.Equal(3, comment.ExperimentId);
        Assert.Null(comment.Scientist);
    }

    [Fact]
    public void GetCommentByTrelloId_Not_Existing_Should_Return_Null()
    {
        var comment = _sut.GetCommentByTrelloId("NonExistingTrelloId");
        Assert.Null(comment);
    }
}
