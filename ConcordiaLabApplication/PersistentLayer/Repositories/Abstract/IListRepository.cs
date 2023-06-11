using PersistentLayer.Models;

namespace PersistentLayer.Repositories.Abstract;

public interface IListRepository
{

    public IEnumerable<ListEntity> GetAll();

    public IEnumerable<ListEntity> GetByScientistId(int scientistId);
}