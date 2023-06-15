namespace BusinessLogic.DataTransferLogic.Abstract;

public interface IDataSyncer
{
    public Task SynchronizeAsync();
}