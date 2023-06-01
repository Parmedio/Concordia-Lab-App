namespace ConcordiaLab.Mock_Data.MockModels
{
    public class MockList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<int>? Experiments { get; set; } //passo gli ID dei vari MockExperiment

        public MockList(int id, string name, IEnumerable<int>? experiments)
        {
            Id = id;
            Name = name;
            Experiments = experiments;
        }
    }
}
