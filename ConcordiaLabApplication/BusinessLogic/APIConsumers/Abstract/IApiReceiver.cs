using BusinessLogic.DTOs.TrelloDtos;

namespace BusinessLogic.APIConsumers.Abstract;

public interface IApiReceiver
{
    public Task<IEnumerable<TrelloExperimentDto>?> GetAllExperimentsInToDoColumn();
    public Task<IEnumerable<TrelloCommentDto>?> GetAllComments();

}
