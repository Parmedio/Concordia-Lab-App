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
        _dbContext.AddRange(experiments);
        _dbContext.SaveChanges();
        return experiments;
    }

    public Experiment Add(Experiment experiment)
    {
        var entity = _dbContext.Experiments.Add(experiment);
        _dbContext.SaveChanges();

        if (experiment.ScientistsIds != null)
        {
            var scientists = _dbContext.Scientists.AsNoTracking().Where(s => experiment.ScientistsIds.Contains(s.Id));
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
            var list = _dbContext.EntityLists.AsNoTracking().FirstOrDefault(l => l.Id == experiment.ListId);
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

    public Comment? GetLastCommentWhereTrelloIdIsNull(int experimentId)
    {
        return _dbContext.Experiments.AsNoTracking()
            .Include(e => e.Comments)
            .Where(e => e.Id == experimentId && e.TrelloId == null)
            .SelectMany(e => e.Comments)
            .OrderByDescending(c => c.Date)
            .FirstOrDefault();
    }

    public int? GetLocalIdByTrelloId(string trelloId)
    {
        var experiment = _dbContext.Experiments.AsNoTracking().SingleOrDefault(e => e.TrelloId.Equals(trelloId));
        if (experiment == null) return null;
        return experiment.Id;
    }

    public int? GetLocalIdLabelByTrelloIdLabel(string trelloIdLabel)
    {
        throw new NotImplementedException();
    }

    public Experiment? Move(int experimentId, int listId)
    {
        throw new NotImplementedException();
    }

    public Experiment? Remove(int experimentId)
    {
        var experiment = _dbContext.Experiments.AsNoTracking().SingleOrDefault(e => e.Id == experimentId);
        if (experiment != null)
        {
            _dbContext.Remove(experiment);
            _dbContext.SaveChanges();
        }
        return experiment;
    }

    public Experiment? Update(int experimentId, int listIdDestination)
    {
        var current = _dbContext.Experiments.SingleOrDefault(e => e.Id == experimentId);
        if (current != null)
        {
            current.ListId = listIdDestination;
            _dbContext.SaveChanges();

            var scientists = _dbContext.Scientists.AsNoTracking().Where(s => current.ScientistsIds.Contains(s.Id));
            var comments = _dbContext.Comments.AsNoTracking().Where(c => current.CommentsIds.Contains(c.Id));
            var label = _dbContext.Labels.AsNoTracking().FirstOrDefault(l => l.Id == current.LabelId);

            current.Scientists = scientists;
            current.Comments = comments;
            current.Label = label;
        }
        return current;
    }

    public int GetLabelId (string trelloId)
    {
        var experiment = _dbContext.Experiments.AsNoTracking()
            .Include(e => e.Label)
            .SingleOrDefault(e => e.TrelloId == trelloId);
        if (experiment != null) return experiment.LabelId;
        return 0;
    }
}
