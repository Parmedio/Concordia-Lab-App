using BusinessLogic.DataTransferLogic.Abstract;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class DataHandlerFactory : IDataHandlerFactory
{
    private readonly IApiSender _apiSender;
    private readonly DataService _dataService;

    public DataHandlerFactory(IApiSender apiSender, DataService dataService)
    {
        _apiSender = apiSender;
        _dataService = dataService;
    }

    public IDataService DataServiceFactoryMethod(bool connectionAvailable)
    {
        if (connectionAvailable)
            return new ConnectionConcreteDecorator(_dataService, _apiSender);
        return _dataService;
    }
}
