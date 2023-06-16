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
        var result = new List<Experiment>();
        foreach (var experiment in experiments) result.Add(Add(experiment));
        return result;
    }

    public Experiment Add(Experiment experiment)
    {
        var entity = _dbContext.Experiments.Add(experiment);
        _dbContext.SaveChanges();
        return entity.Entity;
    }

    public IEnumerable<Experiment> GetAll()
    {
        return _dbContext.Experiments
                      .Include(e => e.Scientists)
                      .Include(e => e.Comments)
                      .Include(e => e.Label)
                      .Include(e => e.Column)
                      .AsNoTracking();
    }

    public Experiment? GetById(int experimentId)
    {
        return _dbContext.Experiments.Include(e => e.Scientists)
                                     .Include(e => e.Comments)
                                     .Include(e => e.Label)
                                     .Include(e => e.Column)
                                     .AsNoTracking()
                                     .SingleOrDefault(e => e.Id == experimentId);
    }

    public Comment? GetLastLocalCommentNotOnTrello(int experimentId)
    {
        var lastComment = _dbContext.Experiments
            .Include(c => c.Comments!)
            .ThenInclude(p => p.Scientist)
            .Where(p => p.Comments!.Any())?
            .SingleOrDefault(p => p.Id == experimentId)?
            .Comments!
            .OrderByDescending(p => p.Date)
            .FirstOrDefault();

        return lastComment?.TrelloId is null ? lastComment : null;
    }

    public int? GetLocalIdByTrelloId(string trelloId)
    {
        var experiment = _dbContext.Experiments.AsNoTracking().SingleOrDefault(e => e.TrelloId == trelloId);
        if (experiment == null) return null;
        return experiment.Id;
    }

    public int? GetLocalIdLabelByTrelloIdLabel(string trelloIdLabel)
    {
        var label = _dbContext.Labels.AsNoTracking()
            .SingleOrDefault(l => l!.TrelloId == trelloIdLabel);
        if (label != null) return label.Id;
        return null;
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

    public Experiment? Update(int experimentId, int ColumnIdDestination)
    {
        var current = _dbContext.Experiments.FirstOrDefault(x => x.Id == experimentId);
        if (current != null)
        {
            current.ColumnId = ColumnIdDestination;
            _dbContext.Update(current);
            _dbContext.SaveChanges();
            current.Column = _dbContext.Columns.SingleOrDefault(p => p.Id == current.ColumnId)!;
        }
        return current;
    }
}
