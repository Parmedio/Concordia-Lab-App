using Microsoft.EntityFrameworkCore;

using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace PersistentLayer.Repositories.Concrete;

public class ExperimentRepository : IExperimentRepository
{
    private readonly ConcordiaDbContext _dbContext;

    public ExperimentRepository(ConcordiaDbContext dbContext)
        => _dbContext = dbContext;

    public IEnumerable<Experiment> Add(IEnumerable<Experiment> experiments)
    {
        throw new NotImplementedException();
    }

    public Experiment Add(Experiment experiment)
    {
        var entity = _dbContext.Experiments.Add(experiment);
        _dbContext.SaveChanges();

        if (experiment.ScientistsIds != null)
        {
            var scientists = _dbContext.Scientist.AsNoTracking().Where(s => experiment.ScientistsIds.Contains(s.Id));
            entity.Entity.Scientists = scientists.ToList();
        }

        if (experiment.CommentsIds != null)
        {
            var comments = _dbContext.Comments.AsNoTracking().Where(c => experiment.CommentsIds.Contains(c.Id));
            entity.Entity.Comments = comments.ToList();
        }

        if (experiment.LabelId != 0)
        {
            var label = _dbContext.Labels.AsNoTracking().FirstOrDefault(l => l.Id == experiment.LabelId);
            entity.Entity.Label = label;
        }

        if (experiment.ListId != 0)
        {
            var list = _dbContext.Lists.AsNoTracking().FirstOrDefault(l => l.Id == experiment.ListId);
            entity.Entity.List = list;
        }
        return entity.Entity;
    }

    public IEnumerable<Experiment> GetAll()
    {
        return _dbContext.Experiments.Include(e => e.Scientists)
                      .Include(e => e.Comments)
                      .Include(e => e.Label)
                      .Include(e => e.List);
    }

    public Experiment? GetById(int experimentId)
    {
        return _dbContext.Experiments.Include(e => e.Scientists)
                                     .Include(e => e.Comments)
                                     .Include(e => e.Label)
                                     .Include(e => e.List)
                                     .AsNoTracking()
                                     .SingleOrDefault(e => e.Id == experimentId);
    }

    public Comment GetLastCommentWithTrelloIdNull(Experiment experiment)
    {
        throw new NotImplementedException();
    }

    public int? GetLocalIdByTrelloId(string trelloId)
    {
        throw new NotImplementedException();
    }

    public Experiment? Move(int experimentId, int listId)
    {
        throw new NotImplementedException();
    }

    public Experiment? Remove(int experimentId)
    {
        throw new NotImplementedException();
    }

    public Experiment Update(Experiment experiment)
    {
        throw new NotImplementedException();
    }
}
