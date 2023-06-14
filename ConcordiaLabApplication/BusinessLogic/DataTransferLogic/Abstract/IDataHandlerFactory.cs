namespace BusinessLogic.DataTransferLogic.Abstract;

public interface IDataHandlerFactory
{
    IDataService DataServiceFactoryMethod(bool connectionAvailable);
}