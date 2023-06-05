using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DTOs.TrelloDtos;

namespace BusinessLogic.APIConsumers.Concrete;

public class ApiReceiver : IApiReceiver
{
    public IEnumerable<TrelloCommentDto> GetAllComments()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TrelloExperimentDto> GetAllExperimentsInToDoList()
    {
        throw new NotImplementedException();
    }
}
