using AutoMapper;

using BusinessLogic.DTOs.BusinessDTO;
using BusinessLogic.DTOs.TrelloDtos;

using PersistentLayer.Models;

namespace BusinessLogic.AutomapperProfiles;

public class MainProfile : Profile
{
    public MainProfile()
    { //LINQ su mavigation property
        CreateMap<ListEntity, BusinessListDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title));



        CreateMap<Scientist, BusinessScientistDto>()
            .ForMember("Id", opt => opt.MapFrom(src => src.Id))
            .ForMember("TrelloToken", opt => opt.MapFrom(src => src.TrelloToken))
            .ForMember("Name", opt => opt.MapFrom(src => src.Name));


        CreateMap<Experiment, BusinessExperimentDto>()
            .ForMember("Id", opt => opt.MapFrom(src => src.Id))
            .ForMember("Title", opt => opt.MapFrom(src => src.Title))
            .ForMember("Description", opt => opt.MapFrom(src => src.Description))
            .ForMember("Date", opt => opt.MapFrom(src => src.DeadLine))
            .ForMember("ListId", opt => opt.MapFrom(src => src.ListId))
            .ForMember(dest => dest.ListName, opt => opt.MapFrom(src => src.List.Title))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Label.Title))
            .ForMember("TrelloCardId", opt => opt.MapFrom(src => src.TrelloId))
            .ForMember("TrelloListId", opt => opt.MapFrom(src => src.ListId))
            .ForMember("lastComment", opt => opt.MapFrom(src => src.Comments.AsEnumerable()
            .OrderByDescending(comment => comment.Date)
            .FirstOrDefault()));

        CreateMap<Comment, BusinessCommentDto>()
            .ForMember("Id", opt => opt.MapFrom(src => src.Id))
            .ForMember("CardID", opt => opt.MapFrom(src => src.ExperimentId))
            .ForMember("TrelloCardId", opt => opt.MapFrom(src => src.Experiment.TrelloId))
            .ForMember("CommentText", opt => opt.MapFrom(src => src.Body))
            .ForMember("CreatorName", opt => opt.MapFrom(src => src.CreatorName));

        CreateMap<TrelloCommentDto, Comment>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => 0))
            .ForCtorParam("TrelloId", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Body", opt => opt.MapFrom(src => src.Data.Text))
            .ForCtorParam("Date", opt => opt.MapFrom(src => src.Date))
            .ForCtorParam("CreatorName", opt => opt.MapFrom(src => src.MemberCreator.Username))
            .ForMember("ExperimentId", opt => opt.Ignore())
            .ForMember(dest => dest.Experiment, opt => opt.Ignore())
            .ForMember(dest => dest.Scientist, opt => opt.Ignore())
            .ForMember(dest => dest.ScientistId, opt => opt.Ignore());

        CreateMap<TrelloExperimentDto, Experiment>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => 0))
            .ForCtorParam("TrelloId", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Title", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam("Description", opt => opt.MapFrom(src => src.desc))
            .ForCtorParam("DeadLine", opt => opt.MapFrom(src => src.Due))
            .ForMember("LabelId", opt => opt.Ignore())
            .ForMember("ListId", opt => opt.Ignore())
            .ForMember(dest => dest.Label, opt => opt.Ignore())
            .ForMember(dest => dest.List, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.Scientists, opt => opt.Ignore())
            .ForMember(dest => dest.ScientistsIds, opt => opt.Ignore());

    }

}
