namespace BusinessLogic.DTOs.BusinessDTO;

public record BusinessCommentDto
{
    public string TrelloCardId { get; set; } = null!;
    public string commentText { get; set; } = null!;

    public BusinessScientistDto scientist { get; set; } = null!;


}
