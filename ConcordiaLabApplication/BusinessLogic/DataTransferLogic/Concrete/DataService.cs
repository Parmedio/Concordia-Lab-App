using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class DataService : IDataService
{
    private bool _isConnected;

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

    public async Task<bool> UpdateConnectionStateAsync(bool connectionState)
    {
        return await Task.Run(() =>
        {
            _isConnected = connectionState;
            return _isConnected;
        });
    }
}
