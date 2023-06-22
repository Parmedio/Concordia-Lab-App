using AutoMapper;

using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DataTransferLogic.DateTimeConverter;
using BusinessLogic.DTOs.BusinessDTO;
using BusinessLogic.DTOs.ReportDto;

using ReportSender;
using ReportSender.ReportDto;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class ClientService : IClientService
{
    public static bool ConnectionIsAvailable { get; private set; } = false;
    private readonly IDataHandlerFactory _dataHandlerFactory;
    private readonly IDataService _dataHandler;
    private readonly IDataSyncer _dataSyncer;
    private readonly IConcordiaReportRunner _reportCreator;
    private readonly IMapper _mapper;

    public ClientService(IDataHandlerFactory dataHandlerFactory, IDataSyncer dataSyncer, IMapper mapper, IConcordiaReportRunner reportCreator)
    {
        _dataHandlerFactory = dataHandlerFactory;
        _dataHandler = _dataHandlerFactory.DataServiceFactoryMethod(ConnectionIsAvailable);
        _dataSyncer = dataSyncer;
        _mapper = mapper;
        _reportCreator = reportCreator;
    }

    public async Task<bool> UpdateConnectionStateAsync(bool connectionState)
    {
        return await Task.Run(() =>
        {
            ConnectionIsAvailable = connectionState;
            return ConnectionIsAvailable;
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
        if (ConnectionIsAvailable) await _dataSyncer.SynchronizeAsync();
    }

    public Task<string> GenerateReport(bool sendMail = true)
    {
        var currentDate = DateTime.UtcNow;
        DateTime localCurrentDate = (DateTime)ConverterFromUTCToLocalTime.ConvertToAntartideTimeZone(currentDate)!;
        var experiments = _dataHandler.GetAllExperiments();
        var scientists = _dataHandler.GetAllScientistsWithExperiments();
        return _reportCreator.Run(
            _mapper.Map<IEnumerable<BusinessExperimentDto>,
            IEnumerable<ExperimentForReportDto>>(experiments),
            scientists,
            sendMail,
            localCurrentDate!);
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

    public bool GetConnection()
    {
        return ConnectionIsAvailable;
    }
}
