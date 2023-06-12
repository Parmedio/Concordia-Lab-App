using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class ClientService : IClientService
{
    private static bool _connectionAvailable = false;
    private readonly IDataService _dataHandler;
    private readonly IDataSyncer _dataSyncer;
    private readonly IDataHandlerFactory _dataHandlerFactory;

    public ClientService(IDataHandlerFactory dataHandlerFactory, IDataSyncer dataSyncer)
    {
        _dataHandlerFactory = dataHandlerFactory;
        _dataHandler = _dataHandlerFactory.DataServiceFactoryMethod(_connectionAvailable);
        _dataSyncer = dataSyncer;
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

    public IEnumerable<BusinessListDto> GetAllLists(int scientistId)
    {
        return _dataHandler.GetAllLists(scientistId);
    }

    public BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto)
    {
        return _dataHandler.MoveExperiment(businessExperimentDto);
    }

    public async Task SyncDataAsyncs()
    {
        if (_connectionAvailable)
        {
            await _dataSyncer.SynchronizeAsync();
        }
    }

    public IEnumerable<BusinessListDto> GetAllLists()
    {
        return _dataHandler.GetAllLists();
    }

    public IEnumerable<BusinessExperimentDto> GetAllExperiments()
    {
        return _dataHandler.GetAllExperiments();
    }

    public IEnumerable<BusinessExperimentDto> GetAllExperiments(int scientistId)
    {
        return _dataHandler.GetAllExperiments(scientistId);
    }

    public IEnumerable<BusinessScientistDto> GetAllScientist()
    {
        return _dataHandler.GetAllScientist();
    }

    public BusinessExperimentDto GetExperimentById(int experimentId)
    {
        return _dataHandler.GetExperimentById(experimentId);
    }
}
