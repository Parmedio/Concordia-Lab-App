namespace BusinessLogic.DTOs.BusinessDTO;

public record BusinessExperimentDto
{
    public string TrelloCardId { get; set; } = null!;
    public string TrelloListId { get; set; } = null!;
}
