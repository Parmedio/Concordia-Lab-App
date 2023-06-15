using BusinessLogic.DTOs.BusinessDTO;

using PersistentLayer.Models;

namespace ConcordiaAppTestLayer.BusinessLogicTests.MockData;

public static class DataServiceMockData
{
    public static List<Column> allColumns;
    public static List<Experiment> experiments;
    public static List<Column> allColumnsById;
    public static List<BusinessColumnDto> allbColumn;
    public static List<BusinessColumnDto> allbColumnById;
    public static BusinessExperimentDto businessExperimentDto1;
    public static BusinessExperimentDto businessExperimentDto2;
    public static BusinessExperimentDto businessExperimentDto3;
    public static Experiment experiment1;
    public static Experiment experiment2;
    public static Scientist scientist1;
    public static BusinessScientistDto bscientist1;
    public static BusinessScientistDto bscientist2;

    public static Scientist scientist2;
    public static List<Scientist> scientists;
    public static List<BusinessScientistDto> bscientists;
    public static Column entityAll;
    public static Column entityById;
    public static BusinessColumnDto businessColumnAll;
    public static BusinessColumnDto businessColumnById;

    static DataServiceMockData()
    {
        scientist1 = new Scientist(1, "aaa", "bbb", "Giorgio Giovanni");
        scientist2 = new Scientist(2, "ccc", "ddd", "Alessandro Giovanni");
        scientists = new List<Scientist>()
        {
            scientist1,
            scientist2
        };

        bscientist1 = new BusinessScientistDto()
        {
            Id = 1,
            TrelloToken = "aaa",
            Name = "Giorgio Giovanni"
        };

        bscientist2 = new BusinessScientistDto()
        {
            Id = 2,
            TrelloToken = "ccc",
            Name = "Alessandro Giovanni"
        };

        bscientists = new()
        {
            bscientist1,
            bscientist2
        };


        experiment1 = new Experiment(0, "bbb", "cacciaGrossa");
        experiment1.ScientistsIds = new List<int>() { 1 };
        experiment1.Scientists = new List<Scientist>() { scientist1 };

        experiment1.ColumnId = 2;
        experiment2 = new Experiment(1, "ccc", "cacciaPiccola");
        experiment2.ScientistsIds = new List<int>() { 2 };
        experiment2.Scientists = new List<Scientist>() { scientist2 };
        experiment2.ColumnId = 1;
        entityAll = new Column(0, "aaa", "ToDo");
        entityAll.Experiments = new List<Experiment>()
        {
            experiment1,
            experiment2
        };
        experiments = new List<Experiment>()
        {
            experiment1,
            experiment2
        };
        entityById = new Column(0, "aaa", "ToDo");
        entityById.Experiments = new List<Experiment>()
        {
            experiment1
        };

        businessExperimentDto1 = new BusinessExperimentDto()
        {
            Id = 0,
            TrelloCardId = "bbb",
            Title = "cacciaGrossa",
            ColumnId = 2,

        };

        businessExperimentDto2 = new BusinessExperimentDto()
        {
            Id = 1,
            TrelloCardId = "bbb",
            Title = "cacciaPiccola",
            ColumnId = 0

        };
        businessExperimentDto3 = new BusinessExperimentDto()
        {
            Id = 1,
            TrelloCardId = "bbb",
            Title = "cacciaPiccola",

            ColumnId = 8
        };

        businessColumnAll = new BusinessColumnDto()
        {
            Id = 0,
            Experiments = new List<BusinessExperimentDto>()
            {
                businessExperimentDto1,
                businessExperimentDto2
            }
        };

        businessColumnById = new BusinessColumnDto()
        {
            Id = 0,
            Experiments = new List<BusinessExperimentDto>()
            {
                businessExperimentDto1
            }
        };

        allColumns = new List<Column>()
        {
            entityAll
        };

        allColumnsById = new List<Column>()
        {
            entityById
        };

        allbColumn = new List<BusinessColumnDto>()
        {
            businessColumnAll
        };

        allbColumnById = new List<BusinessColumnDto>()
        {
            businessColumnById
        };
    }
}