using AutoMapper;

using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.BusinessDTO;
using BusinessLogic.Exceptions;

using Microsoft.IdentityModel.Tokens;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class DataService : IDataService
{

    private readonly IListRepository _listRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IExperimentRepository _experimentRepository;
    private readonly IMapper _mapper;

    public DataService(IListRepository listRepository, ICommentRepository commentRepository, IExperimentRepository experimentRepository, IMapper mapper)
    {
        _listRepository = listRepository;
        _commentRepository = commentRepository;
        _experimentRepository = experimentRepository;
        _mapper = mapper;
    }

    public BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId)
    {
        Comment commentToAdd = _mapper.Map<Comment>(businessCommentDto);
        commentToAdd.ScientistId = scientistId;
        var addedComment = _commentRepository.AddComment(commentToAdd);
        if (addedComment is null)
        {
            throw new AddACommentFailedException($"Failed to add comment: \"{businessCommentDto.CommentText}\" by scientist with Id: {scientistId} ");
        }
        return businessCommentDto;
    }

    public IEnumerable<BusinessListDto> GetAllLists(int scientistId = -1)
    {
        IEnumerable<BusinessListDto>? businessLists;
        if (scientistId == -1)
        {
            businessLists = _mapper.Map<IEnumerable<BusinessListDto>?>(_listRepository.GetAll());
        }
        else
        {
            businessLists = _mapper.Map<IEnumerable<BusinessListDto>?>(_listRepository.GetByScientistId(scientistId));
        }

        if (businessLists.IsNullOrEmpty())
        {
            throw new AllListsEmptyException("The database has no lists.");
        }

        return businessLists!;
    }

    public BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto)
    {
        BusinessExperimentDto? updatedExperiment = _mapper.Map<BusinessExperimentDto?>(_experimentRepository.Move(businessExperimentDto.Id, businessExperimentDto.ListId));
        if (updatedExperiment is null)
        {
            string additionalInfo = _experimentRepository.GetById(businessExperimentDto.Id) is null ? "\nNo experiment with corresponding Id found in the database" : string.Empty;
            throw new FailedToMoveExperimentException($"Could not move the experiment to the list with ID: {businessExperimentDto.ListId}{additionalInfo}");
        }
        return updatedExperiment!;
    }


}
