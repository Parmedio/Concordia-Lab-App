using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Abstract;

public interface IDataService
{
    public IEnumerable<BusinessColumnDto> GetAllColumns();
    public IEnumerable<BusinessColumnDto> GetAllColumns(int scientistId);
    public IEnumerable<BusinessColumnDto> GetAllSimple();
    public IEnumerable<BusinessExperimentDto> GetAllExperiments();
    public IEnumerable<BusinessExperimentDto> GetAllExperiments(int scientistId);
    public BusinessExperimentDto GetExperimentById(int experimentId);
    public BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto);
    public IEnumerable<BusinessScientistDto> GetAllScientist();
    public BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId);

}
