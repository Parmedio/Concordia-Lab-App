using Microsoft.EntityFrameworkCore;

using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace PersistentLayer.Repositories.Concrete;

public class ColumnRepository : IColumnRepository
{
    private readonly ConcordiaDbContext _dbContext;

    public ColumnRepository(ConcordiaDbContext dbContext)
        => _dbContext = dbContext;

    public IEnumerable<Column> GetAll()
    {
        var allColumns = _dbContext.Columns
            .Include(l => l.Experiments!)
                .ThenInclude(e => e.Scientists!)
            .Include(l => l.Experiments!)
                .ThenInclude(l => l.Comments)
            .Include(l => l.Experiments!)
                .ThenInclude(e => e.Label).AsEnumerable();
        return allColumns;
    }

    public IEnumerable<Column> GetByScientistId(int scientistId)
    {
        return _dbContext.Columns.AsNoTracking()
            .Include(l => l.Experiments!)
                .ThenInclude(e => e.Scientists!)
            .Include(l => l.Experiments!)
                .ThenInclude(l => l.Comments)
            .Include(l => l.Experiments!)
                .ThenInclude(e => e.Label)
            .Where(l => l.Experiments!.Any(e => e.Scientists!.Select(s => s.Id).Contains(scientistId)));
    }
}