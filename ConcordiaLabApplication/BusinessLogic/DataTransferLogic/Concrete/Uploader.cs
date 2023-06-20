using BusinessLogic.DataTransferLogic.Abstract;

using Microsoft.IdentityModel.Tokens;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class Uploader : IUploader
{
    private readonly IApiSender _sender;
    private readonly IExperimentRepository _experimentRepository;
    private readonly ICommentRepository _commentRepository;

    public Uploader(IApiSender sender, IExperimentRepository experimentRepository, ICommentRepository commentRepository)
    {
        _sender = sender;
        _experimentRepository = experimentRepository;
        _commentRepository = commentRepository;
    }

    public async Task<SyncResult<Experiment>> Upload()
    {
        SyncResult<Experiment> result = new SyncResult<Experiment>();
        var experiments = _experimentRepository.GetAll().ToList();
        result = await SyncTrelloWithAllUpdates(experiments);
        return result;
    }

    private async Task<SyncResult<Experiment>> SyncTrelloWithAllUpdates(IEnumerable<Experiment> experiments)
    {
        int addedCommentsCount = 0;
        int addedExperimentsCount = 0;
        SyncResult<Experiment> result = new SyncResult<Experiment>();
        result.AppendLine($"Found {experiments.Count()} experiment to upload.");
        result.AppendLine("======================================");
        foreach (var experiment in experiments)
        {
            result.Append($"{$" - {experiment.Title}",-50}");
            if (!await _sender.UpdateAnExperiment(experiment.TrelloId, experiment.Column!.TrelloId))
            {
                result.AppendLine($" => The process failed while uploading experiments. Failed at experiment: {experiment.Title}");
                continue;
            }
            addedExperimentsCount++;
            Comment? commentToAdd = null;
            if (!experiment.Comments.IsNullOrEmpty())
            {
                result.AppendLine(" => Upload successful");
                commentToAdd = _experimentRepository.GetLastLocalCommentNotOnTrello(experiment.Id);

                if (commentToAdd is null)
                    continue;

                result.Append($"{$"        - \"{string.Concat(commentToAdd.Body.Take(15))}...\"",-50}");
                try
                {
                    commentToAdd.TrelloId = await _sender.AddAComment(experiment.TrelloId, commentToAdd!.Body, commentToAdd.Scientist!.TrelloToken);
                    _commentRepository.UpdateAComment(commentToAdd.Id, commentToAdd.TrelloId);
                    result.AppendLine(" => Upload successful");
                    addedCommentsCount++;
                }
                catch (Exception ex)
                {
                    result.AppendLine($" => Upload Failed: {ex.Message}");
                }
            }
            else
                result.AppendLine(" => Upload successful");
        }
        result.AppendLine("======================================");
        result.AppendLine("Uploaded ended successfully");
        result.AppendLine($"{addedExperimentsCount} Experiments were updated.");
        result.AppendLine($"{addedCommentsCount} Comments were added.");
        result.AppendLine("======================================");
        return result;
    }
}
