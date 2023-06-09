using AutoMapper;

using BusinessLogic.DataTransferLogic.Concrete;
using BusinessLogic.DTOs.BusinessDTO;
using BusinessLogic.Exceptions;

using ConcordiaAppTestLayer.MockData;

using FluentAssertions;

using Moq;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace ConcordiaAppTestLayer;

public class DataServiceTests
{

    private readonly ICommentRepository _commentRepository;
    private readonly IListRepository _listRepository;
    private readonly IExperimentRepository _experimentRepository;
    private readonly IMapper _mapper;
    private readonly DataService _sut;

    public DataServiceTests()
    {
        _commentRepository = Mock.Of<ICommentRepository>();
        _experimentRepository = Mock.Of<IExperimentRepository>();
        _listRepository = Mock.Of<IListRepository>();
        _mapper = Mock.Of<IMapper>();
        _sut = new DataService(_listRepository, _commentRepository, _experimentRepository, _mapper);

    }

    [Fact]
    public void AddACommentShouldWork()
    {
        Comment comment1 = new Comment(0, "aaa", "sono un commento", DateTime.Now, "Giovanni");
        comment1.ScientistId = 2;
        BusinessCommentDto bcomment1 = new BusinessCommentDto()
        {
            CardID = 1,
            CommentText = "sono un commento",
            TrelloCardId = "aaa",
            Scientist = new BusinessScientistDto()
            {
                Id = 2
            }
        };
        Mock.Get(_mapper).Setup(p => p.Map<Comment>(bcomment1)).Returns(comment1);
        Mock.Get(_commentRepository).Setup(p => p.AddComment(comment1)).Returns(0);
        _sut.AddComment(bcomment1, 2).Should().Be(bcomment1);
    }

    [Fact]
    public void AddACommentShouldThrowException()
    {
        Comment comment1 = new Comment(0, "aaa", "sono un commento", DateTime.Now, "Thomas");
        comment1.ScientistId = 44;
        BusinessCommentDto bcomment1 = new BusinessCommentDto()
        {
            CardID = 1,
            CommentText = "sono un commento",
            TrelloCardId = "aaa",
            Scientist = new BusinessScientistDto()
            {
                Id = 44
            }
        };
        Mock.Get(_mapper).Setup(p => p.Map<Comment>(bcomment1)).Returns(comment1);
        Mock.Get(_commentRepository).Setup(p => p.AddComment(comment1)).Returns(value: null);
        _sut.Invoking(p => p.AddComment(bcomment1, 44)).Should().Throw<AddACommentFailedException>().WithMessage("Failed to add comment: \"sono un commento\" by scientist with Id: 44 ");
    }

    [Fact]
    public void GetAllListShouldGiveAllLists()
    {
        Mock.Get(_listRepository).Setup(p => p.GetAll()).Returns(DataServiceMockData.allList);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessListDto>?>(DataServiceMockData.allList)).Returns(DataServiceMockData.allbList);

        _sut.GetAllLists().Should().Equal(DataServiceMockData.allbList, (p, g) => p.Id == g.Id);
        _sut.GetAllLists().Should().HaveCount(c => c == 1);
        _sut.GetAllLists().FirstOrDefault()!.Experiments.Should().HaveCount(p => p == 2);
    }

    [Fact]
    public void GetAllListShouldGiveAllListsByScientistId()
    {
        Mock.Get(_listRepository).Setup(p => p.GetByScientistId(0)).Returns(DataServiceMockData.allListById);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessListDto>?>(_listRepository.GetByScientistId(0))).Returns(DataServiceMockData.allbListById);

        _sut.GetAllLists(0).Should().Equal(DataServiceMockData.allbListById, (p, g) => p.Id == g.Id);
        _sut.GetAllLists(0).Should().HaveCount(c => c == 1);
        _sut.GetAllLists(0).FirstOrDefault()!.Experiments.Should().HaveCount(p => p == 1);
    }

    [Fact]
    public void GetAllListShouldThrowException()
    {
        Mock.Get(_listRepository).Setup(p => p.GetByScientistId(1)).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessListDto>?>(_listRepository.GetByScientistId(1))).Returns(value: null);

        _sut.Invoking(p => p.GetAllLists(2)).Should().Throw<AllListsEmptyException>().WithMessage("The database has no lists.");
    }

    [Fact]
    public void MoveExperimentShouldWork()
    {
        Mock.Get(_experimentRepository).Setup(p => p.Move(0, 2)).Returns(DataServiceMockData.experiment1);
        Mock.Get(_mapper).Setup(p => p.Map<BusinessExperimentDto>(_experimentRepository.Move(0, 2))).Returns(DataServiceMockData.businessExperimentDto1);

        _sut.MoveExperiment(DataServiceMockData.businessExperimentDto1).Id.Should().Be(DataServiceMockData.businessExperimentDto1.Id);


    }

    [Fact]
    public void MoveExperimentShouldThrowFailedToMoveExperimentExceptionWithNoInfo()
    {
        Mock.Get(_experimentRepository).Setup(p => p.Move(DataServiceMockData.businessExperimentDto3.Id, DataServiceMockData.businessExperimentDto3.ListId)).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.GetById(DataServiceMockData.businessExperimentDto3.Id)).Returns(DataServiceMockData.experiment1);
        Mock.Get(_mapper).Setup(p => p.Map<BusinessExperimentDto?>(_experimentRepository.Move(0, 2))).Returns(value: null);
        _sut.Invoking(p => p.MoveExperiment(DataServiceMockData.businessExperimentDto3)).Should().Throw<FailedToMoveExperimentException>().WithMessage($"Could not move the experiment to the list with ID: {DataServiceMockData.businessExperimentDto3.ListId}");
    }

    [Fact]
    public void MoveExperimentShouldThrowFailedToMoveExperimentExceptionWithAdditionalInfo()
    {
        Mock.Get(_experimentRepository).Setup(p => p.Move(DataServiceMockData.businessExperimentDto3.Id, DataServiceMockData.businessExperimentDto3.ListId)).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<BusinessExperimentDto?>(_experimentRepository.Move(0, 2))).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.GetById(DataServiceMockData.businessExperimentDto3.Id)).Returns(value: null);
        _sut.Invoking(p => p.MoveExperiment(DataServiceMockData.businessExperimentDto3)).Should().Throw<FailedToMoveExperimentException>().WithMessage($"Could not move the experiment to the list with ID: {DataServiceMockData.businessExperimentDto3.ListId}\nNo experiment with corresponding Id found in the database");
    }
}
