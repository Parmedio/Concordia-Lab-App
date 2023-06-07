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

    public async Task<(IEnumerable<int>?, IEnumerable<Experiment>?)> Download()
    {
        IEnumerable<int>? addedCommentsId = null;
        IEnumerable<Experiment>? AddedExperiments = null;
        _logger.LogInformation("Download from Trello started...");
        var experimentsInToDoList = await _receiver.GetAllExperimentsInToDoList();

        if (!experimentsInToDoList.IsNullOrEmpty())
        {

            var resultOfSyncNewExperiments = SyncDatabaseWithAllExperimentsInToDoList(experimentsInToDoList!);

            AddedExperiments = resultOfSyncNewExperiments.Item2;
            _logger.LogInformation($"Added {resultOfSyncNewExperiments.Item1} new experiments. ");
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
            addedCommentsId = SyncDatabaseWithAllLatestComments(latestCommentsToAdd);
        }
        else
        {
            _logger.LogInformation("No comments found on the Trello board.");
        }
        _logger.LogInformation("Download completed with no errors.");
        return (addedCommentsId, AddedExperiments);
    }

    public async Task<bool> Upload()
    {
        _logger.LogInformation("Upload to Trello started...");
        var experiments = _experimentRepository.GetAll();
        await SyncTrelloWithAllUpdates(experiments);

        _logger.LogInformation("Uploaded completed with no errors.");
        return true;
    }

    private List<int> SyncDatabaseWithAllLatestComments(IEnumerable<TrelloCommentDto> comments)
    {
        List<int> addedCommentIds = new List<int>();
        foreach (var comment in comments)
        {
            if (_commentRepository.GetCommentByTrelloId(comment.Id!) is null)
            {
                Comment commentToAdd = _mapper.Map<Comment>(comment);
                commentToAdd.ScientistId = _scientistRepository.GetLocalIdByTrelloId(comment.IdMemberCreator);
                commentToAdd.ExperimentId = _experimentRepository.GetLocalIdByTrelloId(comment.Data.Card.Id) ?? throw new NullReferenceException("The Experiment is not saved in the local database. Try Again.");

                int? newId = _commentRepository.AddComment(commentToAdd);
                if (newId.HasValue)
                    addedCommentIds.Add(newId ?? -1);
                else
                    throw new AddACommentFailedException("Failed To Add comment to Database during the Download Operation from Trello");
            }
        }
        return addedCommentIds;
    }

    private (int, IEnumerable<Experiment>) SyncDatabaseWithAllExperimentsInToDoList(IEnumerable<TrelloExperimentDto> experiments)
    {
        int count = 0;
        List<Experiment> addedExperiments = new List<Experiment>();
        foreach (var experiment in experiments)
        {
            if (_experimentRepository.GetLocalIdByTrelloId(experiment.Id!) is null)
            {

                var experimentToAdd = _mapper.Map<Experiment>(experiment);
                experimentToAdd.ListId = 1;
                var scientistIdList = experiment.IdMembers?.Select(p => _scientistRepository.GetLocalIdByTrelloId(p) ?? -1);
                if (!scientistIdList.IsNullOrEmpty())
                {
                    if (scientistIdList!.Any(p => p == -1))
                        throw new ScientistIdNotPresentOnDatabaseException("One of the assignee is not saved on the database, check Trello MemberId");
                    experimentToAdd.ScientistsIds = scientistIdList;
                }

                if (!experiment.IdLabels.IsNullOrEmpty())
                {
                    var labelsId = experiment.IdLabels!.Select(p => _experimentRepository.GetLocalIdLabelByTrelloIdLabel(p) ?? -1).Max();
                    if (labelsId != -1)
                        experimentToAdd.LabelId = labelsId;
                }

                var addedExperiment = _experimentRepository.Add(experimentToAdd);
                if (experimentToAdd is null)
                    throw new NullReferenceException();
                addedExperiments.Add(addedExperiment);
                count++;
            }
        }
        return (count, addedExperiments.AsEnumerable());
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
