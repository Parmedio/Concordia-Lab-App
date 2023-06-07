using PersistentLayer.Models;

namespace PersistentLayer.Repositories.Abstract;

public interface IExperimentRepository
{
    public IEnumerable<Experiment> Add(IEnumerable<Experiment> experiments);
    public Experiment Add(Experiment experiment);
    public Experiment? Remove(int experimentId);
    //public Experiment Update (int experimentId, int listIdDestination);
    //public Experiment Update(int experimentId, string comment);
    public Experiment? Update(Experiment experiment);
    public Experiment? Move(int experimentId, int listId);
    public IEnumerable<Experiment> GetAll();
    public Experiment? GetById(int experimentId);
    public int? GetLocalIdByTrelloId(string trelloId);
    public Comment GetLastCommentWithTrelloIdNull(Experiment experiment);
}