using System.ComponentModel.DataAnnotations.Schema;

namespace PersistentLayer.Models;

public record Scientist(int Id = default, string TrelloToken = null!, string TrelloMemberId = null!, string Name = null!)
{
    public virtual IEnumerable<Experiment>? Experiments { get; set; }
    [NotMapped]
    public virtual IEnumerable<int>? ExperimentsIds { get; set; }
}