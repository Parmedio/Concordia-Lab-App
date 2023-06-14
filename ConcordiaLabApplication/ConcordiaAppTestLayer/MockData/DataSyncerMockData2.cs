using BusinessLogic.DTOs.TrelloDtos;

using PersistentLayer.Models;

namespace ConcordiaAppTestLayer.MockData;

public class DataSyncerMockData2
{
    // 4 experiments in to do list of which 2 are new
    // 3 experiments saved in local
    // 3 comments saved in local, one new Comment


    public Experiment LocalExperiment1InToDo { get; set; }
    public Experiment LocalExperiment2InToDo { get; set; }
    public Experiment LocalExperiment3InProgress { get; set; }
    public TrelloExperimentDto TrelloExperiment1 { get; set; }
    public TrelloExperimentDto TrelloExperiment2 { get; set; }
    public TrelloExperimentDto TrelloExperiment4New { get; set; }
    public TrelloExperimentDto TrelloExperiment5New { get; set; }
    public Experiment MappedExperiment4New { get; set; }
    public Experiment MappedExperiment5New { get; set; }
    public Experiment ToBeAddedExperiment4NewWithInfo { get; set; }
    public Experiment AddedExperiment4NewWithInfo { get; }
    public Experiment ToBeAddedExperiment5NewWithInfo { get; set; }
    public Experiment AddedExperiment5NewWithInfo { get; }
    public TrelloCommentDto TrelloComment1OnCard1 { get; set; }
    public TrelloCommentDto TrelloComment1OnCard2 { get; set; }
    public TrelloCommentDto TrelloComment1OnCard3 { get; set; }
    public TrelloCommentDto TrelloComment1OnCard5New { get; set; }
    public Comment LocalComment1OnCard1 { get; set; }
    public Comment LocalComment1OnCard2 { get; set; }
    public Comment LocalComment1OnCard3 { get; set; }
    public Comment MappedComment1OnCard5New { get; set; }
    public Comment ToBeAddedComment1OnCard5NewWithInfo { get; set; }
    public Comment AddedComment1OnCard5NewWithInfo { get; }
    public List<Experiment> LocalExperimentListInit { get; set; }
    public List<TrelloExperimentDto> ExperimentsInToDoList { get; set; }
    public List<Comment> LocalCommentListInit { get; set; }
    public List<TrelloCommentDto> AllCommentsOnTrello { get; set; }
    public List<Experiment> AddedExperiments { get; set; }
    public List<int> NewCommentIds { get; set; }
    public Experiment LocalExperiment1InToDoToUpload { get; set; }

    public DataSyncerMockData2()
    {
        LocalExperiment1InToDo = new Experiment(
            1,
            "aaa",
            "esperimentoVecchio1",
            "simpleDescription",
            DateTime.Parse("2022-4-11T12:00:00.000Z")
            )
        {
            LabelId = 1,
            ColumnId = 1,
            ScientistsIds = new List<int>() { 1 }
        };

        LocalExperiment1InToDoToUpload = new Experiment(
            1,
            "aaa",
            "esperimentoVecchio1",
            "simpleDescription",
            DateTime.Parse("2022-4-11T12:00:00.000Z")
            )
        {
            Comments = new List<Comment>() { LocalComment1OnCard1 },
            LabelId = 1,
            ColumnId = 1,
            ScientistsIds = new List<int>() { 1 }
        };

        LocalExperiment2InToDo = new Experiment(
            2,
            "bbb",
            "esperimentoVecchio2",
            null,
            DateTime.Parse("2022-4-11T12:00:00.000Z")
            )
        {
            LabelId = 2,
            ColumnId = 1,
            ScientistsIds = new List<int>() { 2 }
        };

        LocalExperiment3InProgress = new Experiment(
            3,
            "ccc",
            "esperimentoVecchio3",
            "simpleDescription2",
            null
            )
        {
            LabelId = 3,
            ColumnId = 2,
        };

        TrelloExperiment1 = new TrelloExperimentDto()
        {
            Id = "aaa",
            Name = "esperimentoVecchio1",
            IdMembers = new List<string>() { "alessandro" },
            Due = DateTime.Parse("2022-4-11T12:00:00.000Z"),
            IdLabels = new List<string>() { "easy" },
            TrelloColumnId = "todo",
            Desc = "simpleDescription"
        };

        TrelloExperiment2 = new TrelloExperimentDto()
        {
            Id = "bbb",
            Name = "esperimentoVecchio2",
            IdMembers = new List<string>() { "marco" },
            Due = DateTime.Parse("2022-4-11T12:00:00.000Z"),
            IdLabels = new List<string>() { "medium" },
            TrelloColumnId = "todo",
            Desc = null
        };

        TrelloExperiment4New = new TrelloExperimentDto()
        {
            Id = "ddd",
            Name = "esperimentoNuovo1",
            IdMembers = new List<string>() { "alessandro", "marco" },
            Due = null,
            IdLabels = new List<string>() { "hard" },
            TrelloColumnId = "todo",
            Desc = null
        };

        TrelloExperiment5New = new TrelloExperimentDto()
        {
            Id = "eee",
            Name = "esperimentoNuovo2",
            IdMembers = null,
            Due = DateTime.Parse("2022-4-11T12:00:00.000Z"),
            IdLabels = null,
            TrelloColumnId = "todo",
            Desc = "nuovoEsperimento"
        };

        MappedExperiment4New = new Experiment(
            0,
            "ddd",
            "esperimentoNuovo1",
            null,
            null
            )
        {

        };



        ToBeAddedExperiment4NewWithInfo = new Experiment(
            0,
            "ddd",
            "esperimentoNuovo1",
            null,
            null
            )
        {
            LabelId = 3,
            ColumnId = 1,
            ScientistsIds = new List<int>() { 1, 2 }
        };

        AddedExperiment4NewWithInfo = new Experiment(
            4,
            "ddd",
            "esperimentoNuovo1",
            null,
            null
            )
        {
            LabelId = 3,
            ColumnId = 1,
            ScientistsIds = new List<int>() { 1, 2 }
        };

        MappedExperiment5New = new Experiment(
            0,
            "eee",
            "esperimentoNuovo2",
            "nuovoEsperimento",
            DateTime.Parse("2022-4-11T12:00:00.000Z")
            )
        {

        };

        ToBeAddedExperiment5NewWithInfo = new Experiment(
            0,
            "eee",
            "esperimentoNuovo2",
            "nuovoEsperimento",
            DateTime.Parse("2022-4-11T12:00:00.000Z")
            )
        {

            ColumnId = 1,

        };

        AddedExperiment5NewWithInfo = new Experiment(
            5,
            "eee",
            "esperimentoNuovo2",
            "nuovoEsperimento",
            DateTime.Parse("2022-4-11T12:00:00.000Z")
            )
        {

            ColumnId = 1,

        };


        TrelloComment1OnCard1 = new TrelloCommentDto()
        {
            Id = "aaa",
            IdMemberCreator = "extrenal1",
            Date = DateTime.Parse("2022-7-11T12:00:00.000Z"),
            Data = new DataInTrelloCommentDto()
            {
                Text = "primoCommentoVecchio",
                Card = new CardInDataInTrelloCommentDto()
                {
                    Id = "aaa"
                }
            },
            MemberCreator = new MemberCreatorInTrelloDto()
            {
                Username = "external1"
            }

        };

        TrelloComment1OnCard2 = new TrelloCommentDto()
        {
            Id = "bbb",
            IdMemberCreator = "alessandro",
            Date = DateTime.Parse("2022-7-11T12:00:00.000Z"),
            Data = new DataInTrelloCommentDto()
            {
                Text = "secondoCommentoVecchio",
                Card = new CardInDataInTrelloCommentDto()
                {
                    Id = "bbb"
                }
            },
            MemberCreator = new MemberCreatorInTrelloDto()
            {
                Username = "afu"
            }

        };

        TrelloComment1OnCard3 = new TrelloCommentDto()
        {
            Id = "ccc",
            IdMemberCreator = "marco",
            Date = DateTime.Parse("2022-7-11T12:00:00.000Z"),
            Data = new DataInTrelloCommentDto()
            {
                Text = "terzoCommentoVecchio",
                Card = new CardInDataInTrelloCommentDto()
                {
                    Id = "ccc"
                }
            },
            MemberCreator = new MemberCreatorInTrelloDto()
            {
                Username = "marco"
            }

        };

        TrelloComment1OnCard5New = new TrelloCommentDto()
        {
            Id = "ddd",
            IdMemberCreator = "extrenal2",
            Date = DateTime.Parse("2022-7-11T12:00:00.000Z"),
            Data = new DataInTrelloCommentDto()
            {
                Text = "nuovoCommento",
                Card = new CardInDataInTrelloCommentDto()
                {
                    Id = "eee"
                }
            },
            MemberCreator = new MemberCreatorInTrelloDto()
            {
                Username = "external2"
            }

        };

        LocalComment1OnCard1 = new Comment(
            1,
            "aaa",
            "primoCommentoVecchio",
            DateTime.Parse("2022-4-11T12:00:00.000Z"),
            "external1")
        {
            ExperimentId = 1
        };

        LocalComment1OnCard2 = new Comment(
            2,
            "bbb",
            "secondoCommentoVecchio",
            DateTime.Parse("2022-4-11T12:00:00.000Z"),
            "afu")
        {
            ExperimentId = 2,
            ScientistId = 1
        };

        LocalComment1OnCard3 = new Comment(
            3,
            "ccc",
            "terzoCommentoVecchio",
            DateTime.Parse("2022-4-11T12:00:00.000Z"),
            "marco")
        {
            ExperimentId = 3,
            ScientistId = 2

        };

        MappedComment1OnCard5New = new Comment(
            0,
            "ddd",
            "primoCommentoVecchio",
            DateTime.Parse("2022-4-11T12:00:00.000Z"),
            "external2")
        {

        };

        ToBeAddedComment1OnCard5NewWithInfo = new Comment(
            0,
            "ddd",
            "primoCommentoVecchio",
            DateTime.Parse("2022-4-11T12:00:00.000Z"),
            "external2")
        {
            ExperimentId = 5
        };

        AddedComment1OnCard5NewWithInfo = new Comment(
            4,
            "ddd",
            "primoCommentoVecchio",
            DateTime.Parse("2022-4-11T12:00:00.000Z"),
            "external1")
        {
            ExperimentId = 5
        };

        ExperimentsInToDoList = new List<TrelloExperimentDto>()
        {
            TrelloExperiment1,
            TrelloExperiment2,
            TrelloExperiment4New,
            TrelloExperiment5New
        };

        LocalExperimentListInit = new List<Experiment>()
        {
            LocalExperiment1InToDo,
            LocalExperiment2InToDo,
            LocalExperiment3InProgress
        };

        AllCommentsOnTrello = new List<TrelloCommentDto>()
        {
            TrelloComment1OnCard1,
            TrelloComment1OnCard2,
            TrelloComment1OnCard3,
            TrelloComment1OnCard5New
        };

        LocalCommentListInit = new List<Comment>()
        {
            LocalComment1OnCard1,
            LocalComment1OnCard2,
            LocalComment1OnCard3
        };

        NewCommentIds = new()
        {
            4
        };

        AddedExperiments = new List<Experiment>()
        {
            AddedExperiment4NewWithInfo,
            AddedExperiment5NewWithInfo
        };


    }
}
