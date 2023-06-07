using PersistentLayer.Configurations;
using PersistentLayer.Repositories.Abstract;

namespace PersistentLayer.Repositories.Concrete
{
    public class ScientistRepository : IScientistRepository
    {
        private readonly ConcordiaDbContext _dbContext;

        public ScientistRepository(ConcordiaDbContext dbContext)
            => _dbContext = dbContext;

        public int? GetLocalIdByTrelloId(string trelloId)
        {
            var scientist = _dbContext.Scientists.SingleOrDefault(s => s.TrelloMemberId == trelloId);
            if (scientist != null) return scientist.Id;
            return null;
        }
    }
}
