using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class ClientService : IDataService
{
    private static bool _connectionAvailable = false;
    private readonly IDataService _dataHandler;
    private readonly IDataHandlerFactory _dataHandlerFactory;

    public ClientService(IDataHandlerFactory dataHandlerFactory)
    {
        _dataHandlerFactory = dataHandlerFactory;
        _dataHandler = _dataHandlerFactory.DataServiceFactoryMethod(_connectionAvailable);
    }

    public async Task<bool> UpdateConnectionStateAsync(bool connectionState)
    {
        return await Task.Run(() =>
        {
            _connectionAvailable = connectionState;
            return _connectionAvailable;
        });
    }

    public BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId)
    {
        return _dataHandler.AddComment(businessCommentDto, scientistId);
    }

    public List<BusinessListDto>? GetAllLists(int scientistId)
    {
        return _dataHandler.GetAllLists(scientistId);
    }

    public BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto)
    {
        return _dataHandler.MoveExperiment(businessExperimentDto);
    }
}
