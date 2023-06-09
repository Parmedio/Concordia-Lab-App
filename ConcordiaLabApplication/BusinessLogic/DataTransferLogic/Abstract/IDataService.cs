using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Abstract;

public interface IDataService
{
    public IEnumerable<BusinessListDto> GetAllLists(int scientistId);
    public BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId);
    public BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto);
}
