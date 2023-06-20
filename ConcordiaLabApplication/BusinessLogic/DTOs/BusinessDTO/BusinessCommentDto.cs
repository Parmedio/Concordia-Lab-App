namespace BusinessLogic.DTOs.BusinessDTO;

public record BusinessCommentDto
{
    public int? Id { get; set; }
    public int CardID { get; set; } = 0!;
    public string TrelloCardId { get; set; } = null!;
    public string CommentText { get; set; } = null!;
    public string CreatorName { get; set; } = null!;

    public BusinessScientistDto? Scientist { get; set; } = null!;
}
