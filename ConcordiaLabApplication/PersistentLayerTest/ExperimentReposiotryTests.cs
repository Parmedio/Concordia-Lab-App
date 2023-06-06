using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;
using PersistentLayer.Repositories.Concrete;

namespace PersistentLayerTest
{
    [Collection("DbContextCollection")]
    public class ExperimentRepositoryTests
    {
        private readonly ExperimentRepository _sut;

        public ExperimentRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ConcordiaDbContext>()
                .UseSqlServer("Data Source=DESKTOP-476F63V\\SQLEXPRESS;Initial Catalog=ConcordiaLab;Integrated Security=true;TrustServerCertificate=True;")
                .Options;

            var _dbContext = new ConcordiaDbContext(dbContextOptions);
            _sut = new ExperimentRepository(_dbContext);
        }

        [Fact]
        public void Add_Experiment_Should_Add_Experiment_To_Database()
        {
            var experiment = new Experiment
            {
                TrelloId = "TrelloId",
                Title = "Experiment 1",
                Description = "This is an experiment",
                DeadLine = DateTime.Now.AddDays(7),
                LabelId = 4,
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
            Assert.Equal(experiment.ScientistsIds, addedExperiment.Scientists.Select (s => s.Id));
            Assert.Equal(experiment.Label, addedExperiment.Label);
            Assert.Equal(experiment.List, addedExperiment.List);
        }
        [Fact]
        public void Should_Return_Specific_Experiment_By_Id()
        {
            var result = _sut.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("test calotte", result.Title);
            Assert.Equal("description", result.Description);
            Assert.NotNull(result.Scientists);
            Assert.NotNull(result.Comments);
            Assert.NotNull(result.Label);
            Assert.NotNull(result.List);
        }

        [Fact]
        public void Should_Return_LocalId_With_TrelloId()
        {
            var result = _sut.GetLocalIdByTrelloId("vrvrgdwrr43");
            Assert.Equal(result, 1);
        }

        [Fact]  
        public void Should_Return_All_Experiments()
        {
            var result = _sut.GetAll();
            Assert.NotNull(result);
            //Assert.Equal(4, result.Count());
        }

        public void Get_Should_Return_Null_With_Id_Not_Existing()
        {

        }

        public void Should_Remove_Experiment_From_Database()
        {

        }

        public void Remove_Should_Return_Null_With_Id_To_Remove_Not_Existing()
        {

        }

        public void Should_Update_Experiment_In_Database()
        {

        }

        public void Update_Should_Return_Null_With_Id_Not_Existing()
        {

        }
    }
}