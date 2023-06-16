using PersistentLayer.Models;

namespace PersistentLayer.Repositories.Abstract;

public interface IExperimentRepository
{
    public IEnumerable<Experiment> Add(IEnumerable<Experiment> experiments);
    public Experiment Add(Experiment experiment);
    public Experiment? Remove(int experimentId);
    public Experiment? Update(int experimentId, int ColumnIdDestination);
    public IEnumerable<Experiment> GetAll();
    public Experiment? GetById(int experimentId);
    public int? GetLocalIdByTrelloId(string trelloId);
    public Comment? GetLastLocalCommentNotOnTrello(int experimentId);
    public int? GetLocalIdLabelByTrelloIdLabel(string trelloIdLabel);
}