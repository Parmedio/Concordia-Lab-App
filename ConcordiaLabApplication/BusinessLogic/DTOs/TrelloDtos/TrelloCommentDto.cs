namespace BusinessLogic.DTOs.TrelloDtos;

public record TrelloCommentDto
{
    public string? Id { get; set; }
    public string? IdMemberCreator { get; set; }
    public DataInTrelloCommentDto Data { get; set; } = null!;
    public DateTime? Date { get; set; }
}

public record DataInTrelloCommentDto
{
    public string Text { get; set; } = null!;
    public CardInDataInTrelloCommentDto Card { get; set; } = null!;
}

public record CardInDataInTrelloCommentDto
{
    public string Id { get; set; } = null!;
}