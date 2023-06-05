using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace PersistentLayer.Repositories.Concrete;

public class ExperimentRepository : IExperimentRepository
{
    public Experiment Add(List<Experiment> experiments)
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

    public IEnumerable<Experiment> GetById(int experimentId)
    {
        throw new NotImplementedException();
    }

    public Experiment Remove(int experimentId)
    {
        throw new NotImplementedException();
    }

    public Experiment Update(int experimentId, int listIdDestination)
    {
        throw new NotImplementedException();
    }

    public Experiment Update(Experiment experiment)
    {
        throw new NotImplementedException();
    }

    public Experiment Update(int experimentId, string comment)
    {
        throw new NotImplementedException();
    }
}
