

public interface IApiSender
{
    public Task<bool> AddAComment(string cardId, string commentText, string authToken);
    public Task<bool> UpdateAnExperiment(string cardId, string newListId, string authToken);
}
