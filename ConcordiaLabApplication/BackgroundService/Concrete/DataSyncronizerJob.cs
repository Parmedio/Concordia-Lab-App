using BackgroundServices.Abstract;
using BusinessLogic.DataTransferLogic.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace BackgroundServices.Concrete
{
    public class DataSynchronizerJob : IJob
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DataSynchronizerJob> _logger;
        private bool _connectionAchieved;

        public DataSynchronizerJob(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, ILogger<DataSynchronizerJob> logger)
        {
            _connectionAchieved = false;
            _configuration = configuration;
            _scopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        public void ChangeConnectionState()
        {
            _connectionAchieved = !_connectionAchieved;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await using AsyncServiceScope asyncScope = _scopeFactory.CreateAsyncScope();
                IClientService dataService = asyncScope.ServiceProvider.GetRequiredService<IClientService>();
                IConnectionChecker connectionChecker = asyncScope.ServiceProvider.GetRequiredService<IConnectionChecker>();
                string connectionState;

                if (await connectionChecker.CheckConnection())
                {
                    if (!_connectionAchieved)
                    {
                        _logger.LogInformation("Connection Established!");
                        ChangeConnectionState();
                    }

                    connectionState = await dataService.UpdateConnectionStateAsync(true) ? "Online" : "Offline";
                    Task sync = dataService.SyncDataAsyncs();
                    _logger.LogInformation($"Executed new cycle. current connection state is {connectionState}");
                    await sync;
                }
                else if (_connectionAchieved)
                {
                    ChangeConnectionState();
                    connectionState = await dataService.UpdateConnectionStateAsync(false) ? "Online" : "Offline";
                    _logger.LogInformation($"Connection interrupted, shutting down periodic service. connection state is {connectionState}");
                }
                else
                {
                    _logger.LogInformation($"Connection not available, not entering in the cycle"); // aggiungere informazione al log della prossima conessione
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to run through the cycle, obtained error: {ex.Message}");
            }
        }
    }
}
