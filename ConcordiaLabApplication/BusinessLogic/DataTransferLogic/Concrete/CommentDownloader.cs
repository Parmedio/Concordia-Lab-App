using AutoMapper;

using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.TrelloDtos;
using BusinessLogic.Exceptions;

using Microsoft.IdentityModel.Tokens;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class CommentDownloader : ICommentDownloader
{
    private readonly IApiReceiver _receiver;
    private readonly IExperimentRepository _experimentRepository;
    private readonly IScientistRepository _scientistRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public CommentDownloader(IApiReceiver receiver, IExperimentRepository experimentRepository, IScientistRepository scientistRepository, IMapper mapper, ICommentRepository commentRepository)
    {
        _receiver = receiver;
        _experimentRepository = experimentRepository;
        _scientistRepository = scientistRepository;
        _mapper = mapper;
        _commentRepository = commentRepository;
    }

    public async Task<IEnumerable<int>?> DownloadComments()
    {
        IEnumerable<int>? addedCommentsId = null;

        var comments = await _receiver.GetAllComments();
        if (!comments.IsNullOrEmpty())
        {
            var latestCommentsToAdd = comments!.GroupBy(p => p.Data.Card.Id).Select(g => g.OrderByDescending(p => p.Date).First());
            addedCommentsId = SyncDatabaseWithAllLatestComments(latestCommentsToAdd);
        }
        return addedCommentsId;
    }

    private List<int> SyncDatabaseWithAllLatestComments(IEnumerable<TrelloCommentDto> comments)
    {
        List<int> addedCommentIds = new List<int>();
        foreach (var comment in comments)
        {
            if (_commentRepository.GetCommentByTrelloId(comment.Id!) is null)
            {
                Comment commentToAdd = _mapper.Map<Comment>(comment);
                commentToAdd.ScientistId = _scientistRepository.GetLocalIdByTrelloId(comment.IdMemberCreator);
                commentToAdd.ExperimentId = _experimentRepository.GetLocalIdByTrelloId(comment.Data.Card.Id) ??
                    throw new ExperimentNotPresentInLocalDatabaseException($"The Experiment with associated Trello ID: {comment.Data.Card.Id} is not saved in the local database. Try Again.");

                Comment? newId = _commentRepository.AddComment(commentToAdd);
                if (newId is not null)
                    addedCommentIds.Add(newId.Id);
                else
                    throw new AddACommentFailedException($"Failed To Add comment with text: {commentToAdd.Body} and Id: {commentToAdd.TrelloId} to the Database during the Download Operation from Trello");
            }
        }
        return addedCommentIds;
    }
}
