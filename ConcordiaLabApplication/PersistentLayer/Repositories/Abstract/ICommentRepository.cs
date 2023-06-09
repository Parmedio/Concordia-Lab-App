using PersistentLayer.Models;

namespace PersistentLayer.Repositories.Abstract;

public interface ICommentRepository
{
    public int? AddComment(Comment comment);
    public Comment? GetCommentByTrelloId(string trelloId);
}

