using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DTOs.TrelloDtos;

namespace BusinessLogic.APIConsumers.Concrete;

public class ApiSender : IApiSender
{
    public bool AddAComment(TrelloCommentDto commentDto)
    {
        throw new NotImplementedException();
    }

    public bool AddAllLastComments(IEnumerable<TrelloCommentDto> comments)
    {
        throw new NotImplementedException();
    }

    public bool UpdateAllExperiments(IEnumerable<TrelloExperimentDto> experiments)
    {
        throw new NotImplementedException();
    }

    public bool UpdateAnExperiment(TrelloExperimentDto experiment)
    {
        throw new NotImplementedException();
    }
}
