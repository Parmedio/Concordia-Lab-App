using BusinessLogic.DTOs.BusinessDTO;

using PersistentLayer.Models;

namespace ConcordiaAppTestLayer.MockData;

public static class DataServiceMockData
{
    public static List<ListEntity> allList;
    public static List<ListEntity> allListById;
    public static List<BusinessListDto> allbList;
    public static List<BusinessListDto> allbListById;
    public static BusinessExperimentDto businessExperimentDto1;
    public static BusinessExperimentDto businessExperimentDto2;
    public static BusinessExperimentDto businessExperimentDto3;
    public static Experiment experiment1;
    public static Experiment experiment2;
    public static ListEntity entityAll;
    public static ListEntity entityById;
    public static BusinessListDto businessListAll;
    public static BusinessListDto businessListById;

    static DataServiceMockData()
    {
        experiment1 = new Experiment(0, "bbb", "cacciaGrossa");
        experiment1.ScientistsIds = new List<int>() { 0 };
        experiment1.ListId = 2;
        experiment2 = new Experiment(1, "ccc", "cacciaPiccola");
        experiment2.ScientistsIds = new List<int>() { 1 };
        experiment2.ListId = 1;
        entityAll = new ListEntity(0, "aaa", "ToDo");
        entityAll.Experiments = new List<Experiment>()
        {
            experiment1,
            experiment2
        };
        entityById = new ListEntity(0, "aaa", "ToDo");
        entityById.Experiments = new List<Experiment>()
        {
            experiment1
        };

        businessExperimentDto1 = new BusinessExperimentDto()
        {
            Id = 0,
            TrelloCardId = "bbb",
            Title = "cacciaGrossa",
            ListId = 2,
            ScientistId = 0
        };

        businessExperimentDto2 = new BusinessExperimentDto()
        {
            Id = 1,
            TrelloCardId = "bbb",
            Title = "cacciaPiccola",
            ScientistId = 1
        };
        businessExperimentDto3 = new BusinessExperimentDto()
        {
            Id = 1,
            TrelloCardId = "bbb",
            Title = "cacciaPiccola",
            ScientistId = 1,
            ListId = 8
        };

        businessListAll = new BusinessListDto()
        {
            Id = 0,
            Experiments = new List<BusinessExperimentDto>()
            {
                businessExperimentDto1,
                businessExperimentDto2
            }
        };

        businessListById = new BusinessListDto()
        {
            Id = 0,
            Experiments = new List<BusinessExperimentDto>()
            {
                businessExperimentDto1
            }
        };

        allList = new List<ListEntity>()
        {
            entityAll
        };

        allListById = new List<ListEntity>()
        {
            entityById
        };

        allbList = new List<BusinessListDto>()
        {
            businessListAll
        };

        allbListById = new List<BusinessListDto>()
        {
            businessListById
        };
    }
}