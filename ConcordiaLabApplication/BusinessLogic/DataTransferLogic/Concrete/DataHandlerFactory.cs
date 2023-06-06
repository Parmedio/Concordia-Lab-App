using BusinessLogic.DataTransferLogic.Abstract;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class DataHandlerFactory : IDataHandlerFactory
{
    private readonly IApiSender _apiSender;

    public DataHandlerFactory(IApiSender apiSender)
    {
        _apiSender = apiSender;
    }

    public IDataService DataServiceFactoryMethod(bool connectionAvailable)
    {
        if (connectionAvailable)
            return new ConnectionConcreteDecorator(new DataService(), _apiSender);
        return new DataService();
    }
}
