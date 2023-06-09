using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Concrete;

namespace PersistentLayerTest
{
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
                ListId = 1,
                ScientistsIds = new List<int> { 1, 2, 3 }
            };

            var addedExperiment = _sut.Add(experiment);

            Assert.NotNull(addedExperiment);
            Assert.Equal(experiment.Id, addedExperiment.Id);
            Assert.Equal(experiment.TrelloId, addedExperiment.TrelloId);
            Assert.Equal(experiment.Title, addedExperiment.Title);
            Assert.Equal(experiment.Description, addedExperiment.Description);
            Assert.Equal(experiment.DeadLine, addedExperiment.DeadLine);
            Assert.Equal(experiment.LabelId, addedExperiment.LabelId);
            Assert.Equal(experiment.ListId, addedExperiment.ListId);
            Assert.NotNull(addedExperiment.Scientists);
            Assert.Equal(experiment.ScientistsIds, addedExperiment.Scientists.Select(s => s.Id));
            Assert.Equal(experiment.Label, addedExperiment.Label);
            Assert.Equal(experiment.LabelId, addedExperiment.LabelId);
            Assert.Equal(experiment.List, addedExperiment.List);
            Assert.Equal(experiment.ListId, addedExperiment.ListId);

            _dbContext.ChangeTracker.Clear();
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
                    ListId = 1,
                    LabelId = 2
                },
                new Experiment
                {
                    TrelloId = "TrelloId7",
                    Title = "Experiment 2",
                    Description = "This is experiment 2",
                    ListId = 2,
                    LabelId = 3,
                }
            };
            var result = _sut.Add(experiments);
            Assert.Equal(2, result.Count());
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
            Assert.NotNull(result.Scientists);
            Assert.NotNull(result.Comments);
            Assert.NotNull(result.Label);
            Assert.NotNull(result.List);
        }

        [Fact]
        public void Should_Return_LocalId_With_TrelloId()
        {
            var result = _sut.GetLocalIdByTrelloId("TrelloId1");
            Assert.Equal(result, 1);
        }

        [Fact]
        public void Should_Return_All_Experiments()
        {
            var result = _sut.GetAll();
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void Get_Should_Return_Null_With_Id_Not_Existing()
        {
            var result = _sut.GetById(0);
            Assert.Null(result);
        }

        [Fact(Skip = "method not used")]
        public void Should_Return_Last_Comment_Where_TrelloId_Is_null()
        {
            var result = _sut.GetLastCommentWhereTrelloIdIsNull(1);
            Assert.NotNull(result);
        }

        [Fact]
        public void Should_Remove_Experiment_From_Database()
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            var removedExperiment = _sut.Remove(3);
            Assert.NotNull(removedExperiment);
            var retrievedExperiment = _sut.GetById(3);
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
            Assert.Equal(updatedExperiment.Id, updatedExperiment.Id);
            Assert.Equal(2, updatedExperiment.ListId);
            Assert.NotNull(updatedExperiment.Scientists);
            Assert.NotNull(updatedExperiment.Comments);
            Assert.NotNull(updatedExperiment.Label);

            transaction.Rollback();
        }

        [Fact]
        public void Should_Return_LabelId_By_ExperimentTrelloId()
        {
            var result = _sut.GetLabelId("TrelloId3");
            Assert.Equal(result, 2);
        }
    }
}