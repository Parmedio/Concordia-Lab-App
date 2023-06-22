using FluentAssertions;

using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Concrete;

namespace ConcordiaAppTestLayer.PersistentLayerTests;

public class ExperimentRepositoryTests
{
    private readonly ExperimentRepository _sut;
    private readonly ConcordiaDbContext _dbContext;

    public ExperimentRepositoryTests()
    {
        _dbContext = new TestDatabaseFixture().CreateContext();
        _sut = new ExperimentRepository(_dbContext);
    }

    [Fact]
    public void Add_Experiment_Should_Add_Experiment_To_Database()
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        var experiment = new Experiment
        {
            TrelloId = "NewTrelloId",
            Title = "New Experiment",
            Description = "This is an experiment",
            DeadLine = DateTime.Now.AddDays(7),
            LabelId = 2,
            ColumnId = 1,
            ScientistsIds = new List<int> { 4, 5, 6 },
            Scientists = new List<Scientist>
                    {
                        new Scientist { TrelloToken = "wfrf445eef344rf", TrelloMemberId = "3434fv", Name = "gabriele" },
                        new Scientist { TrelloToken = "wedecerfedef", TrelloMemberId = "324332d", Name = "marco" },
                        new Scientist { TrelloToken = "wwdwx2rycecee23", TrelloMemberId = "dcwd2323c", Name = "alessandro" }
                    }
        };

        var addedExperiment = _sut.Add(experiment);

        Assert.Equal(experiment, addedExperiment);
        Assert.Equal(experiment.Label, addedExperiment.Label);
        Assert.Equal(experiment.Column, addedExperiment.Column);

        Assert.NotNull(addedExperiment.Scientists);
        foreach (var scientist in addedExperiment.Scientists) Assert.NotNull(scientist);

        transaction.Rollback();
    }

    [Fact]
    public void Add_Experiment_With_No_Scientist_And_DeadLine_Should_Add_Experiment_To_Database()
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        var experiment = new Experiment
        {
            TrelloId = "NewTrelloId",
            Title = "New Experiment",
            Description = "This is an experiment",
            LabelId = 2,
            ColumnId = 1
        };

        var addedExperiment = _sut.Add(experiment);

        Assert.Equal(experiment, addedExperiment);
        Assert.Equal(experiment.Label, addedExperiment.Label);
        Assert.Equal(experiment.Column, addedExperiment.Column);

        Assert.Null(addedExperiment.Scientists);

        Assert.Null(addedExperiment.DeadLine);

        transaction.Rollback();
    }

    [Fact]
    public void Add_Experiments_Should_Add_Experiments_To_Database()
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        var experiments = new List<Experiment>
        {
            new Experiment
            {
                TrelloId = "TrelloId6",
                Title = "Experiment 1",
                Description = "This is experiment 1",
                ScientistsIds = new List<int> {4, 5},
                Scientists = new List<Scientist>
                    {
                        new Scientist { TrelloToken = "wfrf445eef344rf", TrelloMemberId = "3434fv", Name = "gabriele" },
                        new Scientist { TrelloToken = "wedecerfedef", TrelloMemberId = "324332d", Name = "marco" }
                    },
                ColumnId = 1,
                LabelId = 2
            },
            new Experiment
            {
                TrelloId = "TrelloId7",
                Title = "Experiment 2",
                Description = "This is experiment 2",
                ScientistsIds = new List<int> {4, 5},
                Scientists = new List<Scientist>
                    {
                        new Scientist { TrelloToken = "wfrf445eef344rf", TrelloMemberId = "3434fv", Name = "gabriele" },
                        new Scientist { TrelloToken = "wedecerfedef", TrelloMemberId = "324332d", Name = "marco" }
                    },
                ColumnId = 2,
                LabelId = 3,
            }
        };
        var result = _sut.Add(experiments).ToList();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(experiments);

        result.ForEach(experiment =>
        {
            experiment.Scientists.Should().NotBeNull();
        });

        result.ForEach(experiment =>
        {
            experiment.Column.Should().BeEquivalentTo(experiments.First(e => e.Id == experiment.Id).Column);
        });

        result.ForEach(experiment =>
        {
            experiment.Label.Should().BeEquivalentTo(experiments.First(e => e.Id == experiment.Id).Label);
        });

        transaction.Rollback();
    }

    [Fact]
    public void Should_Return_Specific_Experiment_By_Id()
    {
        var result = _sut.GetById(1);

        Assert.NotNull(result);

        Assert.Equal(1, result.Id);
        Assert.Equal("Experiment 1", result.Title);
        Assert.Equal("This is experiment 1", result.Description);

        result.Scientists.Should().NotBeNull();
        result.Scientists!.ToList().ForEach(scientist =>
        {
            scientist.Should().NotBeNull();
        });

        result.Comments.Should().NotBeNull();
        result.Comments!.ToList().ForEach(comment =>
        {
            comment.Should().NotBeNull();
        });

        Assert.NotNull(result.Label);
        Assert.Equal(1, result.Label.Id);
        Assert.Equal("Low", result.Label.Title);
        Assert.Equal("647609751afdaf2b05536cd7", result.Label.TrelloId);

        Assert.NotNull(result.Column);
        Assert.Equal(1, result.Column.Id);
        Assert.Equal("64760975fbea80d6ef329080", result.Column.TrelloId);
        Assert.Equal("to do", result.Column.Title);
    }

    [Fact]
    public void Should_Return_LocalId_With_TrelloId()
    {
        var result = _sut.GetLocalIdByTrelloId("TrelloId1");
        Assert.Equal(result, 1);
    }

    [Fact]
    public void Get_Should_Return_Null_With_Id_Not_Existing()
    {
        var result = _sut.GetById(0);
        Assert.Null(result);
    }

    [Fact]
    public void Should_Return_All_Experiments()
    {
        var result = _sut.GetAll();
        Assert.Equal(5, result.Count());

        result.ToList().ForEach(experiment =>
        {
            experiment.Should().NotBeNull();
            experiment.Id.Should().NotBe(0);
        });
    }

    [Fact]
    public void Should_Return_Last_Local_Comment_Not_In_Trello_By_ExperimentId()
    {
        var comment = _sut.GetLastLocalCommentNotOnTrello(3);
        Assert.NotNull(comment);
        Assert.Null(comment.TrelloId);
        Assert.True(comment.Id == 5);
    }

    [Fact]
    public void Should_Return_Null_When_Last_Comment_Is_In_Trello_By_ExperimentId()
    {
        var comment = _sut.GetLastLocalCommentNotOnTrello(2);
        Assert.Null(comment);
    }

    [Fact]
    public void Should_Remove_Experiment_From_Database()
    {
        using var transaction = _dbContext.Database.BeginTransaction();

        var removedExperiment = _sut.Remove(3);
        Assert.NotNull(removedExperiment);
        var retrievedExperiment = _dbContext.Experiments.FirstOrDefault(e => e.Id == removedExperiment.Id);
        Assert.Null(retrievedExperiment);

        transaction.Rollback();
    }

    [Fact]
    public void Remove_Should_Return_Null_With_Id_Not_Existing()
    {
        var result = _sut.Remove(0);
        Assert.Null(result);
    }

    [Fact]
    public void Update_Should_Move_Experiment_In_Anoher_List()
    {
        using var transaction = _dbContext.Database.BeginTransaction();

        var updatedExperiment = _sut.Update(1, 2);

        Assert.NotNull(updatedExperiment);

        Assert.Equal(1, updatedExperiment.Id);

        Assert.Equal(2, updatedExperiment.ColumnId);

        Assert.NotNull(updatedExperiment.Scientists);
        Assert.True(updatedExperiment.Scientists.VerifyAllPropertiesNotNull());

        Assert.NotNull(updatedExperiment.Comments);
        Assert.True(updatedExperiment.Comments.VerifyAllPropertiesNotNull());

        Assert.NotNull(updatedExperiment.Label);
        Assert.True(updatedExperiment.Label.VerifyAllPropertiesNotNull());

        transaction.Rollback();
    }

    [Fact]
    public void Should_Return_LabelId_By_ExperimentTrelloId()
    {
        var result = _sut.GetLocalIdLabelByTrelloIdLabel("647609751afdaf2b05536cd9");
        Assert.Equal(2, result);
    }
}