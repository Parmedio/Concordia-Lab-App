using System.ComponentModel.DataAnnotations.Schema;

namespace PersistentLayer.Models;

public record Experiment(int Id = default, string TrelloId = null!, string Title = null!, string? Description = null!, DateTime? DeadLine = default)
{
    public int? LabelId { get; set; }
    public int ColumnId { get; set; }
    [NotMapped]
    public IEnumerable<int>? ScientistsIds { get; set; }
    public virtual Column Column { get; set; } = null!;
    public virtual IEnumerable<Comment>? Comments { get; set; }
    public virtual Label? Label { get; set; }
    public virtual IEnumerable<Scientist>? Scientists { get; set; }
}
