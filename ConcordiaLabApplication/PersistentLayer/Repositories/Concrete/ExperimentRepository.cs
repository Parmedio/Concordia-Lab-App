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
        if (experiment.ScientistsIds != null)
        {
            var scientists = _dbContext.Scientist.Where(s => experiment.ScientistsIds.Contains(s.Id));
            experiment.Scientists = scientists.ToList();
        }

        if (experiment.CommentsIds != null)
        {
            var comments = _dbContext.Comments.Where(c => experiment.CommentsIds.Contains(c.Id));
            experiment.Comments = comments.ToList();
        }

        if (experiment.LabelId != 0)
        {
            var label = _dbContext.Labels.FirstOrDefault(l => l.Id == experiment.LabelId);
            experiment.Label = label;
        }

        if (experiment.ListId != 0)
        {
            var list = _dbContext.Lists.FirstOrDefault(l => l.Id == experiment.ListId);
            experiment.List = list;
        }

        _dbContext.SaveChanges();
        return entity.Entity;
    }

    public IEnumerable<Experiment> GetAll()
    {
        throw new NotImplementedException();
    }

    public Experiment? GetById(int experimentId)
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
