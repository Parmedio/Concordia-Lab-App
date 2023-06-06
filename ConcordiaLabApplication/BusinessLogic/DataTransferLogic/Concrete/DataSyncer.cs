using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DTOs.TrelloDtos;
using BusinessLogic.Exceptions;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using PersistentLayer.Models;
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
        var experimentsInToDoList = _receiver.GetAllExperimentsInToDoList();
        var comments = _receiver.GetAllComments();
        if (comments.IsCompletedSuccessfully && !comments.Result.IsNullOrEmpty())
        {
            var latestCommentsToAdd = comments.Result!.GroupBy(p => p.Data.Card.Id).Select(g => g.OrderByDescending(p => p.Date).First());
        }
        return true;
    }

    public async Task<bool> Upload()
    {
        var experiments = _experimentRepository.GetAll();
        var result = await SyncTrelloWithAllUpdates(experiments);

        if (result)
            return true;
        return false;
    }

    private void SyncDatabaseWithAllLatestComments(IEnumerable<TrelloCommentDto> comments)
    {
        foreach (var comment in comments)
        {
            if (_commentRepository.GetByTrelloId(comment.)
        }
    }

    private void SyncDatabaseWithAllExperimentsInToDoList(IEnumerable<TrelloExperimentDto> experiments)
    {
        foreach (var experiment in experiments)
        {
            _experimentRepository.Add(experiment);
        }
    }


    private Task<bool> SyncTrelloWithAllUpdates(IEnumerable<Experiment> experiments)
    {
        foreach (var experiment in experiments)
        {
            if (!_sender.UpdateAnExperiment(experiment.TrelloId, experiment.List!.TrelloId).IsCompletedSuccessfully)
                throw new UploadFailedException($"The process failed while uploading experiments. Failed at experiment: {experiment.Title}");

            var commentToAdd = experiment.Comments.Where(p => p.Body is null && p.Date == experiment.Comments?.Max(g => g.Date)).FirstOrDefault();
            if (commentsToAdd is not null && !_sender.AddAComment(experiment.TrelloId, commentToAdd.Body, commentToAdd.Scientist.TrelloId)
                throw new UploadFailedException($"The process failed while uploading the experiment: {experiment.Title}. Error while trying to upload its latest comment {experiment.Title}");
        }
        return true;
    }
}
