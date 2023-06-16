using PersistentLayer.Models;

namespace PersistentLayer.Repositories.Abstract;

public interface ICommentRepository
{
    public Comment? AddComment(Comment comment);
    public Comment UpdateAComment(Comment comment);
    public Comment? GetCommentByTrelloId(string trelloId);
}

