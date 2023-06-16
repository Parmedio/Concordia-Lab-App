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

public class CommentDownloaderTests
{
    private readonly ICommentDownloader _commentDownloader;
    private readonly IApiReceiver _apiReceiver;
    private readonly IExperimentRepository _experimentRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IScientistRepository _scientistRepository;
    private readonly IMapper _mapper;

    public CommentDownloaderTests()
    {
        _apiReceiver = Mock.Of<IApiReceiver>();
        _experimentRepository = Mock.Of<IExperimentRepository>();
        _commentRepository = Mock.Of<ICommentRepository>();
        _scientistRepository = Mock.Of<IScientistRepository>();
        _mapper = Mock.Of<IMapper>();
        _commentDownloader = new CommentDownloader(_apiReceiver, _experimentRepository, _scientistRepository, _mapper, _commentRepository);
    }

    [Fact]
    public void DownloadShouldWorkWithComments()
    {

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
        Mock.Get(_commentRepository).Setup(p => p.AddComment(DataSyncerMockData.comment1map)).Returns(DataSyncerMockData.comment1);
        Mock.Get(_commentRepository).Setup(p => p.AddComment(DataSyncerMockData.comment2map)).Returns(DataSyncerMockData.comment2);


        _commentDownloader.DownloadComments().Result.Should().Equal(DataSyncerMockData.ExpectedResult1.Item1);

    }

    [Fact]
    public void DownloadCommentsShouldWorkSecondTest()
    {
        var mockData = new DataSyncerMockData2();
        Mock.Get(_apiReceiver).Setup(p => p.GetAllComments()).ReturnsAsync(mockData.AllCommentsOnTrello);
        Mock.Get(_commentRepository).Setup(p => p.GetCommentByTrelloId("aaa")).Returns(mockData.LocalComment1OnCard1);
        Mock.Get(_commentRepository).Setup(p => p.GetCommentByTrelloId("bbb")).Returns(mockData.LocalComment1OnCard2);
        Mock.Get(_commentRepository).Setup(p => p.GetCommentByTrelloId("ccc")).Returns(mockData.LocalComment1OnCard3);
        Mock.Get(_commentRepository).Setup(p => p.GetCommentByTrelloId("ddd")).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<Comment>(mockData.TrelloComment1OnCard5New)).Returns(mockData.MappedComment1OnCard5New);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId(mockData.TrelloComment1OnCard5New.IdMemberCreator)).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(mockData.TrelloComment1OnCard5New.Data.Card.Id)).Returns(5);
        Mock.Get(_commentRepository).Setup(p => p.AddComment(mockData.ToBeAddedComment1OnCard5NewWithInfo)).Returns(mockData.AddedComment1OnCard5NewWithInfo);

        var result = _commentDownloader.DownloadComments().Result;
        result!.Should().Equal(mockData.NewCommentIds);

    }

    [Fact]
    public void DownloadShouldWorkWithNoComments()
    {
        var mockData = new DataSyncerMockData2();
        Mock.Get(_apiReceiver).Setup(p => p.GetAllComments()).ReturnsAsync(new List<TrelloCommentDto>());

        var result = _commentDownloader.DownloadComments().Result;
        result.Should().Equal(null);
    }

    [Fact]
    public void DownloadCommentsShouldThrowExceptionWhenExpermentIdDoesNotExist()
    {
        var mockData = new DataSyncerMockData2();
        Mock.Get(_apiReceiver).Setup(p => p.GetAllComments()).ReturnsAsync(new List<TrelloCommentDto>() { mockData.TrelloComment1OnCard5New });
        Mock.Get(_commentRepository).Setup(p => p.GetCommentByTrelloId("ddd")).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<Comment>(mockData.TrelloComment1OnCard5New)).Returns(mockData.MappedComment1OnCard5New);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId(mockData.TrelloComment1OnCard5New.IdMemberCreator)).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(mockData.TrelloComment1OnCard5New.Data.Card.Id)).Returns(value: null);

        _commentDownloader.Invoking(p => p.DownloadComments().Result).Should().Throw<ExperimentNotPresentInLocalDatabaseException>().WithMessage("The Experiment with associated Trello ID: eee is not saved in the local database. Try Again.");

    }

    [Fact]
    public void DownloadCommentsShouldThrowExceptionWhenAddMethodFails()
    {
        var mockData = new DataSyncerMockData2();
        Mock.Get(_apiReceiver).Setup(p => p.GetAllComments()).ReturnsAsync(new List<TrelloCommentDto>() { mockData.TrelloComment1OnCard5New });
        Mock.Get(_commentRepository).Setup(p => p.GetCommentByTrelloId("ddd")).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<Comment>(mockData.TrelloComment1OnCard5New)).Returns(mockData.MappedComment1OnCard5New);
        Mock.Get(_scientistRepository).Setup(p => p.GetLocalIdByTrelloId(mockData.TrelloComment1OnCard5New.IdMemberCreator)).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.GetLocalIdByTrelloId(mockData.TrelloComment1OnCard5New.Data.Card.Id)).Returns(5);
        Mock.Get(_commentRepository).Setup(p => p.AddComment(mockData.ToBeAddedComment1OnCard5NewWithInfo)).Returns(value: null);

        _commentDownloader.Invoking(p => p.DownloadComments().Result).Should().Throw<AddACommentFailedException>().WithMessage($"Failed To Add comment with text: primoCommentoVecchio and Id: ddd to the Database during the Download Operation from Trello");

    }

}
