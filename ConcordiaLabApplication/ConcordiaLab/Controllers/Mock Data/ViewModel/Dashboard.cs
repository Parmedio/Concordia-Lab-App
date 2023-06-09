using ConcordiaLab.Controllers.Mock_Data.MockModels;

namespace ConcordiaLab.Controllers.Mock_Data.ViewModel
{
    public class Dashboard
    {
        public Dashboard(IEnumerable<MockList> mockLists)
        {
            Lists = mockLists;
        }

        public Dashboard() : this(new List<MockList>())
        {
        }

        public IEnumerable<MockList> Lists { get; }
    }
}
