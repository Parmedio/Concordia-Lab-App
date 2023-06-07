using AutoMapper;

using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DataTransferLogic.Concrete;

using ConcordiaAppTestLayer.MockData;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace ConcordiaAppTestLayer;

public class DataSyncerTests
{
    private readonly IApiSender _apiSender;
    private readonly IApiReceiver _apiReceiver;
    private readonly ILogger<DataSyncer> _logger;
    private readonly IExperimentRepository _experimentRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;
    private readonly IScientistRepository _scientistRepository;


    private readonly IDataSyncer _dataSyncer;
    public DataSyncerTests()
    {
        _apiReceiver = Mock.Of<IApiReceiver>();
        _apiSender = Mock.Of<IApiSender>();
        _logger = Mock.Of<ILogger<DataSyncer>>();
        _experimentRepository = Mock.Of<IExperimentRepository>();
        _commentRepository = Mock.Of<ICommentRepository>();
        _mapper = Mock.Of<IMapper>();
        _scientistRepository = Mock.Of<IScientistRepository>();
        _dataSyncer = new DataSyncer(_apiSender, _apiReceiver, _logger, _experimentRepository, _commentRepository, _mapper, _scientistRepository);
    }

    [Fact]
    public void DownloadShouldWorkWithExperiments()
    {
        Mock.Get(_apiReceiver).Setup(p => p.GetAllExperimentsInToDoList()).ReturnsAsync(DataSyncerMockData.trelloExperiments);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(DataSyncerMockData.trelloExperiment1.Id)).Returns(1);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(DataSyncerMockData.trelloExperiment2.Id)).Returns(2);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(DataSyncerMockData.trelloExperiment3.Id)).Returns(3);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(DataSyncerMockData.trelloExperiment4.Id)).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<Experiment>(DataSyncerMockData.trelloExperiment4)).Returns(DataSyncerMockData.experiment4map);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId("alessandro")).Returns(2);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId("marco")).Returns(1);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId("Thobias")).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("ezez")).Returns(1);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("invalid")).Returns(-1);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdLabelByTrelloIdLabel("tough")).Returns(3);
        Mock.Get(_experimentRepository).Setup(p => p.Add(DataSyncerMockData.experimentAdded)).Returns(DataSyncerMockData.experiment4);

        Mock.Get(_apiReceiver).Setup(p => p.GetAllComments()).ReturnsAsync(DataSyncerMockData.allCommentsOnTrello);
        Mock.Get(_commentRepository).Setup(p => p.GetCommentByTrelloId("aaa")).Returns(value: null);
        Mock.Get(_commentRepository).Setup(p => p.GetCommentByTrelloId("bbb")).Returns(value: DataSyncerMockData.comment1);
        Mock.Get(_commentRepository).Setup(p => p.GetCommentByTrelloId("ccc")).Returns(value: null);
        Mock.Get(_commentRepository).Setup(p => p.GetCommentByTrelloId("ddd")).Returns(value: DataSyncerMockData.comment2);
        Mock.Get(_commentRepository).Setup(p => p.GetCommentByTrelloId("eee")).Returns(value: DataSyncerMockData.comment3);

        Mock.Get(_mapper).Setup(p => p.Map<Comment>(DataSyncerMockData.trelloComment1)).Returns(DataSyncerMockData.comment1map);
        Mock.Get(_mapper).Setup(p => p.Map<Comment>(DataSyncerMockData.trelloComment3)).Returns(DataSyncerMockData.comment2map);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(DataSyncerMockData.trelloComment1.Data.Card.Id)).Returns(1);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(DataSyncerMockData.trelloComment3.Data.Card.Id)).Returns(3);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId(DataSyncerMockData.trelloComment1.IdMemberCreator)).Returns(value: null);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId(DataSyncerMockData.trelloComment3.IdMemberCreator)).Returns(value: null);
        Mock.Get(_commentRepository).Setup(p => p.AddComment(DataSyncerMockData.comment1map)).Returns(4);
        Mock.Get(_commentRepository).Setup(p => p.AddComment(DataSyncerMockData.comment2map)).Returns(5);

        _dataSyncer.Download().Result.Item1.Should().Equal(DataSyncerMockData.ExpectedResult1.Item1);
        _dataSyncer.Download().Result.Item2.Should().Equal(DataSyncerMockData.ExpectedResult1.Item2);



    }

    [Fact]
    public void DownloadShouldWorkWithNoExperiments()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void DownloadShouldThrowExpection()
    {
        throw new NotImplementedException();
    }

}
