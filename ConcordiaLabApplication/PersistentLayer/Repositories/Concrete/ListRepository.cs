using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace PersistentLayer.Repositories.Concrete;

public class ListRepository : IListRepository
{
    private readonly ConcordiaDbContext _dbContext;

    public ListRepository(ConcordiaDbContext dbContext)
        => _dbContext = dbContext;

    public IEnumerable<ListEntity> GetAll()
    {
        return _dbContext.EntityLists
            .AsNoTracking()
            .Include(l => l.Experiments)
            .ThenInclude(e => e.Scientists);
    }

    public ListEntity? GetById(int id)
    {
        return _dbContext.EntityLists.AsNoTracking()
            .Include(l => l.Experiments)
            .SingleOrDefault(l => l.Id == id);
    }

    public IEnumerable<ListEntity> GetByScientistId(int scientistId)
    {
        return _dbContext.EntityLists.AsNoTracking()
            .Include(l => l.Experiments)
            .ThenInclude(e => e.Scientists)
            .Where(l => l.Experiments.Any(e => e.Scientists.Select(s => s.Id).Contains(scientistId)));
    }
}
