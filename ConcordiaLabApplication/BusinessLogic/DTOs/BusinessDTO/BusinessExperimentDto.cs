namespace BusinessLogic.DTOs.BusinessDTO;

public record BusinessExperimentDto
{
    public int Id { get; set; } = 0!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? Date { get; set; }
    public int ListId { get; set; } = 0!;

    public string TrelloCardId { get; set; } = null!;
    public string TrelloListId { get; set; } = null!;
    public int ScientistId { get; set; } = 0!;

}
