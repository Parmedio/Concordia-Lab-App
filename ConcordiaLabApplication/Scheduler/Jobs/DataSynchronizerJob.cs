using BusinessLogic.DataTransferLogic.Abstract;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Quartz;

namespace Scheduler.Jobs;

public class DataSynchronizerJob : IJob
{
    private readonly ILogger _logger;
    private readonly TimeSpan _delay;
    private readonly IServiceScopeFactory _scopeFactory;
    private static bool _connectionAchieved = false;

    public DataSynchronizerJob(ILogger<DataSynchronizerJob> logger, IServiceScopeFactory scopeFactory)
    {
        _connectionAchieved = false;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public void ChangeConnectionState()
        => _connectionAchieved = !_connectionAchieved;

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await using AsyncServiceScope asyncScope = _scopeFactory.CreateAsyncScope();
            var connectionChecker = asyncScope.ServiceProvider.GetRequiredService<IConnectionChecker>();
            var dataService = asyncScope.ServiceProvider.GetRequiredService<IClientService>();

            string connectionState;

            if (await connectionChecker.CheckConnection())
            {
                if (!_connectionAchieved)
                {
                    _logger.LogInformation("Connection Established!");
                    dataService.GenerateReport();
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
                _logger.LogInformation($"Connection not available, not entering in the cycle.");
            }

        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Failed to run through the cycle, obtained error: {ex.Message}");
        }
    }
}

