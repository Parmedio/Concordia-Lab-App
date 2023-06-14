using PersistentLayer.Models;

namespace PersistentLayer.Repositories.Abstract;

public interface IScientistRepository
{
    public int? GetLocalIdByTrelloId(string trelloId);

    public IEnumerable<Scientist> GetAll();

    public Scientist? GetById(int id);
}
