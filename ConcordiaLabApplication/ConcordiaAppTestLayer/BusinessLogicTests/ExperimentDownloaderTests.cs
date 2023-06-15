using AutoMapper;

using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DataTransferLogic.Concrete;
using BusinessLogic.DTOs.TrelloDtos;
using BusinessLogic.Exceptions;

using ConcordiaAppTestLayer.BusinessLogicTests.MockData;

using FluentAssertions;

using Moq;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace ConcordiaAppTestLayer.BusinessLogicTests;

public class ExperimentDownloaderTests
{
    private readonly IApiReceiver _apiReceiver;
    private readonly IExperimentRepository _experimentRepository;
    private readonly IScientistRepository _scientistRepository;
    private readonly IMapper _mapper;
    private readonly IExperimentDownloader _experimentDownloader;


    public ExperimentDownloaderTests()
    {
        _apiReceiver = Mock.Of<IApiReceiver>();
        _experimentRepository = Mock.Of<IExperimentRepository>();
        _scientistRepository = Mock.Of<IScientistRepository>();
        _mapper = Mock.Of<IMapper>();
        _experimentDownloader = new ExperimentDownloader(_apiReceiver, _experimentRepository, _scientistRepository, _mapper);
    }

    [Fact]
    public void DownloadShouldWorkWithExperiments()
    {
        Mock.Get(_apiReceiver).Setup(p => p.GetAllExperimentsInToDoColumn()).ReturnsAsync(DataSyncerMockData.trelloExperiments);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(DataSyncerMockData.trelloExperiment1.Id)).Returns(1);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(DataSyncerMockData.trelloExperiment2.Id)).Returns(2);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(DataSyncerMockData.trelloExperiment3.Id)).Returns(3);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(DataSyncerMockData.trelloExperiment4.Id)).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<TrelloExperimentDto, Experiment>(DataSyncerMockData.trelloExperiment4)).Returns(DataSyncerMockData.experiment4map);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId("alessandro")).Returns(2);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId("marco")).Returns(1);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId("Thobias")).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("ezez")).Returns(1);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("invalid")).Returns(-1);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("tough")).Returns(3);
        Mock.Get(_experimentRepository).Setup(p => p.Add(DataSyncerMockData.experimentAdded)).Returns(DataSyncerMockData.experiment4);

        _experimentDownloader.DownloadExperiments().Result.Should().Equal(DataSyncerMockData.ExpectedResult1.Item2);
    }

    [Fact]
    public void DownloadExperimentsShouldWorkSecondTest()
    {
        var mockData = new DataSyncerMockData2();
        Mock.Get(_apiReceiver).Setup(p => p.GetAllExperimentsInToDoColumn()).ReturnsAsync(mockData.ExperimentsInToDoList);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(mockData.TrelloExperiment1.Id)).Returns(1);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(mockData.TrelloExperiment2.Id)).Returns(2);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(mockData.TrelloExperiment4New.Id)).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(mockData.TrelloExperiment5New.Id)).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<TrelloExperimentDto, Experiment>(mockData.TrelloExperiment4New)).Returns(mockData.MappedExperiment4New);
        Mock.Get(_mapper).Setup(p => p.Map<TrelloExperimentDto, Experiment>(mockData.TrelloExperiment5New)).Returns(mockData.MappedExperiment5New);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId("alessandro")).Returns(1);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId("marco")).Returns(2);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("easy")).Returns(1);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("medium")).Returns(2);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("hard")).Returns(3);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("other")).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.Add(It.Is<Experiment>(p => p.TrelloId == "ddd"))).Returns(mockData.AddedExperiment4NewWithInfo);
        Mock.Get(_experimentRepository).Setup(p => p.Add(It.Is<Experiment>(p => p.TrelloId == "eee"))).Returns(mockData.AddedExperiment5NewWithInfo);

        var result = _experimentDownloader.DownloadExperiments().Result;
        result.Should().Equal(mockData.AddedExperiments);

    }

    [Fact]
    public void DownloadShouldWorkWithNoExperiments()
    {
        var mockData = new DataSyncerMockData2();
        Mock.Get(_apiReceiver).Setup(p => p.GetAllExperimentsInToDoColumn()).ReturnsAsync(value: null);

        var result = _experimentDownloader.DownloadExperiments().Result;
        result.Should().HaveCount(0);
    }

    [Fact]
    public void DownloadExperimentsThrowExceptionIfAssigneeDoesNotExist()
    {
        var mockData = new DataSyncerMockData2();
        Mock.Get(_apiReceiver).Setup(p => p.GetAllExperimentsInToDoColumn()).ReturnsAsync(new List<TrelloExperimentDto>() { mockData.TrelloExperiment4New });
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(mockData.TrelloExperiment4New.Id)).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<TrelloExperimentDto, Experiment>(mockData.TrelloExperiment4New)).Returns(mockData.MappedExperiment4New);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(It.IsAny<string>())).Returns(value: null);

        _experimentDownloader.Invoking(p => p.DownloadExperiments().Result).Should().Throw<ScientistIdNotPresentOnDatabaseException>().WithMessage("One or more of the assignees are not saved on the database, check Trello MemberId\n" +
                            $"Trello members Id: alessandro, marco");
    }
}
