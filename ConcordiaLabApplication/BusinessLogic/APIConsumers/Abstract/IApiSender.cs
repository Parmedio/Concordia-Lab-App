public interface IApiSender
{
    public Task<string> AddAComment(string cardId, string commentText, string authToken);
    public Task<bool> UpdateAnExperiment(string cardId, string newColumnId);
}
