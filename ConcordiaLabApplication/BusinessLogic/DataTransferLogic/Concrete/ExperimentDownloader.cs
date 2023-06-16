using AutoMapper;

using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.TrelloDtos;

using Microsoft.IdentityModel.Tokens;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

using System.Text;

namespace BusinessLogic.DataTransferLogic.Concrete;

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

    public async Task<(IEnumerable<Experiment>?, string)> DownloadExperiments()
    {

        (IEnumerable<Experiment>?, string) AddedExperiments = (new List<Experiment>(), string.Empty);

        var experimentsInToDoColumn = await _receiver.GetAllExperimentsInToDoColumn();

        if (!experimentsInToDoColumn.IsNullOrEmpty())
        {
            var resultOfSyncNewExperiments = SyncDatabaseWithAllExperimentsInToDoColumn(experimentsInToDoColumn!);
            AddedExperiments = resultOfSyncNewExperiments;
        }

        return AddedExperiments;
    }

    private (IEnumerable<Experiment>, string) SyncDatabaseWithAllExperimentsInToDoColumn(IEnumerable<TrelloExperimentDto> experiments)
    {
        StringBuilder infoMessage = new StringBuilder();
        List<Experiment> addedExperiments = new List<Experiment>();
        infoMessage.AppendLine($"Experiments found on To Do List : {experiments.Count()}");
        infoMessage.AppendLine("Downloading...");
        infoMessage.AppendLine("======================================");
        foreach (var experiment in experiments)
        {
            infoMessage.Append($"{$" - {experiment.Name}",-30}");
            if (_experimentRepository.GetLocalIdByTrelloId(experiment.Id!) is null)
            {
                var experimentToAdd = _mapper.Map<TrelloExperimentDto, Experiment>(experiment);
                experimentToAdd.ColumnId = 1;
                var scientistIdList = experiment.IdMembers?.Select(p => new { id = _scientistRepository.GetLocalIdByTrelloId(p) ?? -1, trelloId = p }).ToList();
                if (!scientistIdList.IsNullOrEmpty())
                {
                    if (scientistIdList!.Any(p => p.id == -1))
                    {
                        infoMessage.Append($" => One or more of the assignees are not saved on the database, check Trello Members' Ids:\n" +
                            $"Trello members Id: {string.Join(", \n", scientistIdList!.Where(g => g.id == -1).Select(p => $"{p.trelloId}"))}");
                        continue;

                    }
                    experimentToAdd.ScientistsIds = scientistIdList!.Select(p => p.id);
                    experimentToAdd.Scientists = scientistIdList!.Select(p => _scientistRepository.GetById(p.id)!).ToList() ?? new List<Scientist>();
                }
                if (!experiment.IdLabels.IsNullOrEmpty())
                {
                    var labelsId = experiment.IdLabels!.Select(p => _experimentRepository.GetLocalIdLabelByTrelloIdLabel(p) ?? -1).Max();
                    if (labelsId != -1)
                        experimentToAdd.LabelId = labelsId;
                }
                var addedExperiment = _experimentRepository.Add(experimentToAdd);
                addedExperiments.Add(addedExperiment);
                infoMessage.AppendLine(" => Experiment saved successfully.");
            }
            else
                infoMessage.AppendLine(" => Already saved in local Database.");
        }
        infoMessage.AppendLine("======================================");
        infoMessage.AppendLine("Download ended successfully");
        infoMessage.AppendLine($"{addedExperiments.Count()} Were added.");
        infoMessage.AppendLine("======================================");
        return (addedExperiments.AsEnumerable(), infoMessage.ToString());
    }
}
