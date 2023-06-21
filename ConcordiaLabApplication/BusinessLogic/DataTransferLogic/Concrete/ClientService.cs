using AutoMapper;

using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.BusinessDTO;
using BusinessLogic.DTOs.ReportDto;

using ReportSender;
using ReportSender.ReportDto;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class ClientService : IClientService
{
    private static bool _connectionAvailable = false;
    private readonly IDataHandlerFactory _dataHandlerFactory;
    private readonly IDataService _dataHandler;
    private readonly IDataSyncer _dataSyncer;
    private readonly IConcordiaReportRunner _reportCreator;
    private readonly IMapper _mapper;

    public ClientService(IDataHandlerFactory dataHandlerFactory, IDataSyncer dataSyncer, IMapper mapper, IConcordiaReportRunner reportCreator)
    {
        _dataHandlerFactory = dataHandlerFactory;
        _dataHandler = _dataHandlerFactory.DataServiceFactoryMethod(_connectionAvailable);
        _dataSyncer = dataSyncer;
        _mapper = mapper;
        _reportCreator = reportCreator;
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

    public void GenerateReport()
    {
        var experiments = _dataHandler.GetAllExperiments();
        var scientists = _dataHandler.GetAllScientistsWithExperiments();
        _reportCreator.Run(
            _mapper.Map<IEnumerable<BusinessExperimentDto>,
            IEnumerable<ExperimentForReportDto>>(experiments),
            scientists);
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

    public IEnumerable<ScientistForReportDto> GetAllScientistsWithExperiments()
        => _dataHandler.GetAllScientistsWithExperiments();
}
