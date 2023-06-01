namespace PersistentLayer.Models;

public record ScientistModel
{
    public int Id { get; set; }
    public string TrelloToken { get; set; } = null!;
    public string TrelloMemberId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public virtual IEnumerable<ExperimentModel>? Experiments { get; set; }
}
