using AutoMapper;

using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.TrelloDtos;
using BusinessLogic.Exceptions;

using Microsoft.IdentityModel.Tokens;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace BusinessLogic.DataTransferLogic.Concrete;

/// <summary>
/// riceve e salva i nuovi commenti in locale
/// </summary>
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
                    throw new ExperimentNotPresentInLocalDatabaseException("The Experiment is not saved in the local database. Try Again.");

                int? newId = _commentRepository.AddComment(commentToAdd);
                if (newId is not null)
                    addedCommentIds.Add(newId ?? -1);
                else
                    throw new AddACommentFailedException("Failed To Add comment to Database during the Download Operation from Trello");
            }
        }
        return addedCommentIds;
    }
}
