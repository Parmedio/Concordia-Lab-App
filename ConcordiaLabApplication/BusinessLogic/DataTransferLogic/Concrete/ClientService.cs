using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class ClientService : IClientService
{
    private static bool _connectionAvailable = false;
    private readonly IDataHandlerFactory _dataHandlerFactory;
    private readonly IDataService _dataHandler;
    private readonly IDataSyncer _dataSyncer;

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
        => _dataHandler.AddComment(businessCommentDto, scientistId);

    public IEnumerable<BusinessColumnDto> GetAllColumns(int scientistId)
        => _dataHandler.GetAllColumns(scientistId);

    public BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto) 
        => _dataHandler.MoveExperiment(businessExperimentDto);

    public async Task SyncDataAsyncs()
    {
        if (_connectionAvailable) await _dataSyncer.SynchronizeAsync();
    }

    public IEnumerable<BusinessColumnDto> GetAllColumns()
        => _dataHandler.GetAllColumns();

    public IEnumerable<BusinessExperimentDto> GetAllExperiments()
        => _dataHandler.GetAllExperiments();

    public IEnumerable<BusinessExperimentDto> GetAllExperiments(int scientistId)
        => _dataHandler.GetAllExperiments(scientistId);

    public IEnumerable<BusinessScientistDto> GetAllScientist()
        => _dataHandler.GetAllScientist();

    public BusinessExperimentDto GetExperimentById(int experimentId)
        => _dataHandler.GetExperimentById(experimentId);

    public IEnumerable<BusinessColumnDto> GetAllSimple()
        => _dataHandler.GetAllSimple();
}
