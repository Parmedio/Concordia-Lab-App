using Microsoft.Extensions.Configuration;

namespace BusinessLogic.APIConsumers.UriCreators;

public class UriCreatorFactory : IUriCreatorFactory
{
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;
    private readonly string _concordiaToken;
    private readonly string _ToDoListId;
    private readonly Uri _baseUri;

    public UriCreatorFactory(IConfiguration configuration)
    {
        _configuration = configuration;
        _apiKey = _configuration.GetSection("TrelloAuthorization").GetSection("Key").Value!;
        _concordiaToken = _configuration.GetSection("TrelloAuthorization").GetSection("Token").Value!;
        _baseUri = new Uri(_configuration.GetSection("TrelloUrlToUse").GetSection("baseUrl").Value!);
#if DEBUG
        _ToDoListId = _configuration.GetSection("TrelloTestEnvironment").GetSection("List").GetSection("idToDo").Value!;
#else
        _ToDoListId = _configuration.GetSection("TrelloIDsDevelopment").GetSection("List").GetSection("idToDo").Value!;
# endif
    }

    public Uri GetAllCardsOnToDoList()
    {
        return new Uri(_baseUri, $"lists/{_ToDoListId}/cards?key={_apiKey}&token={_concordiaToken}");
    }
}
