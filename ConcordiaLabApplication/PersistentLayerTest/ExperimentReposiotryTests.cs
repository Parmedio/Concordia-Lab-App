using PersistentLayer.Configurations;
using PersistentLayer.Models;
using PersistentLayer.Repositories.Concrete;

namespace PersistentLayerTest
{
    [Collection("DbContextCollection")]
    public class ExperimentRepositoryTests
    {
        private readonly DbContextFixture _sut;

        public ExperimentRepositoryTests(DbContextFixture dbContext) // passre reposiotry
        {
            _sut = dbContext;
        }

        [Fact]
        public void Add_Experiments_Should_Add_Experiments_To_Database()
        {
        }
        [Fact]
        public void Should_Return_All_Experiments()
        {

        }

        [Fact]
        public void Should_Return_Specific_Experiment()
        {

        }

        [Fact]
        public void Get_Should_Return_Null_With_Id_Not_Existing()
        {

        }

        [Fact]
        public void Should_Remove_Experiment_From_Database()
        {

        }

        [Fact]
        public void Remove_Should_Return_Null_With_Id_To_Remove_Not_Existing()
        {

        }

        [Fact]
        public void Should_Update_Experiment_In_Database()
        {

        }


        [Fact]
        public void Update_Should_Return_Null_With_Id_Not_Existing()
        {

        }
    }
}