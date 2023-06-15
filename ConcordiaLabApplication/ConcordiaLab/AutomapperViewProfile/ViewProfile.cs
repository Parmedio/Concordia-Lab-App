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
                .ForMember(dest => dest.ColumnName, opt => opt.MapFrom(src => src.ColumnName))
                .ForMember(dest => dest.ColumnId, opt => opt.MapFrom(src => src.ColumnId))
                .ForMember(dest => dest.Scientists, opt => opt.MapFrom(src => src.Scientists));

            CreateMap<ViewMExperiment, BusinessExperimentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.ColumnId, opt => opt.MapFrom(src => src.ColumnId))
                .ForMember(dest => dest.ColumnName, opt => opt.MapFrom(src => src.ColumnName))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
                .ForMember(dest => dest.TrelloCardId, opt => opt.Ignore())
                .ForMember(dest => dest.TrelloColumnId, opt => opt.Ignore())
                .ForMember(dest => dest.Scientists, opt => opt.Ignore())
                .ForMember(dest => dest.LastComment, opt => opt.Ignore());

            CreateMap<BusinessColumnDto, ViewMColumn>();
        }
    }
}
