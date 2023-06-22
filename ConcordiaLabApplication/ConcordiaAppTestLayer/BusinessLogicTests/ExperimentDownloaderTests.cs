using AutoMapper;

using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DataTransferLogic.Concrete;
using BusinessLogic.DTOs.TrelloDtos;

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
        Mock.Get(_mapper).Setup(p => p.Map<TrelloExperimentDto, Experiment>(mockData.TrelloExperiment1)).Returns(mockData.MappedExperiment1Old);
        Mock.Get(_mapper).Setup(p => p.Map<TrelloExperimentDto, Experiment>(mockData.TrelloExperiment2)).Returns(mockData.MappedExperiment2Old);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId("alessandro")).Returns(1);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId("marco")).Returns(2);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("easy")).Returns(1);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("medium")).Returns(2);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("hard")).Returns(3);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("other")).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.Add(It.Is<Experiment>(p => p.TrelloId == "ddd"))).Returns(mockData.AddedExperiment4NewWithInfo);
        Mock.Get(_experimentRepository).Setup(p => p.Add(It.Is<Experiment>(p => p.TrelloId == "eee"))).Returns(mockData.AddedExperiment5NewWithInfo);
        Mock.Get(_experimentRepository).Setup(p => p.Update(It.Is<Experiment>(p => p.Id == 1))).Returns(mockData.LocalExperiment1InToDo);
        Mock.Get(_experimentRepository).Setup(p => p.Update(It.Is<Experiment>(p => p.Id == 2))).Returns(mockData.LocalExperiment2InToDo);

        var result = _experimentDownloader.DownloadExperiments().Result;
        result.Items.Should().HaveCount(4);

    }

    [Fact]
    public void DownloadShouldWorkWithNoExperiments()
    {
        var mockData = new DataSyncerMockData2();
        Mock.Get(_apiReceiver).Setup(p => p.GetAllExperimentsInToDoColumn()).ReturnsAsync(value: null);

        var result = _experimentDownloader.DownloadExperiments().Result;
        result.Items.Should().HaveCount(0);
    }


}
