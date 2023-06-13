namespace BusinessLogic.DTOs.BusinessDTO;

public record BusinessExperimentDto
{
    public int Id { get; set; } = 0!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? Date { get; set; }
    public int ListId { get; set; } = 0!;
    public string ListName { get; set; } = null!;

    public string? Priority { get; set; }

    public string TrelloCardId { get; set; } = null!;
    public string TrelloListId { get; set; } = null!;
    
    public IEnumerable<BusinessScientistDto>? Scientists { get; set; }

    public BusinessCommentDto? lastComment { get; set; }
}
