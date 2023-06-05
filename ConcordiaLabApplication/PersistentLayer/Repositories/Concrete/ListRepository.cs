using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace PersistentLayer.Repositories.Concrete;

public class ListRepository : IListRepository
{
    public IEnumerable<ListEntity> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ListEntity> GetByScientistId(int scientistId)
    {
        throw new NotImplementedException();
    }
}
