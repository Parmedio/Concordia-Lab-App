namespace PersistentLayer.Models;

public record Comment(int Id = default, string Body = "", DateTime Date = default, string CreatorName = "")
{
    public string? TrelloId { get; set; }
    public int ExperimentId { get; set; } = 0!;
    public int? ScientistId { get; set; }
    public virtual Experiment Experiment { get; set; } = null!;
    public virtual Scientist? Scientist { get; set; } = null!;
}