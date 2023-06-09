using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace PersistentLayer.Repositories.Concrete
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ConcordiaDbContext _dbContext;

        public CommentRepository(ConcordiaDbContext dbContext)
            =>_dbContext = dbContext;

        public int AddComment(Comment comment)
        {
            var entity = _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
            return entity.Entity.Id;
        }

        public Comment? GetCommentByTrelloId(string trelloId)
            => _dbContext.Comments.AsNoTracking().SingleOrDefault(c => c.TrelloId == trelloId);      
    }
}
