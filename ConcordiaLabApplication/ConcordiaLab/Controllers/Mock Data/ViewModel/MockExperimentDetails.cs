using ConcordiaLab.Controllers.Mock_Data.MockModels;

namespace ConcordiaLab.Controllers.Mock_Data.ViewModel
{
    public class MockExperimentDetails
    {
        public MockExperimentDetails(MockExperiment mockExperiments)
        {
            Experiment = mockExperiments;
        }

        public MockExperiment Experiment { get; }
    }
}
