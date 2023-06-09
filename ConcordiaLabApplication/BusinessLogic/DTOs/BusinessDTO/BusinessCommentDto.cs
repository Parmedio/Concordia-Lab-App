namespace BusinessLogic.DTOs.BusinessDTO;

public record BusinessCommentDto
{
    public string TrelloCardId { get; set; } = null!;
    public int CardID { get; set; } = 0!;
    public string CommentText { get; set; } = null!;

    public BusinessScientistDto Scientist { get; set; } = null!;


}
