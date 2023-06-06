using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Concrete;

namespace PersistentLayerTest
{
    [Collection("DbContextCollection")]
    public class ExperimentRepositoryTests : IClassFixture<ExperimentRepositoryFixture>
    {
        private readonly ExperimentRepositoryFixture _sut;

        public ExperimentRepositoryTests(ExperimentRepositoryFixture experimentRepository) 
        {
            _sut = experimentRepository;
        }

        [Fact]
        public void Add_Experiments_Should_Add_Experiments_To_Database()
        {
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