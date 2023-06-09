
using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.APIConsumers.UriCreators;
using BusinessLogic.DTOs.TrelloDtos;

using System.Net.Http.Json;

namespace BusinessLogic.APIConsumers.Concrete;

public class ApiReceiver : IApiReceiver
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IUriCreatorFactory _uriCreator;

    public ApiReceiver(IHttpClientFactory httpClientFactory, IUriCreatorFactory uriCreatorFactory)
    {
        _httpClientFactory = httpClientFactory;
        _uriCreator = uriCreatorFactory;
    }

    public async Task<IEnumerable<TrelloExperimentDto>?> GetAllExperimentsInToDoList()
    {
        var client = _httpClientFactory.CreateClient("ApiConsumer");
        var response = await client.GetFromJsonAsync<IEnumerable<TrelloExperimentDto>?>(_uriCreator.GetAllCardsOnToDoList());
        return response;
    }

    public async Task<IEnumerable<TrelloCommentDto>?> GetAllComments()
    {
        var client = _httpClientFactory.CreateClient("ApiConsumer");
        var response = await client.GetFromJsonAsync<IEnumerable<TrelloCommentDto>?>(_uriCreator.GetAllCommentsOnABoard());
        return response;
    }
}
