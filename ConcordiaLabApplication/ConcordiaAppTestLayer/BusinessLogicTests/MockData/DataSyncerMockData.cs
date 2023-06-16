using BusinessLogic.DTOs.TrelloDtos;

using PersistentLayer.Models;

namespace ConcordiaAppTestLayer.BusinessLogicTests.MockData;

public static class DataSyncerMockData
{
    public static TrelloExperimentDto trelloExperiment1;
    public static TrelloExperimentDto trelloExperiment2;
    public static TrelloExperimentDto trelloExperiment3;
    public static TrelloExperimentDto trelloExperiment4;
    public static TrelloCommentDto trelloComment1;
    public static TrelloCommentDto trelloComment2;
    public static TrelloCommentDto trelloComment3;
    public static TrelloCommentDto trelloComment4;
    public static (IEnumerable<int>?, IEnumerable<Experiment>?) ExpectedResult1;
    public static TrelloCommentDto trelloComment5;
    public static Comment comment1;
    public static Comment comment2;
    public static Comment comment3;
    public static Comment comment1added;
    public static Comment comment2added;
    public static Comment comment1map;
    public static Experiment experiment1;
    public static Experiment experiment1map;
    public static Experiment experiment2;
    public static Experiment experiment2map;
    public static Experiment experiment3;
    public static Experiment experimentAdded;
    public static Experiment experiment3map;
    public static Experiment experiment4;
    public static Experiment experiment4map;
    public static Experiment experiment5;
    public static List<Experiment> experiments;
    public static List<Experiment> experimentsMap;
    public static List<TrelloExperimentDto> trelloExperiments;

    public static List<Comment> commentsPresent;
    public static Comment comment2map;
    public static List<TrelloCommentDto> allCommentsOnTrello;

    static DataSyncerMockData()
    {
        trelloExperiment1 = new TrelloExperimentDto()
        {
            Id = "aaa",
            Name = "esperimento1",
            IdMembers = new List<string>() { "alessandro" },
            Due = DateTime.Parse("2023-07-02T15:00:00.000Z"),
            IdLabels = new List<string>() { "ezez", "tough" },
            Desc = null
        };
        trelloExperiment2 = new TrelloExperimentDto()
        {
            Id = "bbb",
            Name = "esperimento2",
            IdMembers = new List<string>() { "marco" },
            Due = null,
            IdLabels = null,
            Desc = null
        };
        trelloExperiment3 = new TrelloExperimentDto()
        {
            Id = "ccc",
            Name = "esperimento3",
            IdMembers = new List<string>() { "alessandro", "marco" },
            Due = null,
            IdLabels = new List<string>() { "ezez" },
            Desc = null
        };
        trelloExperiment4 = new TrelloExperimentDto()
        {
            Id = "ddd",
            Name = "esperimento4",
            IdMembers = null,
            Due = DateTime.Parse("2023-07-02T21:00:00.000Z"),
            IdLabels = new List<string>() { "tough", "invalid" },
            Desc = null
        };

        experiment1 = new Experiment(
                1,
                "aaa",
                "esperimento1",
                null,
                DateTime.Parse("2023-07-02T21:00:00.000Z")
            )
        {
            LabelId = 3,
            ColumnId = 1,
            ScientistsIds = new List<int>() { 2 }
        };

        experiment2 = new Experiment(
                2,
                "bbb",
                "esperimento2",
                null,
                null
            )
        {
            ColumnId = 1,
            ScientistsIds = new List<int>() { 1 }
        };

        experiment3 = new Experiment(
                3,
                "ccc",
                "esperimento3",
                null,
                null
            )
        {
            LabelId = 1,
            ColumnId = 1,
            ScientistsIds = new List<int>() { 1, 2 }
        };

        experiment4 = new Experiment(
                5,
                "ddd",
                "esperimento4",
                null,
                DateTime.Parse("2023-07-02T21:00:00.000Z")
            )
        {
            LabelId = 3,
            ColumnId = 1,

        };
        experiment5 = new Experiment(
                4,
                "eee",
                "esperimento5",
                null,
                null
            )
        {
            LabelId = 3,
            ColumnId = 2,
            ScientistsIds = new List<int>() { 1, 2 }
        };

        experiment1map = new Experiment(
               0,
               "aaa",
               "esperimento1",
               null,
               DateTime.Parse("2023-07-02T21:00:00.000Z")
           )
        {

        };

        experiment2map = new Experiment(
                0,
                "bbb",
                "esperimento2",
                null,
                null
            )
        {

        };

        experiment3map = new Experiment(
                0,
                "ccc",
                "esperimento3",
                null,
                null
            )
        {

        };
        experiment4map = new Experiment(
                0,
                "ddd",
                "esperimento4",
                null,
                DateTime.Parse("2023-07-02T21:00:00.000Z")
            )
        {

        };

        experimentAdded = new Experiment(
                0,
                "ddd",
                "esperimento4",
                null,
                DateTime.Parse("2023-07-02T21:00:00.000Z")
            )
        {
            LabelId = 3,
            ColumnId = 1,
        };



        trelloExperiments = new List<TrelloExperimentDto>()
        {
            trelloExperiment1,
            trelloExperiment2,
            trelloExperiment3,
            trelloExperiment4
        };

        experiments = new List<Experiment>()
        {
            experiment1,
            experiment2,
            experiment3,
            experiment5
        };

        trelloComment1 = new TrelloCommentDto()
        {
            Id = "aaa",
            IdMemberCreator = "external1",
            Data = new DataInTrelloCommentDto()
            {
                Text = "commento1",
                Card = new CardInDataInTrelloCommentDto()
                {
                    Id = "aaa"
                }
            },
            MemberCreator = new MemberCreatorInTrelloDto()
            {
                Username = "Giovanni"
            },
            Date = DateTime.Parse("2023-07-02T21:00:00.000Z")
        };
        trelloComment2 = new TrelloCommentDto()
        {
            Id = "bbb",
            IdMemberCreator = "alessandro",
            Data = new DataInTrelloCommentDto()
            {
                Text = "comment2",
                Card = new CardInDataInTrelloCommentDto()
                {
                    Id = "aaa"
                }
            },
            MemberCreator = new MemberCreatorInTrelloDto()
            {
                Username = "Afu"
            },
            Date = DateTime.Parse("2023-01-02T21:00:00.000Z")
        };

        trelloComment3 = new TrelloCommentDto()
        {
            Id = "ccc",
            IdMemberCreator = "external2",
            Data = new DataInTrelloCommentDto()
            {
                Text = "commento3",
                Card = new CardInDataInTrelloCommentDto()
                {
                    Id = "ccc"
                }
            },
            MemberCreator = new MemberCreatorInTrelloDto()
            {
                Username = "Francesco"
            },
            Date = DateTime.Parse("2023-07-02T21:00:00.000Z")
        };

        trelloComment4 = new TrelloCommentDto()
        {
            Id = "ddd",
            IdMemberCreator = "marco",
            Data = new DataInTrelloCommentDto()
            {
                Text = "commento4",
                Card = new CardInDataInTrelloCommentDto()
                {
                    Id = "bbb"
                }
            },
            MemberCreator = new MemberCreatorInTrelloDto()
            {
                Username = "marco"
            },
            Date = DateTime.Parse("2023-07-02T21:00:00.000Z")
        };

        trelloComment5 = new TrelloCommentDto()
        {
            Id = "eee",
            IdMemberCreator = "alessandro",
            Data = new DataInTrelloCommentDto()
            {
                Text = "commento5",
                Card = new CardInDataInTrelloCommentDto()
                {
                    Id = "eee"
                }
            },
            MemberCreator = new MemberCreatorInTrelloDto()
            {
                Username = "afu"
            },
            Date = DateTime.Parse("2023-07-02T21:00:00.000Z")
        };

        comment1 = new Comment(
            1,
            "bbb",
            "commeno2",
            DateTime.Parse("2023-01-02T21:00:00.000Z"),
            "afu"
            )
        {
            ScientistId = 2,
            ExperimentId = 1
        };
        comment2 = new Comment(
            2,
            "ddd",
            "commento4",
            DateTime.Parse("2023-01-02T21:00:00.000Z"),
            "marco"
            )
        {
            ScientistId = 1,
            ExperimentId = 2
        };
        comment3 = new Comment(
            3,
            "eee",
            "commento5",
            DateTime.Parse("2023-01-02T21:00:00.000Z"),
            "afu"
            )
        {
            ExperimentId = 5,
            ScientistId = 2
        };
        comment1map = new Comment(
            0,
            "aaa",
            "commento5",
            DateTime.Parse("2023-07-02T21:00:00.000Z"),
            "Giovanni"
            )
        {

        };
        comment1added = new Comment(
            4,
            "aaa",
            "commento5",
            DateTime.Parse("2023-07-02T21:00:00.000Z"),
            "Giovanni"
            )
        {
            ExperimentId = 1,
            ScientistId = null
        };
        comment2map = new Comment(
            0,
            "ccc",
            "commento5",
            DateTime.Parse("2023-07-02T21:00:00.000Z"),
            "Francesco"
            )
        {
            ExperimentId = 3
        };

        comment2added = new Comment(
        5,
        "ccc",
        "commento5",
        DateTime.Parse("2023-07-02T21:00:00.000Z"),
        "Francesco"
        )
        {

        };

        allCommentsOnTrello = new()
        {
            trelloComment1,
            trelloComment2,
            trelloComment3,
            trelloComment4,
            trelloComment5
        };

        commentsPresent = new()
        {
            comment1,
            comment2,
            comment3
        };

        experimentsMap = new()
        {
            experiment1map,
            experiment2map,
            experiment3map,
            experiment4map
        };

        ExpectedResult1 = (new List<int> { 1, 2 }.AsEnumerable(), new List<Experiment>() { experiment4 }.AsEnumerable());
    }
}
