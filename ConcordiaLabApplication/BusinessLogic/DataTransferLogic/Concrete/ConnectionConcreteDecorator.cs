using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.BusinessDTO;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class ConnectionConcreteDecorator : DataServiceDecorator
{
    private readonly IApiSender _sender;

    public ConnectionConcreteDecorator(IDataService component, IApiSender sender) : base(component)
    {
        _sender = sender;
    }

    public override BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId)
    {
        var comment = base.AddComment(businessCommentDto, scientistId);
        _sender.AddAComment(comment.TrelloCardId, comment.commentText, comment.scientist.TrelloToken);
        return comment;
    }

    public override List<BusinessListDto>? GetAllLists(int scientistId)
    {
        return base.GetAllLists(scientistId);
    }

    public override BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto)
    {
        var experiment = base.MoveExperiment(businessExperimentDto);
        _sender.UpdateAnExperiment(experiment.TrelloCardId, experiment.TrelloListId);
        return experiment;
    }
}
