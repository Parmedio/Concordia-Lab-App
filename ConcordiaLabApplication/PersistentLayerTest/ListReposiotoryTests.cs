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

            Seed();
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
        private void Seed()
        {

            new Scientist
            {
                TrelloToken = "wfrf445eef344rf",
                TrelloMemberId = "3434fv",
                Name = "gabriele"
            };

            new Scientist
            {
                TrelloToken = "wedecerfedef",
                TrelloMemberId = "324332d",
                Name = "marco"
            };

            new Scientist
            {
                TrelloToken = "wwdwx2rycecee23",
                TrelloMemberId = "dcwd2323c",
                Name = "alessandro"
            };
            var lists = new List<ListEntity>
            {
                new ListEntity
                {
                    TrelloId = "ce34442cw",
                    Title = "to do",
                },
                new ListEntity
                {
                    TrelloId = "efcrvrt23",
                    Title = "in progress",
                },

                 new ListEntity
                {
                    TrelloId = "wede224ev",
                    Title = "done"
                }
             };

            var experiments = new List<Experiment>
            {
                new Experiment
                {
                    TrelloId = "TrelloId1",
                    Title = "Experiment 1",
                    Description = "This is experiment 1",
                    ListId = 1,
                    LabelId = 3
                },
                new Experiment
                {
                    TrelloId = "TrelloId2",
                    Title = "Experiment 2",
                    Description = "This is experiment 2",
                    ListId = 2,
                    LabelId = 1,
                },
                new Experiment
                {
                    TrelloId = "TrelloId3",
                    Title = "Experiment 3",
                    Description = "This is experiment 3",
                    ListId = 3,
                    LabelId = 2,
                },
                new Experiment
                {
                    TrelloId = "TrelloId4",
                    Title = "Experiment 4",
                    Description = "This is experiment 4",
                    ListId = 2,
                    LabelId = 3,
                }
            };

            var comments = new List<Comment>
            {
                new Comment
                {
                    TrelloId = "TrelloId1",
                    Body = "This is the first comment.",
                    Date = DateTime.Now.AddDays(-2),
                    CreatorName = "John"
                },
                new Comment
                {
                    TrelloId = "TrelloId2",
                    Body = "This is the second comment.",
                    Date = DateTime.Now.AddDays(-1),
                    CreatorName = "Jane"
                },
                new Comment
                {
                    TrelloId = "TrelloId3",
                    Body = "This is the third comment.",
                    Date = DateTime.Now,
                    CreatorName = "Mike"
                }
        };
        }

    }
}
