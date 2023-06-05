using BusinessLogic.DataTransferLogic.Abstract;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundServices;

public class ConnectionChecker : BackgroundService
{
    private readonly ILogger _logger;
    private readonly TimeSpan _delay;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly IRetrieveConnectionTimeInterval _timeRetriever;
    private bool _connectionAchieved;

    public ConnectionChecker(ILogger<ConnectionChecker> logger, IConfiguration configuration, IRetrieveConnectionTimeInterval timeRetriever, IServiceScopeFactory scopeFactory)
    {
        _connectionAchieved = false;
        _timeRetriever = timeRetriever;
        _logger = logger;
        _configuration = configuration;
        _delay = TimeSpan.FromSeconds(Convert.ToDouble(configuration.GetSection("ConnectionCheckerDelay").GetSection("delay").Value)!);
        _scopeFactory = scopeFactory;
    }

    public void ChangeConnectionState()
    {
        _connectionAchieved = !_connectionAchieved;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_delay);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await using AsyncServiceScope asyncScope = _scopeFactory.CreateAsyncScope();
                IDataService dataService = asyncScope.ServiceProvider.GetRequiredService<IDataService>();
                string connectionState;

                if (_timeRetriever.IsTimeInInterval())
                {


                    if (!_connectionAchieved)
                    {
                        _logger.LogInformation("Connection Established!");
                        ChangeConnectionState();
                    }

                    connectionState = await dataService.UpdateConnectionStateAsync(true) ? "Online" : "Offline";
                    _logger.LogInformation($"Executed new cycle. current connection state is {connectionState}");

                }
                else if (_connectionAchieved)
                {
                    ChangeConnectionState();
                    connectionState = await dataService.UpdateConnectionStateAsync(false) ? "Online" : "Offline";
                    _logger.LogInformation($"Connection interrupted, shutting down periodic service. connection state is {connectionState}");
                }
                else
                {
                    _logger.LogInformation("Connection not available, not entering in the cycle.");
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to run through the cycle, obtained error: {ex.Message}");
            }
        }
    }
}
