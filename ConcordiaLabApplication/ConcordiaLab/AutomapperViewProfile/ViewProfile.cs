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
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name));

            CreateMap<BusinessExperimentDto, ViewMExperiment>()
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.LastComment, opt => opt.MapFrom(src => src.LastComment.CommentText))
                .ForMember(dest => dest.AuthorComment, opt => opt.MapFrom(src => src.LastComment.CreatorName))
                .ForMember(dest => dest.BelongToColumn, opt => opt.MapFrom(src => src.ColumnName))
                .ForMember(dest => dest.Scientists, opt => opt.MapFrom(src => src.Scientists));

            CreateMap<BusinessColumnDto, ViewMColumn>();
        }
    }
}
