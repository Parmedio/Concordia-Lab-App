using AutoMapper;

using BusinessLogic.DTOs.TrelloDtos;

using PersistentLayer.Models;

namespace BusinessLogic.AutomapperProfiles;

public class MainProfile : Profile
{



    public MainProfile()
    {
        CreateMap<TrelloCommentDto, Comment>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.DatabaseID))
            .ForCtorParam("TrelloId", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Body", opt => opt.MapFrom(src => src.Data.Text))
            .ForCtorParam("Date", opt => opt.MapFrom(src => src.Date))
            .ForCtorParam("CreatorName", opt => opt.MapFrom(src => src.MemberCreator.Username));

        CreateMap<TrelloExperimentDto, Experiment>();
    }
}
