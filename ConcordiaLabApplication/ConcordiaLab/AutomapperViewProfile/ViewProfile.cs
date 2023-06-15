using AutoMapper;

using BusinessLogic.DTOs.BusinessDTO;

using ConcordiaLab.ViewModels;

namespace ConcordiaLab.AutomapperViewProfile
{
    public class ViewProfile : Profile
    {
        public ViewProfile()
        {
            CreateMap<BusinessScientistDto, ViewMScientist>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<BusinessExperimentDto, ViewMExperiment>()
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.LastComment, opt => opt.MapFrom(src => src.lastComment.CommentText))
                .ForMember(dest => dest.AuthorComment, opt => opt.MapFrom(src => src.lastComment.CreatorName))
                .ForMember(dest => dest.BelongToList, opt => opt.MapFrom(src => src.ColumnName))
                .ForMember(dest => dest.Scientists, opt => opt.MapFrom(src => src.Scientists));

            CreateMap<BusinessColumnDto, ViewMColumn>();
        }
    }
}
