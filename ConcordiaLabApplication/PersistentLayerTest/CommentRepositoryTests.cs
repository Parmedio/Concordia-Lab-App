using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Concrete;
using PersistentLayerTest;

namespace PersistentLayer.Tests
{
    public class CommentRepositoryTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly CommentRepository _sut;
        private readonly ConcordiaDbContext _dbContext;

        public CommentRepositoryTests(TestDatabaseFixture database)
        {
            _dbContext = database.CreateContext();
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
        public void GetCommentByTrelloId_Of_Scientist_Should_Return_Comment()
        {
            var comment = _sut.GetCommentByTrelloId("TrelloIdComment1");
            Assert.NotNull(comment);
            Assert.Equal("This is the first comment.", comment.Body);
            Assert.Equal("Gabriele", comment.CreatorName);
            Assert.Equal(1, comment.ExperimentId);
            Assert.Equal(1, comment.ScientistId);
            Assert.NotNull(comment.Scientist);
            Assert.Equal(1, comment.Scientist.Id);
            Assert.Equal("gabriele", comment.Scientist.Name);
            Assert.Equal("3434fv", comment.Scientist.TrelloMemberId); 
            Assert.Equal("wfrf445eef344rf", comment.Scientist.TrelloToken);
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
}
