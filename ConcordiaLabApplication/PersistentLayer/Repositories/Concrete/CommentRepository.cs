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

        public Comment? AddComment(Comment comment)
        {
            var entity = _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
            entity.Entity.Experiment = _dbContext.Experiments.SingleOrDefault(p => p.Id == comment.ExperimentId)!;
            entity.Entity.Scientist = _dbContext.Scientists.SingleOrDefault(p => p.Id == comment.ScientistId)!;
            return entity.Entity;
        }

        public Comment? GetCommentByTrelloId(string trelloId)
            => _dbContext.Comments.AsNoTracking()
            .Include(c => c.Scientist)
            .SingleOrDefault(c => c.TrelloId == trelloId);

        public Comment UpdateAComment(Comment comment)
        {
            var entity = _dbContext.Update(comment);
            _dbContext.SaveChanges();
            return entity.Entity;
        }
    }
}
