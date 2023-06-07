using AutoMapper;

using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.TrelloDtos;
using BusinessLogic.Exceptions;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class DataSyncer : IDataSyncer
{
    private readonly IApiSender _sender;
    private readonly IApiReceiver _receiver;
    private readonly ILogger<DataSyncer> _logger;
    private readonly IScientistRepository _scientistRepository;
    private readonly IExperimentRepository _experimentRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public DataSyncer(IApiSender sender, IApiReceiver receiver, ILogger<DataSyncer> logger, IExperimentRepository experimentRepository, ICommentRepository commentRepository, IMapper mapper, IScientistRepository repository)
    {
        _sender = sender;
        _receiver = receiver;
        _logger = logger;
        _experimentRepository = experimentRepository;
        _commentRepository = commentRepository;
        _mapper = mapper;
        _scientistRepository = repository;
    }

    public async Task<bool> Download()
    {
        _logger.LogInformation("Download from Trello started...");
        var experimentsInToDoList = await _receiver.GetAllExperimentsInToDoList();

        if (!experimentsInToDoList.IsNullOrEmpty())
        {

            var countOfNewExperiments = SyncDatabaseWithAllExperimentsInToDoList(experimentsInToDoList!);
            _logger.LogInformation($"Added {countOfNewExperiments} new experiments. ");
        }
        else
        {
            _logger.LogInformation("No experiment found on the To Do List on Trello");
        }



        var comments = await _receiver.GetAllComments();
        if (!comments.IsNullOrEmpty())
        {
            _logger.LogInformation($"Downloading {comments!.Count()} comments...");
            var latestCommentsToAdd = comments!.GroupBy(p => p.Data.Card.Id).Select(g => g.OrderByDescending(p => p.Date).First());
            SyncDatabaseWithAllLatestComments(latestCommentsToAdd);
        }
        else
        {
            _logger.LogInformation("No comments found on the Trello board.");
        }
        _logger.LogInformation("Download completed with no errors.");
        return true;
    }

    public async Task<bool> Upload()
    {
        _logger.LogInformation("Upload to Trello started...");
        var experiments = _experimentRepository.GetAll();
        await SyncTrelloWithAllUpdates(experiments);

        _logger.LogInformation("Uploaded completed with no errors.");
        return true;
    }

    private void SyncDatabaseWithAllLatestComments(IEnumerable<TrelloCommentDto> comments)
    {
        foreach (var comment in comments)
        {
            if (_commentRepository.GetCommentByTrelloId(comment.Id!) is null)
            {
                Comment commentToAdd = _mapper.Map<Comment>(comment);
                commentToAdd.ScientistId = _scientistRepository.GetLocalIdByTrelloId(comment.IdMemberCreator);
                commentToAdd.ExperimentId = _experimentRepository.GetLocalIdByTrelloId(comment.Data.Card.Id) ?? throw new NullReferenceException("The Experiment is not saved in the local database. Try Again.");

                _commentRepository.AddComment(commentToAdd);
            }
        }
    }

    private int SyncDatabaseWithAllExperimentsInToDoList(IEnumerable<TrelloExperimentDto> experiments)
    {
        int count = 0;
        foreach (var experiment in experiments)
        {
            if (_experimentRepository.GetLocalIdByTrelloId(experiment.Id!) is null)
            {
                _experimentRepository.Add(_mapper.Map<Experiment>(experiment));
                count++;
            }
        }
        return count;
    }


    private async Task<bool> SyncTrelloWithAllUpdates(IEnumerable<Experiment> experiments)
    {


        foreach (var experiment in experiments)
        {
            if (!_sender.UpdateAnExperiment(experiment.TrelloId, experiment.List!.TrelloId).IsCompletedSuccessfully)
                throw new UploadFailedException($"The process failed while uploading experiments. Failed at experiment: {experiment.Title}");

            var commentToAdd = experiment.Comments!.Where(p => p.TrelloId is null && p.Date == experiment.Comments?.Max(g => g.Date)).FirstOrDefault();
            if (commentToAdd is not null && !await _sender.AddAComment(experiment.TrelloId, commentToAdd.Body, commentToAdd.Scientist!.TrelloToken))
                throw new UploadFailedException($"The process failed while uploading the experiment: {experiment.Title}. Error while trying to upload its latest comment {experiment.Title}");
        }
        return true;
    }
}
