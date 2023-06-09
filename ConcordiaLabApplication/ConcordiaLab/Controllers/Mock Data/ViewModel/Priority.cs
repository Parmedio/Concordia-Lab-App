using ConcordiaLab.Controllers.Mock_Data.MockModels;

namespace ConcordiaLab.Controllers.Mock_Data.ViewModel
{
    public class Priority
    {
        public Priority(IEnumerable<MockExperiment> mockExperiments)
        {
            Experiments = mockExperiments;
        }

        public Priority() : this(new List<MockExperiment>())
        {
        }

        public IEnumerable<MockExperiment> Experiments { get; }
    }
}
