using PersistentLayer.Models;

namespace PersistentLayer.Repositories.Abstract;

public interface IColumnRepository
{

    public IEnumerable<Column> GetAll();

    public IEnumerable<Column> GetByScientistId(int scientistId);
}