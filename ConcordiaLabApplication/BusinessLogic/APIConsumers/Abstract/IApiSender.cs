using BusinessLogic.DTOs.BusinessDTO;
using BusinessLogic.DTOs.TrelloDtos;

namespace BusinessLogic.APIConsumers.Abstract;

public interface IApiSender
{
    public bool UpdateAllExperiments(IEnumerable<TrelloExperimentDto> experiments);
    public bool AddAComment(BusinessCommentDto commentDto);
    public bool AddAllLastComments(IEnumerable<TrelloCommentDto> comments);
    public bool UpdateAnExperiment(TrelloExperimentDto experiment);
}
