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

    public async Task<SyncResult<Experiment>> DownloadExperiments()
    {
        SyncResult<Experiment> AddedExperiments = new SyncResult<Experiment>();
        var experimentsInToDoColumn = await _receiver.GetAllExperimentsInToDoColumn();
        if (!experimentsInToDoColumn.IsNullOrEmpty())
        {
            var resultOfSyncNewExperiments = SyncDatabaseWithAllExperimentsInToDoColumn(experimentsInToDoColumn!);
            AddedExperiments = resultOfSyncNewExperiments;
            return AddedExperiments;
        }
        AddedExperiments.AppendLine("No experiments found on Trello");
        return AddedExperiments;
    }

    private SyncResult<Experiment> SyncDatabaseWithAllExperimentsInToDoColumn(IEnumerable<TrelloExperimentDto> experiments)
    {
        int errorCount = 0;
        int updateCount = 0;
        int addedCount = 0;
        StringBuilder infoMessage = new StringBuilder();
        int? experimentFromTrelloId = null;
        List<Experiment> addedExperiments = new List<Experiment>();
        infoMessage.AppendLine($"Experiments found on To Do List : {experiments.Count()}");
        infoMessage.AppendLine("Downloading...");
        infoMessage.AppendLine("======================================");
        foreach (var experiment in experiments)
        {
            infoMessage.Append($"{$" - {experiment.Name}",-50}");
            experimentFromTrelloId = _experimentRepository.GetLocalIdByTrelloId(experiment.Id!);
            var scientistIdList = experiment.IdMembers?.Select(p => (_scientistRepository.GetLocalIdByTrelloId(p) ?? -1, p)).ToList();

            if (experimentFromTrelloId is null)
            {
                var result = SyncExperiments(scientistIdList, experiment, false, 0);
                infoMessage.AppendLine(result.message);
                if (result.isError)
                {
                    errorCount += result.isError ? 1 : 0;
                    continue;
                }
                addedCount++;
                addedExperiments.Add(result.experimentToAdd!);
            }
            else
            {
                var result = SyncExperiments(scientistIdList, experiment, true, (int)experimentFromTrelloId);
                infoMessage.AppendLine(result.message);
                if (result.isError)
                {
                    errorCount += result.isError ? 1 : 0;
                    continue;
                }
                updateCount++;
                addedExperiments.Add(result.experimentToAdd!);
            }
        }
        infoMessage.AppendLine("======================================");
        if (errorCount == 0)
        {
            infoMessage.AppendLine("Download ended successfully");
            infoMessage.AppendLine($"{addedCount} Were added.");
            infoMessage.AppendLine($"{updateCount} Were updated.");
        }
        else
        {
            infoMessage.AppendLine("Download Ended With Errors");
            infoMessage.AppendLine($"{updateCount} Were updated.");
            infoMessage.AppendLine($"{addedCount} Were added.");
            infoMessage.AppendLine($"{errorCount} Downloads failed due to errors...");
        }
        infoMessage.AppendLine("======================================");
        return new SyncResult<Experiment>(addedExperiments, infoMessage, errorCount);
    }


    private (Experiment? experimentToAdd, string message, bool isError) SyncExperiments(IEnumerable<(int id, string trelloId)>? scientistInfo, TrelloExperimentDto experiment, bool IsUpdate, int experimentId)
    {
        string message = "";
        Experiment? addedExperiment = null!;
        var experimentToAdd = _mapper.Map<TrelloExperimentDto, Experiment>(experiment);
        (IEnumerable<int>? ids, IEnumerable<Scientist>? scientists, string message, bool isError) scientistsInfo = new(new List<int> { }, new List<Scientist> { }, "", false);
        if (!scientistInfo.IsNullOrEmpty())
        {
            scientistsInfo = GetScientistsForThisExperiment(scientistInfo!);
            if (scientistsInfo.isError)
                return new(null, scientistsInfo.message, true);
        }
        if (!experiment.IdLabels.IsNullOrEmpty())
        {
            experimentToAdd.LabelId = GetLabel(experiment);

        }
        if (!IsUpdate)
        {
            experimentToAdd.ColumnId = 1;
            experimentToAdd.ScientistsIds = scientistsInfo.ids;
            experimentToAdd.Scientists = scientistsInfo.scientists;
            addedExperiment = _experimentRepository.Add(experimentToAdd);
            message = " => Experiment saved successfully.";
        }
        else
        {
            addedExperiment = new Experiment(experimentId)
            {
                DeadLine = experimentToAdd.DeadLine,
                LabelId = experimentToAdd.LabelId,
                ScientistsIds = scientistsInfo.ids,
                Scientists = scientistsInfo.scientists
            };
            addedExperiment =
                _experimentRepository.Update(addedExperiment);
            message = " => Experiment updated successfully.";
        }
        return new(addedExperiment, addedExperiment == null ? "Failed" : message, addedExperiment == null ? true : false);

    }

    private (IEnumerable<int> ids, IEnumerable<Scientist> scientists, string message, bool error) GetScientistsForThisExperiment(IEnumerable<(int id, string trelloId)> scientistsInfo)
    {
        string message = "";
        bool error = false;
        if (scientistsInfo.Any(p => p.id == -1))
        {
            message = $" => One or more of the assignees are not saved on the database, check Trello Members' Ids:\n" +
                $"Trello members Id: {string.Join(", \n", scientistsInfo!.Where(g => g.id == -1).Select(p => $"{p.trelloId}"))}";
            error = true;

        }
        return new(
            scientistsInfo.Select(p => p.id),
            scientistsInfo.Select(p => _scientistRepository.GetById(p.id)!).ToList() ?? new List<Scientist>(),
            message,
            error
            );
    }

    private int? GetLabel(TrelloExperimentDto experiment)
    {
        var labelsId = experiment.IdLabels!.Select(p => _experimentRepository.GetLocalIdLabelByTrelloIdLabel(p) ?? -1).Max();
        if (labelsId != -1)
            return labelsId;
        return null;
    }
}
