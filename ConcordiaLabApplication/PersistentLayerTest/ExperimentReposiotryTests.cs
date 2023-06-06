using Microsoft.EntityFrameworkCore;
using Moq;
using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;
using PersistentLayer.Repositories.Concrete;

namespace PersistentLayerTest
{
    [Collection("DbContextCollection")]
    public class ExperimentRepositoryTests : IClassFixture<ExperimentRepositoryFixture>
    {
        private readonly ExperimentRepository _sut;

        public ExperimentRepositoryTests(ExperimentRepositoryFixture sut)
        {
            _sut = sut.ExperimentRepository;
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
                CommentsIds = new List<int> { 2 },
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
            Assert.Equal(experiment.CommentsIds, addedExperiment.CommentsIds);
            Assert.Equal(experiment.ScientistsIds, addedExperiment.ScientistsIds);
        }

        public void Should_Return_All_Experiments()
        {

        }

        public void Should_Return_Specific_Experiment()
        {

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