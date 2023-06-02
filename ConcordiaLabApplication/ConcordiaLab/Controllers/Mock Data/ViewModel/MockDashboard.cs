using ConcordiaLab.Controllers.Mock_Data.MockModels;

namespace ConcordiaLab.Controllers.Mock_Data.ViewModel
{
    public class MockDashboard
    {
        public MockDashboard(IEnumerable<MockList> mockLists) //, IEnumerable<MockExperiment> mockExperiments, IEnumerable<MockScientist> mockScientist)
        {
            Lists = mockLists;
            //Experiments = mockExperiments;
            //Scientists = mockScientist;
        }

        public MockDashboard() : this(new List<MockList>()) //, new List<MockExperiment>(), new List<MockScientist>())
        {
        }

        public IEnumerable<MockList> Lists { get; }
        //public IEnumerable<MockExperiment> Experiments { get; }
        //public IEnumerable<MockScientist> Scientists { get; }

        //public int SelectedScientist { get; set; }
        //public string Title { get; set; } = string.Empty;
        //public string Body { get; set; } = string.Empty;
    }
}
