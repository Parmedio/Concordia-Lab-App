using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class DataService : IDataService
{
    public void AddComment(BusinessExperimentDto businessExperimentDto, int scientistId)
    {
        throw new NotImplementedException();
    }

    public List<BusinessListDto>? GetAllLists(int scientistId)
    {
        throw new NotImplementedException();
    }

    public void MoveExperiment(BusinessExperimentDto businessCardDto, int scientistId)
    {
        throw new NotImplementedException();
    }
}
