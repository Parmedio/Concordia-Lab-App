using ConcordiaLab.Controllers.Mock_Data.MockModels;

namespace ConcordiaLab.Controllers.Mock_Data.Mock_Gateway
{
    public class MockGatewayList
    {
        private readonly IEnumerable<MockList> _mockList = new List<MockList>
        {
            new(1, "To Do", new List<int>(){3, 4, 5}),
            new(2, "In Progress", new List<int>(){1, 2}),
            new(3, "Completed", new List<int>(){6, 7}),
        };

        public IEnumerable<MockList> GetAll() => _mockList;
        public MockList? GetById(int id) => _mockList.SingleOrDefault(ml => ml.Id == id);
    }
}
