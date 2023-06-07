using AutoMapper;

using BusinessLogic.DTOs.BusinessDTO;
using BusinessLogic.DTOs.TrelloDtos;

using PersistentLayer.Models;

namespace BusinessLogic.AutomapperProfiles;

public class MainProfile : Profile
{



    public MainProfile()
    {
        CreateMap<TrelloCommentDto, Comment>()
            .ForCtorParam("TrelloId", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Body", opt => opt.MapFrom(src => src.Data.Text))
            .ForCtorParam("Date", opt => opt.MapFrom(src => src.Date))
            .ForCtorParam("CreatorName", opt => opt.MapFrom(src => src.MemberCreator.Username));
        CreateMap<BusinessCommentDto, Comment>();
        CreateMap<Comment, BusinessCommentDto>(); // Non so se e' necessario
        CreateMap<Experiment, BusinessExperimentDto>();
        CreateMap<TrelloExperimentDto, Experiment>();
        CreateMap<ListEntity, BusinessListDto>();
    }
}
