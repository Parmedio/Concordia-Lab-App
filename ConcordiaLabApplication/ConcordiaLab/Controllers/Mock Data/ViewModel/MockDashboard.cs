using ConcordiaLab.Controllers.Mock_Data.MockModels;

namespace ConcordiaLab.Controllers.Mock_Data.ViewModel
{
    public class MockDashboard
    {
        public MockDashboard(IEnumerable<MockList> mockLists, IEnumerable<MockScientist> allScientist)
        {
            Lists = mockLists;
            AllScientists = allScientist;
        }

        public MockDashboard() : this(new List<MockList>(), new List<MockScientist>())
        {
        }

        public IEnumerable<MockList> Lists { get; }
        public IEnumerable<MockScientist> AllScientists { get; }
    }
}
