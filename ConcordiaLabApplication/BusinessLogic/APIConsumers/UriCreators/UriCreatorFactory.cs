using Microsoft.Extensions.Configuration;

namespace BusinessLogic.APIConsumers.UriCreators;

public class UriCreatorFactory : IUriCreatorFactory
{
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;
    private readonly string _concordiaToken;
    private readonly string _ToDoColumnId;
    private readonly string _BoardId;

    public UriCreatorFactory(IConfiguration configuration)
    {
        _configuration = configuration;
        _apiKey = _configuration.GetSection("TrelloAuthorization").GetSection("Key").Value!;
        _concordiaToken = _configuration.GetSection("TrelloAuthorization").GetSection("Token").Value!;
#if DEBUG
        _ToDoColumnId = _configuration.GetSection("TrelloTestEnvironment").GetSection("Column").GetSection("idToDo").Value!;
        _BoardId = _configuration.GetSection("TrelloTestEnvironment").GetSection("idBoard").Value!;
#else
        _ToDoColumnId = _configuration.GetSection("TrelloIDsDevelopment").GetSection("Column").GetSection("idToDo").Value!;
        _BoardId = _configuration.GetSection("TrelloIDsDevelopment").GetSection("idBoard").Value!;
#endif
    }

    public string GetAllCardsOnToDoColumn()
        => $"lists/{_ToDoColumnId}/cards?{GetBaseAuth()}";
 
    public string GetAllCommentsOnABoard()
        => $"boards/{_BoardId}/actions?filter=commentCard&{GetBaseAuth()}";

    public string UpdateAnExperiment(string cardId, string columnId)
    => $"cards/{cardId}?idList={columnId}&{GetBaseAuth()}";

    public string AddACommentOnACard(string cardId, string text, string authToken)
    => $"cards/{cardId}/actions/comments?text={text}&key={_apiKey}&token={authToken}";

    private string GetBaseAuth()
        => $"key={_apiKey}&token={_concordiaToken}";
}