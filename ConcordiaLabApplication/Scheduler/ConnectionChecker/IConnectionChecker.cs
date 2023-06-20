namespace Scheduler;

public interface IConnectionChecker
{
    public Task<bool> CheckConnection();
}