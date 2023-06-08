using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Concrete;

namespace PersistentLayerTest
{
    public class CommentRepositoryTests : IDisposable
    {
        private readonly CommentRepository _commentRepository;
        private readonly ConcordiaDbContext _dbContext;

        public CommentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ConcordiaDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ConcordiaDbContext(options);
            _commentRepository = new CommentRepository(_dbContext);
        }

        [Fact]
        public void Add_Comment_Should_Return_CommentId_()
        {
            var comment = new Comment { TrelloId = "T1", Body = "Test Comment" };
            var commentId = _commentRepository.AddComment(comment);
            Assert.NotEqual(0, commentId);
            _dbContext.Dispose();
        }

        [Fact]
        public void GetCommentByTrelloId_ExistingComment_Should_Return_Comment()
        {
            var existingComment = new Comment { TrelloId = "T2", Body = "Existing Comment" };
            _dbContext.Comments.Add(existingComment);
            _dbContext.SaveChanges();

            var comment = _commentRepository.GetCommentByTrelloId("T2");

            Assert.NotNull(comment);
            Assert.Equal(existingComment.TrelloId, comment.TrelloId);
            Assert.Equal(existingComment.Body, comment.Body);
        }

        [Fact]
        public void GetCommentByTrelloId_Not_ExistingComment_Should_Return_Null()
        {
            var existingComment = new Comment { TrelloId = "T1", Body = "Existing Comment" };
            _dbContext.Comments.Add(existingComment);
            _dbContext.SaveChanges();

            var comment = _commentRepository.GetCommentByTrelloId("NonExistingTrelloId");
            Assert.Null(comment);
        }

        public void Dispose()
            => _dbContext.Dispose();
    }
}
