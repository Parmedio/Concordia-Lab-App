using AutoMapper;

using BusinessLogic.DTOs.BusinessDTO;
using BusinessLogic.DTOs.TrelloDtos;

using PersistentLayer.Models;

namespace BusinessLogic.AutomapperProfiles;

public class MainProfile : Profile
{
    public MainProfile()
    {
        CreateMap<Column, BusinessColumnDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Experiments, opt => opt.MapFrom(src => src.Experiments));

        CreateMap<Scientist, BusinessScientistDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TrelloToken, opt => opt.MapFrom(src => src.TrelloToken))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<Experiment, BusinessExperimentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.DeadLine))
            .ForMember(dest => dest.ColumnId, opt => opt.MapFrom(src => src.ColumnId))
            .ForMember(dest => dest.ColumnName, opt => opt.MapFrom(src => src.Column.Title))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Label.Title))
            .ForMember(dest => dest.TrelloCardId, opt => opt.MapFrom(src => src.TrelloId))
            .ForMember(dest => dest.TrelloColumnId, opt => opt.MapFrom(src => src.ColumnId))
            .ForMember(dest => dest.LastComment, opt => opt.MapFrom(src => src.Comments.AsEnumerable()
            .OrderByDescending(comment => comment.Date)
            .FirstOrDefault()));

        CreateMap<Comment, BusinessCommentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CardID, opt => opt.MapFrom(src => src.ExperimentId))
            .ForMember(dest => dest.TrelloCardId, opt => opt.MapFrom(src => src.Experiment.TrelloId))
            .ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.Body))
            .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.CreatorName));

        CreateMap<TrelloCommentDto, Comment>()
            .ConvertUsing(src => new Comment(
                    0,
                    src.Id,
                    src.Data.Text,
                    src.Date,
                    src.MemberCreator.Username
                ));

        CreateMap<TrelloExperimentDto, Experiment>()
            .ConvertUsing(src => new Experiment(
                    0,
                    src.Id,
                    src.Name,
                    src.Desc,
                    src.Due
                ));
    }

}
