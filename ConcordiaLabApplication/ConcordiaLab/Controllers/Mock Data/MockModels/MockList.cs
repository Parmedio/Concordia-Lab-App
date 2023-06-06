namespace ConcordiaLab.Controllers.Mock_Data.MockModels
{
    public class MockList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<int>? IntExperiments { get; set; } //passo gli ID dei vari MockExperiment
        public IEnumerable<MockExperiment>? Experiments { get; set; }

        public MockList(int id, string name, IEnumerable<int>? experiments)
        {
            Id = id;
            Name = name;
            IntExperiments = experiments;
        }
    }
}
