using System.ComponentModel.DataAnnotations.Schema;

namespace PersistentLayer.Models;

public record Experiment(int Id = default, string TrelloId = null!, string Title = null!, string? Description = null!, DateTime? DeadLine = default)
{
    public virtual Label? Label { get; set; }
    public int? LabelId { get; set; }

    public virtual Column? List { get; set; }
    public int ListId { get; set; }

    public virtual IEnumerable<Comment>? Comments { get; set; }

    public virtual IEnumerable<Scientist>? Scientists { get; set; }

    [NotMapped]
    public virtual IEnumerable<int>? ScientistsIds { get; set; }
}
