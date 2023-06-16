using BackgroundServices.Abstract;
using Microsoft.Extensions.Configuration;

namespace BackgroundServices.Concrete;

public class RetrieveConnectionTimeInterval : IRetrieveConnectionTimeInterval
{
    private readonly DateTime _startTime;
    private readonly TimeSpan _dailyOffset;
    private readonly TimeSpan _connectionDuration;
    private readonly IConfiguration _configuration;
    private static DateTime _nextIntervalStart;


    public RetrieveConnectionTimeInterval(IConfiguration configuration)
    {
        _configuration = configuration;

        _startTime = DateTime.Parse(_configuration.GetSection("ConnectionCheckerInfo:ConnectionIntervalInfo:initialDate").Value!);
        _dailyOffset = TimeSpan.Parse(_configuration.GetSection("ConnectionCheckerInfo:ConnectionIntervalInfo:offset").Value!);
        _connectionDuration = TimeSpan.Parse(_configuration.GetSection("ConnectionCheckerInfo:ConnectionIntervalInfo:duration").Value!);
    }

    public (bool, TimeSpan) IsTimeInInterval(DateTime currentDate)
    {

        CalculateNextInterval(currentDate);

        if (currentDate >= _nextIntervalStart && currentDate <= _nextIntervalStart + _connectionDuration)
        {
            return (true, TimeSpan.Zero);
        }
        return (false, _nextIntervalStart - currentDate);
    }

    private void CalculateNextInterval(DateTime currentDate)
    {
        DateTime higherDateTime = _nextIntervalStart > _startTime ? _nextIntervalStart : _startTime;
        while (higherDateTime + _connectionDuration < currentDate)
        {
            higherDateTime += _dailyOffset;
        }
        _nextIntervalStart = higherDateTime;
    }
}
