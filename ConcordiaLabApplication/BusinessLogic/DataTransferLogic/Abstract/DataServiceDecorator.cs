using BusinessLogic.DTOs.BusinessDTO;

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

    public virtual List<BusinessListDto>? GetAllLists(int scientistId)
    {
        return _component.GetAllLists(scientistId);
    }

    public virtual BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto)
    {
        return _component.MoveExperiment(businessExperimentDto);
    }
}
