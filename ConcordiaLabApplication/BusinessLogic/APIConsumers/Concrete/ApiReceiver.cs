using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.APIConsumers.UriCreators;
using BusinessLogic.DTOs.TrelloDtos;

using System.Net.Http.Json;

namespace BusinessLogic.APIConsumers.Concrete;

public class ApiReceiver : IApiReceiver
{
    private readonly HttpClient _httpClient;
    private readonly IUriCreatorFactory _uriCreator;
    public ApiReceiver(HttpClient httpClient, IUriCreatorFactory uriCreatorFactory)
    {
        _httpClient = httpClient;
        _uriCreator = uriCreatorFactory;
    }

    public async Task<IEnumerable<TrelloExperimentDto>?> GetAllExperimentsInToDoList()
    {
        Uri getAllComments = _uriCreator.GetAllCardsOnToDoList();
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<TrelloExperimentDto>?>(getAllComments);
        Console.WriteLine(response);
        return response;

    }

    public IEnumerable<TrelloCommentDto> GetAllComments()
    {
        throw new NotImplementedException();
    }
}
