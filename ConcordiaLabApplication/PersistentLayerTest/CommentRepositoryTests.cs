using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Concrete;
using Xunit;

namespace PersistentLayerTest
{
    public class CommentRepositoryTests
    {
        private readonly CommentRepository _sut;
        private readonly ConcordiaDbContext _dbContext;

        public CommentRepositoryTests()
        {
            _dbContext = new TestDatabaseFixture().CreateContext();
            _sut = new CommentRepository (_dbContext);
        }

        [Fact]
        public void Add_Comment_Should_Return_CommentId_()
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            var comment = new Comment { TrelloId = "rfgerre444f", Body = "Test Comment", ExperimentId = 1, ScientistId = 2 };
            var commentId = _sut.AddComment(comment);
            Assert.NotEqual(0, commentId);

            var commentAdded = _sut.GetCommentByTrelloId("rfgerre444f");
            Assert.NotNull(commentAdded);
            Assert.Equal (2, commentAdded.ScientistId);
            Assert.Equal (1, commentAdded.ExperimentId);
            transaction.Rollback();
        }

        [Fact]
        public void GetCommentByTrelloId_ExistingComment_Should_Return_Comment()
        {
            var comment = _sut.GetCommentByTrelloId("TrelloIdComment1");
            Assert.NotNull(comment);
            Assert.Equal("This is the first comment.", comment.Body);
            Assert.Equal("Gabriele", comment.CreatorName);
            Assert.Equal(1, comment.ExperimentId);
            Assert.Equal(1, comment.ScientistId);
            Assert.NotNull(comment.Scientist);
        }

        [Fact]
        public void GetCommentByTrelloId_Not_ExistingComment_Should_Return_Null()
        {
            var comment = _sut.GetCommentByTrelloId("NonExistingTrelloId");
            Assert.Null(comment);
        }
    }
}
