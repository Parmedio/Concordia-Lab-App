using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Models;

namespace PersistentLayerTest
{
    public class TestDatabaseFixture
    {
        private const string ConnectionString = @"Data Source=DESKTOP-476F63V\SQLEXPRESS;Initial Catalog=ConcordiaLab;Integrated Security=true;TrustServerCertificate=True;";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public TestDatabaseFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        var scientists = new List<Scientist>
                        {
                            new Scientist { TrelloToken = "wfrf445eef344rf", TrelloMemberId = "3434fv", Name = "gabriele" },
                            new Scientist { TrelloToken = "wedecerfedef", TrelloMemberId = "324332d", Name = "marco" },
                            new Scientist { TrelloToken = "wwdwx2rycecee23", TrelloMemberId = "dcwd2323c", Name = "alessandro" }
                        };

                        var lists = new List<ListEntity>
                        {
                            new ListEntity { TrelloId = "ce34442cw", Title = "to do" },
                            new ListEntity { TrelloId = "efcrvrt23", Title = "in progress" },
                            new ListEntity { TrelloId = "wede224ev", Title = "done" }
                        };

                        var experiments = new List<Experiment>
                        {
                            new Experiment { TrelloId = "TrelloId1", Title = "Experiment 1", Description = "This is experiment 1", ListId = 1, LabelId = 3, Scientists = scientists },
                            new Experiment { TrelloId = "TrelloId2", Title = "Experiment 2", Description = "This is experiment 2", ListId = 2, LabelId = 1, Scientists = scientists },
                            new Experiment { TrelloId = "TrelloId3", Title = "Experiment 3", Description = "This is experiment 3", ListId = 3, LabelId = 2, Scientists = scientists},
                            new Experiment { TrelloId = "TrelloId4", Title = "Experiment 4", Description = "This is experiment 4", ListId = 2, LabelId = 3, Scientists = scientists }
                        };

                        var comments = new List<Comment>
                        {
                            new Comment { TrelloId = "TrelloIdComment1", Body = "This is the first comment.", Date = DateTime.Now.AddDays(-2), CreatorName = "Gabriele", ExperimentId = 1, ScientistId = 1},
                            new Comment { TrelloId = "TrelloIdComment2", Body = "This is the second comment.", Date = DateTime.Now.AddDays(-1), CreatorName = "Jane",ExperimentId = 2},
                            new Comment { TrelloId = "TrelloIdComment3", Body = "This is the third comment.", Date = DateTime.Now, CreatorName = "Mike",ExperimentId = 3 },
                            new Comment { TrelloId = "TrelloIdComment4", Body = "This is the fourth comment.", Date = DateTime.Now, CreatorName = "Fin",ExperimentId = 3 }
                        };

                        var labels = new List<Label>
                        {
                            new Label { TrelloId = "TrelloLabelId1", Title = "low priority" },
                            new Label { TrelloId = "TrelloLabelId2", Title = "medium priority" },
                            new Label { TrelloId = "TrelloLabelId3", Title = "high priority" }
                        };
                        context.Scientists.AddRange(scientists);
                        context.SaveChanges();
                        context.Labels.AddRange(labels);
                        context.SaveChanges();
                        context.Experiments.AddRange(experiments);
                        context.SaveChanges();
                        context.Comments.AddRange(comments);
                        context.SaveChanges();
                    }
                    _databaseInitialized = true;
                }
            }
        }

        public ConcordiaDbContext CreateContext()
            => new(
                new DbContextOptionsBuilder<ConcordiaDbContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);

    }
}
