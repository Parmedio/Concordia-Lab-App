using BusinessLogic.DTOs.BusinessDTO;
using PersistentLayer.Models;

namespace BusinessLogic.DataTransferLogic.Abstract;

public abstract class DataServiceDecorator : IDataService
{
    private readonly IDataService _component;

    protected DataServiceDecorator(IDataService component)
    {
        _component = component;
    }

    public virtual BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId)
    {
        return _component.AddComment(businessCommentDto, scientistId);
    }

    public IEnumerable<BusinessExperimentDto> GetAllExperiments()
    {
        return _component.GetAllExperiments();
    }

    public IEnumerable<BusinessExperimentDto> GetAllExperiments(int scientistId)
    {
        return _component.GetAllExperiments(scientistId);
    }

    public IEnumerable<BusinessListDto> GetAllLists()
    {
        return _component.GetAllLists();
    }

    public virtual IEnumerable<BusinessListDto> GetAllLists(int scientistId)
    {
        return _component.GetAllLists(scientistId);
    }

    public IEnumerable<BusinessScientistDto> GetAllScientist()
    {
        return _component.GetAllScientist();
    }

    public BusinessExperimentDto GetExperimentById(int experimentId)
    {
        return _component.GetExperimentById(experimentId);
    }

    public virtual BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto)
    {
        return _component.MoveExperiment(businessExperimentDto);
    }
}
