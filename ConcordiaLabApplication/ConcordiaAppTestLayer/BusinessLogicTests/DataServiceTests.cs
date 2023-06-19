using AutoMapper;

using BusinessLogic.DataTransferLogic.Concrete;
using BusinessLogic.DTOs.BusinessDTO;
using BusinessLogic.Exceptions;

using ConcordiaAppTestLayer.BusinessLogicTests.MockData;

using FluentAssertions;

using Moq;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace ConcordiaAppTestLayer.BusinessLogicTests;

public class DataServiceTests
{

    private readonly ICommentRepository _commentRepository;
    private readonly IColumnRepository _columnRepository;
    private readonly IScientistRepository _scientistRepository;
    private readonly IExperimentRepository _experimentRepository;
    private readonly IMapper _mapper;
    private readonly DataService _sut;

    public DataServiceTests()
    {
        _commentRepository = Mock.Of<ICommentRepository>();
        _experimentRepository = Mock.Of<IExperimentRepository>();
        _columnRepository = Mock.Of<IColumnRepository>();
        _scientistRepository = Mock.Of<IScientistRepository>();
        _mapper = Mock.Of<IMapper>();
        _sut = new DataService(_columnRepository, _commentRepository, _experimentRepository, _mapper, _scientistRepository);
    }

    [Fact]
    public void AddACommentShouldWork()
    {
        Comment comment1 = new Comment(0, "sono un commento", DateTime.Now, "Giovanni");
        comment1.ScientistId = 2;
        BusinessCommentDto bcomment1 = new BusinessCommentDto()
        {
            CardID = 1,
            CommentText = "sono un commento",
            CreatorName = "Giovanni",
            Scientist = new BusinessScientistDto()
            {
                Id = 2
            }
        };

        Scientist scientistWithId2 = new Scientist(2, "TrelloToken", "TrelloMemberId", "Giovanni");
        Experiment experimentThatContainsComment = new Experiment(1, "TrelloCardId", "Esperimento di Prova", null, null);

        Mock.Get(_mapper).Setup(p => p.Map<Comment>(bcomment1)).Returns(comment1);
        comment1.Scientist = scientistWithId2;
        comment1.Experiment = experimentThatContainsComment;
        Mock.Get(_commentRepository).Setup(p => p.AddComment(comment1)).Returns(comment1);
        var result = _sut.AddComment(bcomment1, 2);
        result.CreatorName.Should().Be("Giovanni");
        result.Id.Should().Be(0);
        result.CardID.Should().Be(1);
        result.Scientist!.Id.Should().Be(2);
        result.CommentText.Should().Be("sono un commento");
    }

    [Fact]
    public void AddACommentShouldThrowException()
    {
        Comment comment1 = new Comment(0, "sono un commento", DateTime.Now, "Thomas") { TrelloId = "aaa" };
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
    public void GetAllColumnshouldGiveallColumns()
    {
        Mock.Get(_columnRepository).Setup(p => p.GetAll()).Returns(DataServiceMockData.allColumns);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<Column>, IEnumerable<BusinessColumnDto>>(It.IsAny<IEnumerable<Column>>())).Returns(DataServiceMockData.allbColumn);

        _sut.GetAllColumns().Should().Equal(DataServiceMockData.allbColumn, (p, g) => p.Id == g.Id);
        _sut.GetAllColumns().Should().HaveCount(1);
        _sut.GetAllColumns().FirstOrDefault()!.Experiments.Should().HaveCount(p => p == 2);
    }

    [Fact]
    public void GetAllColumnshouldGiveallColumnsByScientistId()
    {
        Mock.Get(_columnRepository).Setup(p => p.GetByScientistId(0)).Returns(DataServiceMockData.allColumnsById);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessColumnDto>?>(_columnRepository.GetByScientistId(0))).Returns(DataServiceMockData.allbColumnById);

        _sut.GetAllColumns(0).Should().Equal(DataServiceMockData.allbColumnById, (p, g) => p.Id == g.Id);
        _sut.GetAllColumns(0).Should().HaveCount(c => c == 1);
        _sut.GetAllColumns(0).FirstOrDefault()!.Experiments.Should().HaveCount(p => p == 1);
    }

    [Fact]
    public void GetAllSimpleShouldReturnTheProperColumns()
    {
        Mock.Get(_columnRepository).Setup(p => p.GetAllSimple()).Returns(DataServiceMockData.allColumns);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<Column>, IEnumerable<BusinessColumnDto>>(It.IsAny<IEnumerable<Column>>())).Returns(DataServiceMockData.allbColumn);

        _sut.GetAllColumns().Should().Equal(DataServiceMockData.allbColumn, (p, g) => p.Id == g.Id);
        _sut.GetAllColumns().Should().HaveCount(1);
        _sut.GetAllColumns().FirstOrDefault()!.Experiments.Should().HaveCount(p => p == 2);
    }

    [Fact]
    public void GetAllColumnshouldThrowException()
    {
        Mock.Get(_columnRepository).Setup(p => p.GetAll()).Returns(new List<Column>() { });
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessColumnDto>?>(_columnRepository.GetAll())).Returns(value: null);

        _sut.Invoking(p => p.GetAllColumns(2)).Should().Throw<ColumnsNumberException>().WithMessage("The database has no lists.");
    }

    [Fact]
    public void GetAllColumnshouldThrowException_ById()
    {
        Mock.Get(_columnRepository).Setup(p => p.GetByScientistId(1)).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessColumnDto>?>(_columnRepository.GetByScientistId(1))).Returns(value: null);

        _sut.Invoking(p => p.GetAllColumns(2)).Should().Throw<ColumnsNumberException>().WithMessage("The database has no lists.");
    }

    [Fact]
    public void GetAllExperiments_ShouldReturn2Experiments()
    {
        Mock.Get(_experimentRepository).Setup(p => p.GetAll()).Returns(DataServiceMockData.experiments);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessExperimentDto>>(_experimentRepository.GetAll())).Returns(DataServiceMockData.businessColumnAll.Experiments!);

        _sut.GetAllExperiments().Should().HaveCount(2);
    }

    public static IEnumerable<object[]> MemberDataForGetAllExperiments =>
        new List<object[]>
        {
            new object[] { 1, 1, new List<BusinessExperimentDto> { DataServiceMockData.businessExperimentDto1 } , new List<Experiment> { DataServiceMockData.experiment1} },
            new object[] { 2, 1, new List<BusinessExperimentDto> { DataServiceMockData.businessExperimentDto2 } , new List<Experiment> { DataServiceMockData.experiment2} }
        };

    [Theory]
    [MemberData(nameof(MemberDataForGetAllExperiments))]
    public void GetAllExperimentsByScientistId_ShouldReturnExperiments(int scientistId, int countOfResult, List<BusinessExperimentDto> resultExperiment, List<Experiment> filteredExperiment)
    {
        Mock.Get(_experimentRepository).Setup(p => p.GetAll()).Returns(DataServiceMockData.experiments);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessExperimentDto>>(filteredExperiment)).Returns(resultExperiment);

        var filtered = DataServiceMockData.experiments.Where(p => p.Scientists != null && p.Scientists.Any(s => s.Id == scientistId)) ?? new List<Experiment>();
        filteredExperiment.Should().Equal(filtered);
        _sut.GetAllExperiments(scientistId).Should().HaveCount(countOfResult);
        _sut.GetAllExperiments(scientistId).Should().Equal(resultExperiment);
    }

    [Fact]
    public void GetAllExperimentsByScientistId_ShouldHaveCountOf0()
    {
        Mock.Get(_experimentRepository).Setup(p => p.GetAll()).Returns(new List<Experiment>() { });
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessExperimentDto>>(new List<Experiment>() { })).Returns(new List<BusinessExperimentDto>() { });

        _sut.GetAllExperiments(3).Should().HaveCount(0);
    }

    [Fact]
    public void GetAllExperimentsByScientistId_ShouldReturnEmptyList()
    {
        Mock.Get(_experimentRepository).Setup(p => p.GetAll()).Returns(DataServiceMockData.experiments);
        Mock.Get(_mapper).Setup(p => p.Map<IEnumerable<BusinessExperimentDto>?>(null)).Returns(value: null);

        _sut.GetAllExperiments(3).Should().HaveCount(0);
    }

    [Fact]
    public void MoveExperimentShouldWork()
    {
        Mock.Get(_experimentRepository).Setup(p => p.Update(0, 2)).Returns(DataServiceMockData.experiment1);
        Mock.Get(_mapper).Setup(p => p.Map<BusinessExperimentDto?>(_experimentRepository.Update(0, 2))).Returns(DataServiceMockData.businessExperimentDto1);

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

    public static IEnumerable<object[]> GetExperimentByIdMemberData =>
        new List<object[]>
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
        Mock.Get(_experimentRepository).Setup(p => p.Update(DataServiceMockData.businessExperimentDto3.Id, DataServiceMockData.businessExperimentDto3.ColumnId)).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.GetById(DataServiceMockData.businessExperimentDto3.Id)).Returns(DataServiceMockData.experiment1);
        Mock.Get(_mapper).Setup(p => p.Map<BusinessExperimentDto?>(_experimentRepository.Update(0, 2))).Returns(value: null);
        _sut.Invoking(p => p.MoveExperiment(DataServiceMockData.businessExperimentDto3)).Should().Throw<FailedToMoveExperimentException>().WithMessage($"Could not move the experiment to the list with ID: {DataServiceMockData.businessExperimentDto3.ColumnId}");
    }

    [Fact]
    public void MoveExperimentShouldThrowFailedToMoveExperimentExceptionWithAdditionalInfo()
    {
        Mock.Get(_experimentRepository).Setup(p => p.Update(DataServiceMockData.businessExperimentDto3.Id, DataServiceMockData.businessExperimentDto3.ColumnId)).Returns(value: null);
        Mock.Get(_mapper).Setup(p => p.Map<BusinessExperimentDto?>(_experimentRepository.Update(0, 2))).Returns(value: null);
        Mock.Get(_experimentRepository).Setup(p => p.GetById(DataServiceMockData.businessExperimentDto3.Id)).Returns(value: null);
        _sut.Invoking(p => p.MoveExperiment(DataServiceMockData.businessExperimentDto3)).Should().Throw<FailedToMoveExperimentException>().WithMessage($"Could not move the experiment to the list with ID: {DataServiceMockData.businessExperimentDto3.ColumnId}\nNo experiment with corresponding Id found in the database");
    }
}
