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
            => _dbContext = dbContext;

        public int? AddComment(Comment comment)
        {
            try
            {
                var entity = _dbContext.Comments.Add(comment);
                _dbContext.SaveChanges();
                return entity.Entity.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Comment? GetCommentByTrelloId(string trelloId)
            => _dbContext.Comments.AsNoTracking()
            .Include (c => c.Scientist)
            .SingleOrDefault(c => c.TrelloId == trelloId);      
    }
}
