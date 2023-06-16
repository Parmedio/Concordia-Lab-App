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
        if (experiment.ScientistsIds != null)
        {
            var scientists = _dbContext.Scientists.Where(s => experiment.ScientistsIds.Contains(s.Id));          
            experiment.Scientists = scientists.ToList();
        }

        if (experiment.LabelId != 0)
        {
            var label = _dbContext.Labels.SingleOrDefault(l => l.Id == experiment.LabelId);
           experiment.Label = label;
        }

        if (experiment.ListId != 0)
        {
            var list = _dbContext.EntityLists.SingleOrDefault(l => l.Id == experiment.ListId);
            experiment.List = list;
        }

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
                      .Include(e => e.List)
                      .AsNoTracking();
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

    public Comment? GetLastCommentWithTrelloIdNull(int experimentId)
    {
        var experiment = _dbContext.Experiments
            .Include(e => e.Comments)
            .FirstOrDefault(e => e.Id == experimentId);

        if (experiment != null)
        {
            return experiment.Comments!
                .FirstOrDefault(c => c.TrelloId == null);
        }

        return null;
    }

    public int? GetLocalIdByTrelloId(string trelloId)
    {
        var experiment = _dbContext.Experiments.AsNoTracking().SingleOrDefault(e => e.TrelloId == trelloId);
        if (experiment == null) return null;
        return experiment.Id;
    }

    public int? GetLocalIdLabelByTrelloIdLabel(string trelloIdLabel)
    {
        var label = _dbContext.Experiments.AsNoTracking()
            .Include(e => e.Label)
            .Select(e => e.Label)
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

    public Experiment? Update(int experimentId, int listIdDestination)
    {
        var current = GetById(experimentId);
        if (current != null)
        {
            current.ListId = listIdDestination;
            _dbContext.SaveChanges();
        }        
        return current;
    }
}
