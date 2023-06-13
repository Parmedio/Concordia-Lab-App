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
    private readonly IScientistRepository _scientistRepository;
    private readonly IExperimentRepository _experimentRepository;
    private readonly IMapper _mapper;
    private readonly DataService _sut;

    public DataServiceTests()
    {
        _commentRepository = Mock.Of<ICommentRepository>();
        _experimentRepository = Mock.Of<IExperimentRepository>();
        _listRepository = Mock.Of<IListRepository>();
        _scientistRepository = Mock.Of<IScientistRepository>();
        _mapper = Mock.Of<IMapper>();
        _sut = new DataService(_listRepository, _commentRepository, _experimentRepository, _mapper, _scientistRepository);
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
        Mock.Get(_listRepository).Setup(p => p.GetAll()).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessListDto>?>(_listRepository.GetAll())).Returns(value: null);

        _sut.Invoking(p => p.GetAllLists(2)).Should().Throw<AllListsEmptyException>().WithMessage("The database has no lists.");
    }

    [Fact]
    public void GetAllListShouldThrowException_ById()
    {
        Mock.Get(_listRepository).Setup(p => p.GetByScientistId(1)).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessListDto>?>(_listRepository.GetByScientistId(1))).Returns(value: null);

        _sut.Invoking(p => p.GetAllLists(2)).Should().Throw<AllListsEmptyException>().WithMessage("The database has no lists.");
    }

    [Fact]
    public void GetAllExperiments_ShouldReturn2Experiments()
    {
        Mock.Get(_experimentRepository).Setup(p => p.GetAll()).Returns(DataServiceMockData.experiments);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessExperimentDto>>(_experimentRepository.GetAll())).Returns(DataServiceMockData.businessListAll.Experiments!);

        _sut.GetAllExperiments().Should().HaveCount(2);
    }

    public static IEnumerable<Object[]> MemberDataForGetAllExperiments =>
        new List<Object[]>
        {
            new object[] { 0, 1, new List<BusinessExperimentDto> { DataServiceMockData.businessExperimentDto1 } , new List<Experiment> { DataServiceMockData.experiment1} },
            new object[] { 1, 1, new List<BusinessExperimentDto> { DataServiceMockData.businessExperimentDto2 } , new List<Experiment> { DataServiceMockData.experiment2} }
        };

    [Theory]
    [MemberData(nameof(MemberDataForGetAllExperiments))]
    public void GetAllExperimentsByScientistId_ShouldReturnExperiments(int scientistId, int countOfResult, List<BusinessExperimentDto> ResultExperiment, List<Experiment> FilteredExperiment)
    {
        Mock.Get(_experimentRepository).Setup(p => p.GetAll()).Returns(DataServiceMockData.experiments);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessExperimentDto>>(FilteredExperiment)).Returns(ResultExperiment);

        _sut.GetAllExperiments(scientistId).Should().HaveCount(countOfResult);
        _sut.GetAllExperiments(scientistId).Should().Equal(ResultExperiment);
    }

    [Fact]
    public void GetAllExperimentsByScientistId_ShouldThrowForEmptyExperimentList()
    {
        Mock.Get(_experimentRepository).Setup(p => p.GetAll()).Returns(new List<Experiment>() { });
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessExperimentDto>>(new List<Experiment>() { })).Returns(new List<BusinessExperimentDto>() { });

        _sut.Invoking(p => p.GetAllExperiments(3)).Should().Throw<ExperimentNotPresentInLocalDatabaseException>().WithMessage("No experiments have been found in the local database.");
    }

    [Fact]
    public void GetAllExperimentsByScientistId_ShouldThrowForEmptyExperimentList2()
    {
        Mock.Get(_experimentRepository).Setup(p => p.GetAll()).Returns(DataServiceMockData.experiments);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessExperimentDto>?>(null)).Returns(value: null);

        _sut.Invoking(p => p.GetAllExperiments(3)).Should().Throw<ExperimentNotPresentInLocalDatabaseException>().WithMessage("No experiments have been found in the local database for scientist with ID: 3");
    }

    [Fact]
    public void MoveExperimentShouldWork()
    {
        Mock.Get(_experimentRepository).Setup(p => p.Update(0, 2)).Returns(DataServiceMockData.experiment1);
        Mock.Get(_mapper).Setup(p => p.Map<BusinessExperimentDto>(_experimentRepository.Update(0, 2))).Returns(DataServiceMockData.businessExperimentDto1);

        _sut.MoveExperiment(DataServiceMockData.businessExperimentDto1).Id.Should().Be(DataServiceMockData.businessExperimentDto1.Id);


    }

    [Fact]
    public void GetAllScientist_ShouldHave2Elements()
    {
        Mock.Get(_scientistRepository).Setup(p => p.GetAll()).Returns(DataServiceMockData.scientists);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessScientistDto>>(DataServiceMockData.scientists)).Returns(DataServiceMockData.bscientists);

        _sut.GetAllScientist().Should().Equal(DataServiceMockData.bscientists);
        _sut.GetAllScientist().Should().HaveCount(2);
    }

    public static IEnumerable<Object[]> GetExperimentByIdMemberData =>
        new List<Object[]>
        {
            new object [] {1, DataServiceMockData.experiment1, DataServiceMockData.businessExperimentDto1},
            new object [] {2, DataServiceMockData.experiment2, DataServiceMockData.businessExperimentDto2}
        };

    [Theory]
    [MemberData(nameof(GetExperimentByIdMemberData))]
    public void GetExperimentById_ShouldReturnTheRightExperiment(int experimentId, Experiment returnedExperiment, BusinessExperimentDto convertedExperiment)
    {
        Mock.Get(_experimentRepository).Setup(p => p.GetById(experimentId)).Returns(returnedExperiment);
        Mock.Get(_mapper).Setup(p => p.Map<BusinessExperimentDto>(returnedExperiment)).Returns(convertedExperiment);

        _sut.GetExperimentById(experimentId).Should().Be(convertedExperiment);

    }

    [Fact]
    public void MoveExperimentShouldThrowFailedToMoveExperimentExceptionWithNoInfo()
    {
        Mock.Get(_experimentRepository).Setup(p => p.Update(DataServiceMockData.businessExperimentDto3.Id, DataServiceMockData.businessExperimentDto3.ListId)).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.GetById(DataServiceMockData.businessExperimentDto3.Id)).Returns(DataServiceMockData.experiment1);
        Mock.Get(_mapper).Setup(p => p.Map<BusinessExperimentDto?>(_experimentRepository.Update(0, 2))).Returns(value: null);
        _sut.Invoking(p => p.MoveExperiment(DataServiceMockData.businessExperimentDto3)).Should().Throw<FailedToMoveExperimentException>().WithMessage($"Could not move the experiment to the list with ID: {DataServiceMockData.businessExperimentDto3.ListId}");
    }

    [Fact]
    public void MoveExperimentShouldThrowFailedToMoveExperimentExceptionWithAdditionalInfo()
    {
        Mock.Get(_experimentRepository).Setup(p => p.Update(DataServiceMockData.businessExperimentDto3.Id, DataServiceMockData.businessExperimentDto3.ListId)).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<BusinessExperimentDto?>(_experimentRepository.Update(0, 2))).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.GetById(DataServiceMockData.businessExperimentDto3.Id)).Returns(value: null);
        _sut.Invoking(p => p.MoveExperiment(DataServiceMockData.businessExperimentDto3)).Should().Throw<FailedToMoveExperimentException>().WithMessage($"Could not move the experiment to the list with ID: {DataServiceMockData.businessExperimentDto3.ListId}\nNo experiment with corresponding Id found in the database");
    }
}
