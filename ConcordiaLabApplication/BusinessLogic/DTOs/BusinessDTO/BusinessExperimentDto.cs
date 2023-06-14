namespace BusinessLogic.DTOs.BusinessDTO;

public record BusinessExperimentDto
{
    public int Id { get; set; } = 0!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? Date { get; set; }
    public int ColumnId { get; set; } = 0!;
    public string ColumnName { get; set; } = null!;

    public string? Priority { get; set; }

    public string TrelloCardId { get; set; } = null!;
    public string TrelloColumnId { get; set; } = null!;

    public IEnumerable<BusinessScientistDto>? Scientists { get; set; }

    public BusinessCommentDto? lastComment { get; set; }
}
