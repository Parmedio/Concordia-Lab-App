namespace ConcordiaLab.Controllers.Mock_Data.MockModels
{
    public record MockScientist
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        
        public MockScientist(int id, string name, string surname)
        {
            Id = id;
            Name = name;
            Surname = surname;
        }
    }
}
