using PersistentLayer.Models;

namespace PersistentLayer.Repositories.Abstract;

public interface IListRepository
{
    public IEnumerable<ListEntity> GetAll(); // per il login

    public ListEntity GetById(int id); // ??
    public IEnumerable<ListEntity> GetByScientistId(int scientistId);
}