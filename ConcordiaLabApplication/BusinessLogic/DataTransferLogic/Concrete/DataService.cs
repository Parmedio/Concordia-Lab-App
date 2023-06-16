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

    private readonly IColumnRepository _columnRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IExperimentRepository _experimentRepository;
    private readonly IScientistRepository _scientistRepository;
    private readonly IMapper _mapper;

    public DataService(IColumnRepository columnRepository, ICommentRepository commentRepository, IExperimentRepository experimentRepository, IMapper mapper, IScientistRepository scientistRepository)
    {
        _columnRepository = columnRepository;
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
        businessCommentDto.Id = addedComment.Id;
        businessCommentDto.TrelloCardId = addedComment.Experiment.TrelloId;

        businessCommentDto.Scientist = _mapper.Map<Scientist?, BusinessScientistDto?>(addedComment.Scientist);

        if (businessCommentDto.Scientist is null)
            throw new LocalCommentWithoutScientistException($"There was an error retrieving the author of the comment: {businessCommentDto.CommentText} with Id: {businessCommentDto.Id} on the experiment: {addedComment.Experiment.Title}");
        
        return businessCommentDto;
    }

    public IEnumerable<BusinessColumnDto> GetAllColumns(int scientistId)
    {
        IEnumerable<BusinessColumnDto>? businessColumns;
        businessColumns = _mapper.Map<IEnumerable<BusinessColumnDto>?>(_columnRepository.GetByScientistId(scientistId));

        if (businessColumns.IsNullOrEmpty())
        {
            throw new ColumnsNumberException("The database has no lists.");
        }

        return businessColumns!;
    }

    public BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto)
    {
        BusinessExperimentDto? updatedExperiment = _mapper.Map<BusinessExperimentDto?>(_experimentRepository.Update(businessExperimentDto.Id, businessExperimentDto.ColumnId));
        if (updatedExperiment is null)
        {
            string additionalInfo = _experimentRepository.GetById(businessExperimentDto.Id) is null ? "\nNo experiment with corresponding Id found in the database" : string.Empty;
            throw new FailedToMoveExperimentException($"Could not move the experiment to the list with ID: {businessExperimentDto.ColumnId}{additionalInfo}");
        }
        return updatedExperiment!;
    }

    public IEnumerable<BusinessColumnDto> GetAllColumns()
    {
        IEnumerable<BusinessColumnDto> businessColumns;
        var allColumns = _columnRepository.GetAll().AsEnumerable<Column>();
        businessColumns = _mapper.Map<IEnumerable<Column>, IEnumerable<BusinessColumnDto>>(allColumns);

        if (!businessColumns.Any())
        {
            throw new ColumnsNumberException("The database has no lists.");
        }

        return businessColumns!;
    }

    public IEnumerable<BusinessExperimentDto> GetAllExperiments()
    {
        IEnumerable<BusinessExperimentDto> businessExperiments;
        businessExperiments = _mapper.Map<IEnumerable<BusinessExperimentDto>>(_experimentRepository.GetAll());
        return businessExperiments!;
    }

    public IEnumerable<BusinessExperimentDto> GetAllExperiments(int scientistId)
    {
        IEnumerable<BusinessExperimentDto> businessExperiments;
        IEnumerable<Experiment> allExperiments = _experimentRepository.GetAll();
        businessExperiments = _mapper.Map<IEnumerable<BusinessExperimentDto>>(allExperiments
            .Where(p => p.Scientists != null && p.Scientists.Any(s => s.Id == scientistId)) ?? new List<Experiment>());
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

    public IEnumerable<BusinessColumnDto> GetAllSimple()
    {
        var columns = _columnRepository.GetAllSimple();
        if (columns.Count() != 3)
            throw new ColumnsNumberException($"The number of columns present in the database is wrong, obtained count: {columns.Count()}");
        return _mapper.Map<IEnumerable<BusinessColumnDto>>(columns);
    }
}
