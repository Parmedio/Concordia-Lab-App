using BusinessLogic.DTOs.TrelloDtos;

namespace BusinessLogic.APIConsumers.Abstract;

public interface IApiReceiver
{
    public IEnumerable<TrelloExperimentDto> GetAllExperimentsInToDoList();
    public IEnumerable<TrelloCommentDto> GetAllComments();

}
