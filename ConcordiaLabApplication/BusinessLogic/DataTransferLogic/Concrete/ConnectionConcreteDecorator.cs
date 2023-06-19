using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.BusinessDTO;

using PersistentLayer.Repositories.Abstract;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class ConnectionConcreteDecorator : DataServiceDecorator
{
    private readonly IApiSender _sender;
    private readonly ICommentRepository _commentRepository;

    public ConnectionConcreteDecorator(IDataService component, IApiSender sender, ICommentRepository commentRepository) : base(component)
    {
        _sender = sender;
        _commentRepository = commentRepository;
    }

    public override BusinessCommentDto AddComment(BusinessCommentDto businessCommentDto, int scientistId)
    {
        var comment = base.AddComment(businessCommentDto, scientistId);
        var trelloId = _sender.AddAComment(comment.TrelloCardId, comment.CommentText, comment.Scientist!.TrelloToken);
        _commentRepository.UpdateAComment((int)comment.Id!, trelloId.Result);
        return comment;
    }

    public override BusinessExperimentDto MoveExperiment(BusinessExperimentDto businessExperimentDto)
    {
        var experiment = base.MoveExperiment(businessExperimentDto);
        _sender.UpdateAnExperiment(experiment.TrelloCardId, experiment.TrelloColumnId);
        return experiment;
    }
}
