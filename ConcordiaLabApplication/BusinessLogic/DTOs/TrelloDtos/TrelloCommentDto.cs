namespace BusinessLogic.DTOs.TrelloDtos;

public record TrelloCommentDto
{
    public int? DatabaseID { get; set; }
    public string Id { get; set; } = null!;
    public string IdMemberCreator { get; set; } = null!;
    public DataInTrelloCommentDto Data { get; set; } = null!;
    public MemberCreatorInTrelloDto MemberCreator { get; set; } = null!;
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

public record MemberCreatorInTrelloDto
{
    public string Username { get; set; } = null!;
}