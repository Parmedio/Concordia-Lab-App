using ConcordiaLab.Controllers.Mock_Data.MockModels;

namespace ConcordiaLab.Controllers.Mock_Data.Mock_Gateway
{
    public class MockGatewayScientist
    {
        private readonly IEnumerable<MockScientist> _mockScientists = new List<MockScientist>
        {
            new(1, "Mario", "Rossi"),
            new(2, "Luca", "Verdi"),
            new(3, "Marco", "Da Pieve"),
            new(4, "Gabriele", "Cecutti"),
            new(5, "Alessandro", "Ferluga")
        };

        public IEnumerable<MockScientist> GetAll() => _mockScientists;
        public MockScientist? GetById(int id) => _mockScientists.SingleOrDefault(ms => ms.Id == id);
    }
}
