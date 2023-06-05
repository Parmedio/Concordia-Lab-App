namespace BackgroundServices;

public class RetrieveConnectionTimeInterval : IRetrieveConnectionTimeInterval
{
    public bool IsTimeInInterval()
    {
        return new Random().Next(0, 2) == 0;
    }
}
