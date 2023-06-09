namespace PersistentLayer.Repositories.Abstract;

public interface IScientistRepository
{
    public int? GetLocalIdByTrelloId(string trelloId);
}
