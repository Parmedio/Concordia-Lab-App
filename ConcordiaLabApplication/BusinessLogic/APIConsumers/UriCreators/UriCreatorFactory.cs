using Microsoft.Extensions.Configuration;

namespace BusinessLogic.APIConsumers.UriCreators;

public class UriCreatorFactory : IUriCreatorFactory
{
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;
    private readonly string _concordiaToken;
    private readonly string _ToDoListId;
    private readonly string _BoardId;

    public UriCreatorFactory(IConfiguration configuration)
    {
        _configuration = configuration;
        _apiKey = _configuration.GetSection("TrelloAuthorization").GetSection("Key").Value!;
        _concordiaToken = _configuration.GetSection("TrelloAuthorization").GetSection("Token").Value!;
#if DEBUG
        _ToDoListId = _configuration.GetSection("TrelloTestEnvironment").GetSection("List").GetSection("idToDo").Value!;
        _BoardId = _configuration.GetSection("TrelloTestEnvironment").GetSection("idBoard").Value!;
#else
        _ToDoListId = _configuration.GetSection("TrelloIDsDevelopment").GetSection("List").GetSection("idToDo").Value!;
        _BoardId = _configuration.GetSection("TrelloIDsDevelopment").GetSection("idBoard").Value!;
#endif
    }

    public string GetAllCardsOnToDoList()
    {
        return $"lists/{_ToDoListId}/cards?{GetBaseAuth()}";
    }

    public string GetAllCommentsOnABoard()
    {
        return $"boards/{_BoardId}/actions?filter=commentCard&{GetBaseAuth()}";
    }

    private string GetBaseAuth()
        => $"key={_apiKey}&token={_concordiaToken}";
}
