using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.Repositories.Concrete
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ConcordiaDbContext _dbContext;

        public CommentRepository(ConcordiaDbContext dbContext)
            =>_dbContext = dbContext;

        public int AddComment(Comment comment)
        {
            var e = _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
            return e.Entity.Id;
        }

        public Comment? GetCommentByTrelloId(string trelloId)
            => _dbContext.Comments.AsNoTracking().SingleOrDefault(c => c.TrelloId.Equals(trelloId));
    }
}
