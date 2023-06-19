using BusinessLogic.DataTransferLogic.Abstract;

using PersistentLayer.Repositories.Abstract;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class DataHandlerFactory : IDataHandlerFactory
{
    private readonly IApiSender _apiSender;
    private readonly DataService _dataService;
    private readonly ICommentRepository _commentRepository;

    public DataHandlerFactory(IApiSender apiSender, DataService dataService, ICommentRepository commentRepository)
    {
        _apiSender = apiSender;
        _dataService = dataService;
        _commentRepository = commentRepository;
    }

    public IDataService DataServiceFactoryMethod(bool connectionAvailable)
    {
        if (connectionAvailable)
            return new ConnectionConcreteDecorator(_dataService, _apiSender, _commentRepository);
        return _dataService;
    }
}
