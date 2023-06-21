using BusinessLogic.DTOs.BusinessDTO;

using ReportSender.ReportDto;

namespace BusinessLogic.DataTransferLogic.Abstract;

public abstract class DataServiceDecorator : IDataService
{
    private readonly IDataService _component;

    protected DataServiceDecorator(IDataService component)
        => _component = component;

    public virtual BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId)
        => _component.AddComment(businessCommentDto, scientistId);

    public IEnumerable<BusinessExperimentDto> GetAllExperiments()
        => _component.GetAllExperiments();

    public IEnumerable<BusinessExperimentDto> GetAllExperiments(int scientistId)
        => _component.GetAllExperiments(scientistId);

    public IEnumerable<BusinessColumnDto> GetAllColumns()
        => _component.GetAllColumns();

    public virtual IEnumerable<BusinessColumnDto> GetAllColumns(int scientistId)
        => _component.GetAllColumns(scientistId);

    public IEnumerable<BusinessScientistDto> GetAllScientist()
        => _component.GetAllScientist();

    public IEnumerable<ScientistForReportDto> GetAllScientistsWithExperiments()
        => _component.GetAllScientistsWithExperiments();

    public IEnumerable<BusinessColumnDto> GetAllSimple()
        => _component.GetAllSimple();

    public BusinessExperimentDto GetExperimentById(int experimentId)
        => _component.GetExperimentById(experimentId);

    public virtual BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto)
        => _component.MoveExperiment(businessExperimentDto);
}
