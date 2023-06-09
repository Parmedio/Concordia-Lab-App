using ConcordiaLab.Controllers.Mock_Data.MockModels;

namespace ConcordiaLab.Controllers.Mock_Data.ViewModel
{
    public class ExperimentDetails
    {
        public ExperimentDetails(MockExperiment mockExperiments)
        {
            Experiment = mockExperiments;
        }

        public MockExperiment Experiment { get; }
    }
}
