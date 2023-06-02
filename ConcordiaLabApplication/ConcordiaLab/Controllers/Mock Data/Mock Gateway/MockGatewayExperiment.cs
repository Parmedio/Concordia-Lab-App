using ConcordiaLab.Controllers.Mock_Data.MockModels;

namespace ConcordiaLab.Controllers.Mock_Data.Mock_Gateway
{
    public class MockGatewayExperiment
    {
        private readonly IEnumerable<MockExperiment> _mockExperiment = new List<MockExperiment>
        {
            new(1, "Esperimento 01", "Questo esperimento vuole verificare... ", new DateTime(2023, 1, 1, 22, 22, 33), "High Priority", "Esperimento Riuscito", new List<int>(){1}),
            new(2, "Esperimento 02", "Questo esperimento vuole verificare... ", new DateTime(2023, 2, 1, 22, 22, 33), "Medium Priority", "Esperimento Riuscito", new List<int>(){2}),
            new(3, "Esperimento 03", "Questo esperimento vuole verificare... ", new DateTime(2023, 3, 1, 22, 22, 33), "Low Priority", "Esperimento Riuscito", new List<int>(){3}),
            new(4, "Esperimento 04", "Questo esperimento vuole verificare... ", new DateTime(2023, 4, 1, 22, 22, 33), "High Priority", "Esperimento Riuscito", new List<int>(){4}),
            new(5, "Esperimento 05", "Questo esperimento vuole verificare... ", new DateTime(2023, 5, 1, 22, 22, 33), "Medium Priority", "Esperimento Riuscito", new List<int>(){5}),
            new(6, "Esperimento 06", "Questo esperimento vuole verificare... ", new DateTime(2023, 6, 1, 22, 22, 33), "Low Priority", "Esperimento Riuscito", new List<int>(){1, 2}),
            new(7, "Esperimento 07", "Questo esperimento vuole verificare... ", new DateTime(2023, 7, 1, 22, 22, 33), "High Priority", "Esperimento Riuscito", new List<int>(){3, 4, 5})
        };

        public IEnumerable<MockExperiment> GetAll() => _mockExperiment;
        public MockExperiment? GetById(int id) => _mockExperiment.SingleOrDefault(me => me.Id == id);
    }
}
