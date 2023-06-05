namespace PersistentLayer.Models;

public record Scientist(int Id = default, string TrelloToken = null!, string TrelloMemberId = null!, string Name = null!)
{
    public virtual IEnumerable<Experiment>? Experiments {get; set; }
}