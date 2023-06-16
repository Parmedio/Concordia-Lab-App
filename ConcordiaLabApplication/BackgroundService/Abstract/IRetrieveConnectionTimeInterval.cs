namespace BackgroundServices.Abstract;

public interface IRetrieveConnectionTimeInterval
{
    (bool, TimeSpan) IsTimeInInterval(DateTime currentDate);
}