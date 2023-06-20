using PersistentLayer.Models;

namespace PersistentLayer.Repositories.Abstract;

public interface IScientistRepository
{
    public int? GetLocalIdByTrelloId(string trelloId);
    public IEnumerable<Scientist> GetAll();
    public IEnumerable<Scientist> GetAllWithExperimentsAndColumns();
    public Scientist? GetById(int id);
}
