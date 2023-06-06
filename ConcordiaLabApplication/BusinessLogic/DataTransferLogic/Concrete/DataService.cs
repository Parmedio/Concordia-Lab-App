using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class DataService : IDataService
{

    public BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId)
    {
        throw new NotImplementedException();
    }

    public List<BusinessListDto>? GetAllLists(int scientistId)
    {
        throw new NotImplementedException();
    }
    public BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto)
    {
        throw new NotImplementedException();
    }


}
