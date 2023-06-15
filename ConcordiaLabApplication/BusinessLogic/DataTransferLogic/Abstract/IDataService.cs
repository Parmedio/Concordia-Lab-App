using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Abstract;

public interface IDataService
{
    public IEnumerable<BusinessColumnDto> GetallColumns();
    public IEnumerable<BusinessColumnDto> GetallColumns(int scientistId);
    public IEnumerable<BusinessExperimentDto> GetAllExperiments();
    public IEnumerable<BusinessExperimentDto> GetAllExperiments(int scientistId);
    public BusinessExperimentDto GetExperimentById(int experimentId);
    public BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto);
    public IEnumerable<BusinessScientistDto> GetAllScientist();
    public BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId);

}
