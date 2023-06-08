using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.Exceptions;

using Microsoft.IdentityModel.Tokens;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class Uploader : IUploader
{
    private readonly IApiSender _sender;
    private readonly IExperimentRepository _experimentRepository;

    public Uploader(IApiSender sender, IExperimentRepository experimentRepository)
    {
        _sender = sender;
        _experimentRepository = experimentRepository;
    }

    public async Task Upload()
    {
        var experiments = _experimentRepository.GetAll();
        await SyncTrelloWithAllUpdates(experiments);
    }

    private async Task SyncTrelloWithAllUpdates(IEnumerable<Experiment> experiments)
    {
        foreach (var experiment in experiments)
        {
            if (!await _sender.UpdateAnExperiment(experiment.TrelloId, experiment.List!.TrelloId))
                throw new UploadFailedException($"The process failed while uploading experiments. Failed at experiment: {experiment.Title}");

            Comment? commentToAdd = null;
            if (!experiment.Comments.IsNullOrEmpty())
            {
                commentToAdd = experiment.Comments!.Where(p => p.TrelloId is null && p.Date == experiment.Comments!.Max(g => g.Date)).FirstOrDefault();
                if (!await _sender.AddAComment(experiment.TrelloId, commentToAdd!.Body, commentToAdd.Scientist!.TrelloToken))
                    throw new UploadFailedException($"The process failed while uploading the experiment: {experiment.Title}. Error while trying to upload its latest comment: {commentToAdd.Body}");
            }

        }
    }
}
