using ConcordiaLab.Controllers.Mock_Data.MockModels;

namespace ConcordiaLab.Controllers.Mock_Data.ViewModel
{
    public class MockDashboard
    {
        public MockDashboard(IEnumerable<MockList> mockLists)
        {
            Lists = mockLists;
        }

        public MockDashboard() : this(new List<MockList>())
        {
        }

        public IEnumerable<MockList> Lists { get; }
    }
}
