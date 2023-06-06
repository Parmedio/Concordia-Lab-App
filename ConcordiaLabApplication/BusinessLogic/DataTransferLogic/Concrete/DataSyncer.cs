using BusinessLogic.APIConsumers.Abstract;

using Microsoft.Extensions.Logging;

using PersistentLayer.Repositories.Abstract;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class DataSyncer
{
    private readonly IApiSender _sender;
    private readonly IApiReceiver _receiver;
    private readonly ILogger<DataSyncer> _logger;
    private readonly IExperimentRepository _experimentRepository;

    public DataSyncer(IApiSender sender, IApiReceiver receiver, ILogger<DataSyncer> logger, IExperimentRepository experimentRepository)
    {
        _sender = sender;
        _receiver = receiver;
        _logger = logger;
        _experimentRepository = experimentRepository;
    }

    public async Task<bool> Download()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Upload()
    {
        var experiments = _experimentRepository.GetAll();
        return await Task.Run(() =>
        {
            foreach (var experiment in experiments)
            {
                if (!_sender.UpdateAnExperiment(experiment.TrelloId, experiment.List!.TrelloId).IsCompletedSuccessfully)
                    return false;

                var commentToAdd = experiment.Comments.Where(p => p.Body is not null && p.Date == experiment.Comments?.Max(g => g.Date)).FirstOrDefault();
                if (!_sender.AddAComment(experiment.TrelloId, commentToAdd.Body, commentToAdd.Scientist.TrelloId)
                    return false;

            }
            return true;
        });
    }
}
