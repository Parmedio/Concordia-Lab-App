namespace PersistentLayer.Models;

public record Experiment (int Id = default, string TrelloId = null!, string Title = null!, string Description = null!, DateTime? DeadLine = default)
{ 
    public virtual Priority? Priority { get; set; }
    public int PriorityId { get; set; }

    public virtual Label? Label { get; set; }
    public int LabelId { get; set; }

    public virtual ListEntity? List { get; set; }
    public int ListId { get; set; }

    public virtual IEnumerable<Comment>? Comments { get; set; }

    public virtual IEnumerable<Scientist>? Scientists { get; set; }
}