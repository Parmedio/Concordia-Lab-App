namespace PersistentLayer.Models
{
    public record Comment(int Id = default, string? TrelloId = default, string Body = "", DateTime Date = default, string CreatorName = "")
    {
        public virtual Experiment Experiment { get; set; } = null!;
        public virtual Scientist? Scientist { get; set; } = null!;

        public int? ScientistId { get; set; }
        public int ExperimentId { get; set; } = 0!;
    }

}
