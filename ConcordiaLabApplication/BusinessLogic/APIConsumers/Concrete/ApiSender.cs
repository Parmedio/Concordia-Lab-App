using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.APIConsumers.UriCreators;

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

    public async Task<bool> AddAComment(string cardId, string commentText, string authToken)
    {
        var client = _clientFactory.CreateClient();
        var response = await client.PostAsync(_uriCreatorFactory.AddACommentOnACard(cardId, commentText, authToken), null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAnExperiment(string cardId, string newListId)
    {
        var client = _clientFactory.CreateClient();
        var response = await client.PostAsync(_uriCreatorFactory.UpdateAnExperiment(cardId, newListId), null);
        return response.IsSuccessStatusCode;
    }
}
