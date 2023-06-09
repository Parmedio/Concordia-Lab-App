using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;
using PersistentLayer.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayerTest
{
    public class ListReposiotoryTests
    {
        private readonly ListRepository _listRepository;
        private readonly ConcordiaDbContext _dbContext;

        public ListReposiotoryTests()
        {
            var options = new DbContextOptionsBuilder<ConcordiaDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ConcordiaDbContext(options);
            _listRepository = new ListRepository(_dbContext);
        }

        [Fact]
        public void Should_Return_All_Lists()
        {

        }

        [Fact]
        public void Shoul_Return_Lists_Of_A_Scientist_By_ScientisId()
        {
        }

        public record Scientist(int Id = default, string TrelloToken = null!, string TrelloMemberId = null!, string Name = null!)
        {
            public virtual IEnumerable<Experiment>? Experiments { get; set; }
            [NotMapped]
            public virtual IEnumerable<int>? ExperimentsIds { get; set; }
        }
      
    }
}
