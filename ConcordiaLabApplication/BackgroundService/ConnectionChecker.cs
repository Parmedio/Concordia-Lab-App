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
    private TimeSpan _actualDelay;
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
        _delay = TimeSpan.FromSeconds(Convert.ToDouble(_configuration.GetSection("ConnectionCheckerInfo").GetSection("delay").Value)!);
        _scopeFactory = scopeFactory;
        _actualDelay = _delay;
    }

    public void ChangeConnectionState()
    {
        _connectionAchieved = !_connectionAchieved;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_actualDelay);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await using AsyncServiceScope asyncScope = _scopeFactory.CreateAsyncScope();
                IDataService dataService = asyncScope.ServiceProvider.GetRequiredService<IDataService>();
                string connectionState;
                (bool, TimeSpan) connectionInfo = _timeRetriever.IsTimeInInterval(DateTime.Now);

                if (connectionInfo.Item1)
                {


                    if (!_connectionAchieved)
                    {
                        _logger.LogInformation("Connection Established!");
                        ChangeConnectionState();
                        _actualDelay = _delay;
                    }

                    connectionState = await dataService.UpdateConnectionStateAsync(true) ? "Online" : "Offline";
                    _logger.LogInformation($"Executed new cycle. current connection state is {connectionState}");

                }
                else if (_connectionAchieved)
                {
                    ChangeConnectionState();
                    connectionState = await dataService.UpdateConnectionStateAsync(false) ? "Online" : "Offline";
                    _logger.LogInformation($"Connection interrupted, shutting down periodic service. connection state is {connectionState}");
                    _actualDelay = connectionInfo.Item2;
                }
                else
                {
                    _logger.LogInformation($"Connection not available, not entering in the cycle. next connection expected in {connectionInfo.Item2:hh\\:mm\\:ss}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to run through the cycle, obtained error: {ex.Message}");
            }
        }
    }
}
