using PersistentLayer.Configurations;
using PersistentLayer.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.Repositories.Concrete
{
    public class ScientistRepository : IScientistRepository
    {
        private readonly ConcordiaDbContext _dbContext;

        public ScientistRepository(ConcordiaDbContext dbContext)
            => _dbContext = dbContext;

        public int? GetLocalIdByTrelloId(string trelloId)
        {
            return _dbContext.Scientists.SingleOrDefault( s => s.TrelloMemberId == trelloId).Id;
        }
    }
}
