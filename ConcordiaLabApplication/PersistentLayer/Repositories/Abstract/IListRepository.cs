using PersistentLayer.Models;

namespace PersistentLayer.Repositories.Abstract;

public interface IListRepository
{
    public IEnumerable<ListEntity> GetAll();

    public ListEntity GetById(int id);

    public IEnumerable<ListEntity> GetByScientistId(int scientistId);
}