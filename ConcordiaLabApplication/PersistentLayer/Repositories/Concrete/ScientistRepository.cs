using Microsoft.EntityFrameworkCore;

using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace PersistentLayer.Repositories.Concrete;

public class ScientistRepository : IScientistRepository
{
    private readonly ConcordiaDbContext _dbContext;

    public ScientistRepository(ConcordiaDbContext dbContext)
        => _dbContext = dbContext;

    public IEnumerable<Scientist> GetAll()
        => _dbContext.Scientists;

    public IEnumerable<Scientist> GetAllWithExperimentsAndColumns()
        => _dbContext.Scientists
                        .Include(p => p.Experiments!)
                        .ThenInclude(l => l.Column)
                        .Where(p => p.Experiments!.Any())
                        .ToList();

    public Scientist? GetById(int id)
        => _dbContext.Scientists.
        SingleOrDefault(s => s.Id == id);

    public int? GetLocalIdByTrelloId(string trelloId)
    {
        var scientist = _dbContext.Scientists.SingleOrDefault(s => s.TrelloMemberId == trelloId);
        if (scientist != null) return scientist.Id;
        return null;
    }
}