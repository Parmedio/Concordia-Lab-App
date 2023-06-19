using Microsoft.EntityFrameworkCore;

using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace PersistentLayer.Repositories.Concrete;

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

    public Comment? UpdateAComment(int id, string trelloId)
    {
        var entity = _dbContext.Comments.SingleOrDefault(p => p.Id == id) ?? null;
        if (entity is not null)
        {
            _dbContext.Update(entity);
            entity.TrelloId = trelloId;
            _dbContext.SaveChanges();
        }
        return entity;
    }
}
