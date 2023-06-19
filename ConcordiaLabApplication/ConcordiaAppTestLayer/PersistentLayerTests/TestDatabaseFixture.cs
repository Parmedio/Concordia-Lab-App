using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Models;
using Microsoft.Extensions.Configuration;

namespace ConcordiaAppTestLayer.PersistentLayerTests;

public class TestDatabaseFixture
{
    private readonly string _connectionString;

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestDatabaseFixture()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        _connectionString = builder.Build().GetConnectionString("TestConnection")!;
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    var experiments = new List<Experiment>
                    {
                        new Experiment { TrelloId = "TrelloId1", Title = "Experiment 1", Description = "This is experiment 1", ColumnId = 1, LabelId = 1, ScientistsIds = new List<int> {1} },
                        new Experiment { TrelloId = "TrelloId2", Title = "Experiment 2", Description = "This is experiment 2", ColumnId = 2, LabelId = 2, ScientistsIds = new List<int> {1,2 } },
                        new Experiment { TrelloId = "TrelloId3", Title = "Experiment 3", Description = "This is experiment 3", ColumnId = 3, LabelId = 4, ScientistsIds = new List<int> {1,2,3 }},
                        new Experiment { TrelloId = "TrelloId4", Title = "Experiment 4", Description = "This is experiment 4", ColumnId = 2, LabelId = 6, ScientistsIds = new List<int> {1,3 } },
                        new Experiment { TrelloId = "TrelloId5", Title = "Experiment 5", Description = "This is experiment 5", ColumnId = 1, LabelId = 3, ScientistsIds = new List<int> {1,2,3 } }
                    };
                    context.Experiments.AddRange(experiments);
                    context.SaveChanges();

                    var comments = new List<Comment>
                        {
                            new Comment { TrelloId = "TrelloIdComment1", Body = "This is the first comment.", Date = DateTime.Now.AddDays(-3), CreatorName = "Alessandro", ExperimentId = 1, ScientistId = 1},
                            new Comment { TrelloId = "TrelloIdComment2", Body = "This is the second comment.", Date = DateTime.Now.AddDays(-3), CreatorName = "Jane", ExperimentId = 2},
                            new Comment { TrelloId = "TrelloIdComment3", Body = "This is the third comment.", Date = DateTime.Now.AddDays(-2), CreatorName = "Marco", ExperimentId = 3, ScientistId = 2},
                            new Comment { TrelloId = "TrelloIdComment4", Body = "This is the fourth comment.", Date = DateTime.Now.AddDays(-1), CreatorName = "Fin", ExperimentId = 3 },
                            new Comment { TrelloId = null, Body = "This is the fifht comment.", Date = DateTime.Now, CreatorName = "Fin",ExperimentId = 3 }
                        };

                    context.Comments.AddRange(comments);
                    context.SaveChanges();
                }
                _databaseInitialized = true;
            }
        }
    }

    internal ConcordiaDbContext CreateContext()
        => new(
            new DbContextOptionsBuilder<ConcordiaDbContext>()
                .UseSqlServer(_connectionString)
                .Options);

}