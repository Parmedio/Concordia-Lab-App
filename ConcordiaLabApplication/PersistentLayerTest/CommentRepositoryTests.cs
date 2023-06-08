using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayerTest
{
    public class CommentRepositoryTests
    {
        private readonly CommentRepository _sut;

        public CommentRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ConcordiaDbContext>()
                .UseSqlServer("Data Source=DESKTOP-476F63V\\SQLEXPRESS;Initial Catalog=ConcordiaLab;Integrated Security=true;TrustServerCertificate=True;")
                .Options;

            var _dbContext = new ConcordiaDbContext(dbContextOptions);
            _sut = new CommentRepository(_dbContext);
        }

        [Fact]
        public void Should_Add_Comment_In_Db()
        {
            var comment = new Comment
            {
                Body = "comment body",
                Date = DateTime.Now,
                ExperimentId = 1,
                TrelloId = "refeferv",
                CreatorName = "gabriele",
                ScientistId = 1
            };
            var result = _sut.AddComment(comment);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void Should_Return_Comment_By_TrelloId()
        {
            var result = _sut.GetCommentByTrelloId("refeferv");
            Assert.NotNull(result);
        }
    }
}
