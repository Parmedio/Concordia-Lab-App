using ConcordiaLab.Mock_Data.MockModels;

namespace ConcordiaLab.Mock_Data
{
    public class MockGatewayScientist
    {
        private readonly IEnumerable<MockScientist> _mockScientists = new List<MockScientist>
        {
            new(1, "Alberto", "Viezzi"),
            new(2, "Dario", "Viezzi"),
            new(3, "Marco", "Da Pieve"),
            new(4, "Gabriele", "Cecutti"),
            new(5, "Alessandro", "Ferluga")
        };

        public IEnumerable<MockScientist> GetAll() => _mockScientists;
        public MockScientist? GetById(int id) => _mockScientists.SingleOrDefault(ms => ms.Id == id);
    }
}
