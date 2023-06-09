using ConcordiaLab.Controllers.Mock_Data.MockModels;

namespace ConcordiaLab.Controllers.Mock_Data.Mock_Gateway
{
    public class MockGatewayExperiment
    {
        private readonly static string descrizione = "Charmander (ヒトカゲ Hitokage?) è un Pokémon appartenente alla prima generazione. Ideato da Atsuko Nishida e fissato nel suo aspetto finale da Ken Sugimori. Charmander fa la sua prima apparizione nel 1996 nei videogiochi Pokémon Rosso e Blu come uno dei Pokémon iniziali che i giocatori possono scegliere per cominciare la loro avventura. Compare inoltre nella maggior parte dei titoli successivi, in videogiochi spin-off, nella serie televisiva anime, nel Pokémon Trading Card Game e nel merchandising derivato dalla serie.Charmander appare sulle copertine di Pokémon!";

        private readonly IEnumerable<MockExperiment> _mockExperiment = new List<MockExperiment>
        {
            new(1, "Esperimento 01", descrizione, new DateTime(2023, 6, 29, 22, 22, 33), "Low", "Esperimento Riuscito", new List<int>(){1}),
            new(2, "Esperimento 02", descrizione, new DateTime(2023, 6, 29, 22, 22, 33), "Medium", "Esperimento Riuscito", new List<int>(){2}),
            new(3, "Esperimento 03", descrizione, "Low", "Esperimento Riuscito", new List<int>(){3}),
            new(4, "Esperimento 04", descrizione, new DateTime(2023, 6, 8, 22, 22, 33), "High", "Esperimento Riuscito", new List<int>(){4}),
            new(5, "Esperimento 05", descrizione, new DateTime(2023, 6, 10, 22, 22, 33), "Medium", "Esperimento Riuscito", new List<int>(){5}),
            new(6, "Esperimento 06", descrizione, new DateTime(2023, 6, 10, 22, 22, 33), "Low", "Esperimento Riuscito", new List<int>(){1, 2}),
            new(7, "Esperimento 07", descrizione, new DateTime(2023, 6, 8, 22, 22, 33), "High", "Esperimento Riuscito", new List<int>(){3, 4, 5})
        };

        public IEnumerable<MockExperiment> GetAll() => _mockExperiment;
        public IEnumerable<MockExperiment> GetAllFromScientistId(int scientistId) => _mockExperiment.Where(experiment => experiment.IntScientists.Contains(scientistId));
        public MockExperiment? GetById(int id) => _mockExperiment.SingleOrDefault(me => me.Id == id);
    }
}
