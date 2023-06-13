using AutoMapper;

using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.TrelloDtos;
using BusinessLogic.Exceptions;

using Microsoft.IdentityModel.Tokens;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace BusinessLogic.DataTransferLogic.Concrete;

/// <summary>
/// riceve e salva in nuovi esperimenti in locale
/// </summary>
public class ExperimentDownloader : IExperimentDownloader
{
    private readonly IApiReceiver _receiver;
    private readonly IExperimentRepository _experimentRepository;
    private readonly IScientistRepository _scientistRepository;
    private readonly IMapper _mapper;

    public ExperimentDownloader(IApiReceiver receiver, IExperimentRepository experimentRepository, IScientistRepository scientistRepository, IMapper mapper)
    {
        _receiver = receiver;
        _experimentRepository = experimentRepository;
        _scientistRepository = scientistRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Experiment>?> DownloadExperiments()
    {

        IEnumerable<Experiment>? AddedExperiments = null;

        var experimentsInToDoList = await _receiver.GetAllExperimentsInToDoList();

        if (!experimentsInToDoList.IsNullOrEmpty())
        {
            var resultOfSyncNewExperiments = SyncDatabaseWithAllExperimentsInToDoList(experimentsInToDoList!);
            AddedExperiments = resultOfSyncNewExperiments.Item2;
        }

        return AddedExperiments;
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
                var scientistIdList = experiment.IdMembers?.Select(p => _scientistRepository.GetLocalIdByTrelloId(p) ?? -1).ToList();

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

                addedExperiments.Add(addedExperiment);
                count++;
            }
        }
        return (count, addedExperiments.AsEnumerable());
    }
}
