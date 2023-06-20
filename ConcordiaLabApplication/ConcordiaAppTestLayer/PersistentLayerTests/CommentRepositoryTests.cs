using Microsoft.EntityFrameworkCore;

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
    public void Should_Return_Comment_By_Comment_TrelloId()
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
    public void Should_Return_Comment_Of_Researcher_By_Comment_TrelloId()
    {
        var comment = _sut.GetCommentByTrelloId("TrelloIdComment2");
        Assert.NotNull(comment);
        Assert.Equal("This is the second comment.", comment.Body);
        Assert.Equal("Jane", comment.CreatorName);
        Assert.Equal(2, comment.ExperimentId);
        Assert.Null(comment.Scientist);
    }

    [Fact]
    public void Should_Return_Null_By_Comment_TrelloId_Not_Existing()
    {
        var comment = _sut.GetCommentByTrelloId("NonExistingTrelloId");
        Assert.Null(comment);
    }

    [Fact]
    public void Should_Update_Comment()
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        var oldComment = _dbContext.Comments.AsNoTracking().FirstOrDefault(c => c.Id == 1);

        var commentUpdated = oldComment! with { Body = "test", CreatorName = "Gabriele", ExperimentId = 2, ScientistId = 3, Date = DateTime.Now };

        var commentResult = _sut.UpdateAComment(commentUpdated.Id, oldComment.TrelloId!); ;

        Assert.NotNull(commentResult);
        Assert.Equal(oldComment.Id, commentResult.Id);
        Assert.Equal(oldComment.TrelloId, commentResult.TrelloId);
        Assert.Equal("This is the first comment.", commentResult.Body);
        Assert.Equal("Alessandro", commentResult.CreatorName);
        Assert.Equal(1, commentResult.ExperimentId);

        transaction.Rollback();
    }
}
