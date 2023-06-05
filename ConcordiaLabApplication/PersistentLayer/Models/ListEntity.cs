
namespace PersistentLayer.Models;

public record ListEntity(int Id = default, string TrelloId = null!, string Title = null!)
{
    public virtual IEnumerable<Experiment>? Experiments { get; set; }
}