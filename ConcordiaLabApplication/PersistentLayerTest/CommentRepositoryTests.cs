using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Concrete;

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

            var comment = new Comment { TrelloId = "rfgerre444f", Body = "Test Comment", ExperimentId = 1 };
            var commentId = _sut.AddComment(comment);
            Assert.NotEqual(0, commentId);

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
        }

        [Fact]
        public void GetCommentByTrelloId_Not_ExistingComment_Should_Return_Null()
        {
            var comment = _sut.GetCommentByTrelloId("NonExistingTrelloId");
            Assert.Null(comment);
        }
    }
}
