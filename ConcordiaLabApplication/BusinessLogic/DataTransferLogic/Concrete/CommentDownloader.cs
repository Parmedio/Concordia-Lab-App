using AutoMapper;

using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DTOs.TrelloDtos;

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

    public async Task<SyncResult<Comment>> DownloadComments()
    {
        SyncResult<Comment> addedComments = new SyncResult<Comment>();

        var comments = await _receiver.GetAllComments();

        if (!comments.IsNullOrEmpty())
        {
            var latestCommentsToAdd = comments!.GroupBy(p => p.Data.Card.Id).Select(g => g.OrderByDescending(p => p.Date).First());
            addedComments = SyncDatabaseWithAllLatestComments(latestCommentsToAdd);
            return addedComments;
        }
        addedComments.AppendLine("No comments found on Trello");
        return addedComments;
    }

    private SyncResult<Comment> SyncDatabaseWithAllLatestComments(IEnumerable<TrelloCommentDto> comments)
    {
        SyncResult<Comment> result = new SyncResult<Comment>();
        List<Comment> newComments = new List<Comment>();
        result.AppendLine($"Found {comments.Count()} downloadable comments candidates on Trello");
        result.AppendLine("======================================");
        foreach (var comment in comments)
        {
            result.Append($"{$" - \"{string.Concat(comment.Data.Text.Take(15))}...\"",-50}");
            if (_commentRepository.GetCommentByTrelloId(comment.Id!) is null)
            {
                Comment commentToAdd = _mapper.Map<Comment>(comment);
                commentToAdd.ScientistId = _scientistRepository.GetLocalIdByTrelloId(comment.IdMemberCreator);
                commentToAdd.ExperimentId = _experimentRepository.GetLocalIdByTrelloId(comment.Data.Card.Id) ?? -1;
                if (commentToAdd.ExperimentId == -1)
                {
                    result.AppendLine($" => The Experiment with associated Trello ID: {comment.Data.Card.Id} is not saved in the local database. Try Again.", true);
                    continue;
                }

                Comment? newComment = _commentRepository.AddComment(commentToAdd);
                if (newComment is not null)
                    newComments.Add(newComment);
                else
                    result.AppendLine($" => Failed To Add comment with text: {commentToAdd.Body} and Id: {commentToAdd.TrelloId} to the Database during the Download Operation from Trello", true);
                result.AppendLine(" => Comment saved successfully");
            }
            else
                result.AppendLine(" => Already saved in local Database.");
        }
        result.AppendLine("======================================");
        if (result._errorCount > 0)
        {
            result.AppendLine("Download ended with errors");
            result.AppendLine($"{newComments.Count()} NEW comments downloaded: ");
            result.AppendLine($"{result._errorCount} Downloads failed.");
        }
        else
        {
            result.AppendLine("Download ended successfully");
            result.AppendLine($"{newComments.Count()} NEW comments downloaded: ");
        }
        result.AppendLine("======================================");
        result.Items = newComments;
        return result;
    }
}
