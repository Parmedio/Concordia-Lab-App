using BusinessLogic.APIConsumers.UriCreators;

using Newtonsoft.Json;

namespace BusinessLogic.APIConsumers.Concrete;

public class ApiSender : IApiSender
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IUriCreatorFactory _uriCreatorFactory;

    public ApiSender(IHttpClientFactory clientFactory, IUriCreatorFactory uriCreatorFactory)
    {
        _clientFactory = clientFactory;
        _uriCreatorFactory = uriCreatorFactory;
    }

    public async Task<string> AddAComment(string cardId, string commentText, string authToken)
    {
        var client = _clientFactory.CreateClient("ApiConsumer");
        var response = await client.PostAsync(_uriCreatorFactory.AddACommentOnACard(cardId, commentText, authToken), null);
        response.EnsureSuccessStatusCode();
        var Id = JsonConvert.DeserializeObject<TrelloId>(await response.Content.ReadAsStringAsync(), settings: null)?.Id ?? string.Empty;
        return Id;
    }

    public async Task<bool> UpdateAnExperiment(string cardId, string newListId)
    {
        var client = _clientFactory.CreateClient("ApiConsumer");
        var response = await client.PutAsync(_uriCreatorFactory.UpdateAnExperiment(cardId, newListId), null);
        return response.IsSuccessStatusCode;
    }

    private class TrelloId
    {
        public string Id { get; set; } = null!;
    }
}

