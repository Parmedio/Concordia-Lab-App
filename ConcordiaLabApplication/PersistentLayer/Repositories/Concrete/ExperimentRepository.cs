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
        throw new NotImplementedException();
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
