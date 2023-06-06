namespace BackgroundServices;

public interface IRetrieveConnectionTimeInterval
{
    (bool, TimeSpan) IsTimeInInterval(DateTime currentDate);
}