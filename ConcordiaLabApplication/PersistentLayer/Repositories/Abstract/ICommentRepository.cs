using PersistentLayer.Models;

namespace PersistentLayer.Repositories.Abstract;

public interface ICommentRepository
{
    public Comment? AddComment(Comment comment);
    public Comment? UpdateAComment(int id, string trelloId);
    public Comment? GetCommentByTrelloId(string trelloId);
}

