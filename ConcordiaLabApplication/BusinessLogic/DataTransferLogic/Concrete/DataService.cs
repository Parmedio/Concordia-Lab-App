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
    private readonly IScientistRepository _scientistRepository;
    private readonly IMapper _mapper;

    public DataService(IListRepository listRepository, ICommentRepository commentRepository, IExperimentRepository experimentRepository, IMapper mapper, IScientistRepository scientistRepository)
    {
        _listRepository = listRepository;
        _commentRepository = commentRepository;
        _experimentRepository = experimentRepository;
        _mapper = mapper;
        _scientistRepository = scientistRepository;
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
        return businessCommentDto; //con il mapper restituire l'oggetto nel formato che serve alla view
    }

    public IEnumerable<BusinessListDto> GetAllLists(int scientistId)
    {
        IEnumerable<BusinessListDto>? businessLists;
        businessLists = _mapper.Map<IEnumerable<BusinessListDto>?>(_listRepository.GetByScientistId(scientistId));

        if (businessLists.IsNullOrEmpty())
        {
            throw new AllListsEmptyException("The database has no lists.");
        }

        return businessLists!;
    }

    public BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto)
    {
        BusinessExperimentDto? updatedExperiment = _mapper.Map<BusinessExperimentDto?>(_experimentRepository.Update(businessExperimentDto.Id, businessExperimentDto.ListId));
        if (updatedExperiment is null)
        {
            string additionalInfo = _experimentRepository.GetById(businessExperimentDto.Id) is null ? "\nNo experiment with corresponding Id found in the database" : string.Empty;
            throw new FailedToMoveExperimentException($"Could not move the experiment to the list with ID: {businessExperimentDto.ListId}{additionalInfo}");
        }
        return updatedExperiment!;
    }

    public IEnumerable<BusinessListDto> GetAllLists()
    {
        IEnumerable<BusinessListDto> businessLists;
        var debug = _listRepository.GetAll();
        businessLists = _mapper.Map<IEnumerable<BusinessListDto>>(_listRepository.GetAll());

        if (!businessLists.Any())
        {
            throw new AllListsEmptyException("The database has no lists.");
        }

        return businessLists!;
    }

    public IEnumerable<BusinessExperimentDto> GetAllExperiments()
    {
        IEnumerable<BusinessExperimentDto> businessExperiments;
        businessExperiments = _mapper.Map<IEnumerable<BusinessExperimentDto>>(_experimentRepository.GetAll());
        if (businessExperiments.IsNullOrEmpty())
            throw new ExperimentNotPresentInLocalDatabaseException("No experiments have been found in the local database.");
        return businessExperiments!;
    }

    public IEnumerable<BusinessExperimentDto> GetAllExperiments(int scientistId)
    {
        IEnumerable<BusinessExperimentDto>? businessExperiments;
        IEnumerable<Experiment> allExperiments = _experimentRepository.GetAll();
        if (allExperiments.IsNullOrEmpty())
            throw new ExperimentNotPresentInLocalDatabaseException($"No experiments have been found in the local database.");

        businessExperiments = _mapper.Map<IEnumerable<BusinessExperimentDto>?>(allExperiments.Where(p => !p.ScientistsIds.IsNullOrEmpty() && p.ScientistsIds!.Contains(scientistId)));
        if (businessExperiments.IsNullOrEmpty())
            throw new ExperimentNotPresentInLocalDatabaseException($"No experiments have been found in the local database for scientist with ID: {scientistId}");

        return businessExperiments!;
    }

    public IEnumerable<BusinessScientistDto> GetAllScientist()
        => _mapper.Map<IEnumerable<BusinessScientistDto>>(_scientistRepository.GetAll());

    public BusinessExperimentDto GetExperimentById(int experimentId)
    {
        var experiment = _experimentRepository.GetById(experimentId);
        if (experiment is null)
            throw new ExperimentNotPresentInLocalDatabaseException($"Experiment with ID: {experimentId} is not present in the local Database");
        return _mapper.Map<BusinessExperimentDto>(experiment);
    }
}
